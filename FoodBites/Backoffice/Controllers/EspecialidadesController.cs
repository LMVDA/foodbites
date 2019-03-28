using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Backoffice.Models.DB;
using Backoffice.Models.Petiscos;

namespace Backoffice.Controllers
{
    public class EspecialidadesController : Controller
	{
		private BackofficeContext db = new BackofficeContext();

		public ActionResult Index()
		{
			db.Configuration.ProxyCreationEnabled = false;
			List<Especialidade> es = db.Especialidades
									   .Include("Caracteristicas")
									   .Include("Estabelecimento")
									   .Include("Petisco")
                                       .ToList();
			foreach (var e in es)
			{
                //string key = e.ID.ToString();
                //ViewData[key] = new SelectList(e.Caracteristicas, "Texto");
                Console.WriteLine("ID:" +e.ID);
                foreach(var car in e.Caracteristicas) {
                    Console.WriteLine(car.Texto);
                }
			}
			return View(es);
		}

		public JsonResult Details(int id)
		{
			return Json(new { foo = id, baz = "Blech" }, JsonRequestBehavior.AllowGet);
		}

		public ActionResult Create()
		{
            PopulateEstabelecimentosDropDownList();
            PopulatePetiscosDropDownList();
            var e = new Especialidade
            {
                Caracteristicas = new List<Caracteristica>
				{
					new Caracteristica{Texto=""},
					new Caracteristica{Texto=""},
					new Caracteristica{Texto=""},
					new Caracteristica{Texto=""},
					new Caracteristica{Texto=""}
                }
            };
			return View(e);
		}

		[HttpPost]
		public ActionResult Create(Especialidade p, HttpPostedFileBase file)
		{
			try
			{
				if (ModelState.IsValid)
				{
                    List<Caracteristica> l = p.Caracteristicas.Where(c => c.Texto != null).ToList();
                    p.Caracteristicas = l;
                    if (file != null && file.ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(file.FileName);
                        var path = Path.Combine(Server.MapPath("~/App_Data/uploads"), fileName);
                        file.SaveAs(path);
                        p.Fotografia = file.FileName;
                        //p.Fotografia = path;
                    }
					db.Especialidades.Add(p);
					db.SaveChanges();
					return RedirectToAction("Index");
				}
			}
			catch (RetryLimitExceededException /* dex */)
			{
				//Log the error (uncomment dex variable name and add a line here to write a log.
				ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
			}
			PopulateEstabelecimentosDropDownList(p.EstabelecimentoID);
            PopulatePetiscosDropDownList(p.PetiscoID);
			return View(p);

		}

		public ActionResult Edit(int? id)
		{
			db.Configuration.ProxyCreationEnabled = false;

            
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}

            Especialidade p = db.Especialidades
                                .Include("Caracteristicas")
                                .Include("Estabelecimento")
                                .Include("Petisco")
                                .ToList()
                                .First(x => x.ID == id);
            
			//Especialidade p = db.Especialidades.Find(id);
			if (p == null)
			{
				return HttpNotFound();
			}

			PopulateEstabelecimentosDropDownList(p.EstabelecimentoID);
            PopulatePetiscosDropDownList(p.PetiscoID);
			return View(p);
		}

		[HttpPost]
		public ActionResult Edit(int? id, FormCollection collection)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}

            Especialidade p = db.Especialidades
                                .Include("Caracteristicas")
                                .Include("Estabelecimento")
                                .Include("Petisco")
                                .ToList()
								.First(x => x.ID == id);
			//Console.WriteLine("ID: " + p.ID);
			//foreach (var car in p.Caracteristicas)
			//{
			//	Console.WriteLine("id-> " + car.ID + " ->" + car.Texto);
			//}
            //var p = db.Especialidades.Find(id);
            var Cars = db.Caracteristicas.Include("Especialidade");

            //Console.WriteLine("Cars 1");
            //foreach (var car in Cars)
            //{
            //    Console.WriteLine("id-> " + car.ID + " ->" + car.Texto);
            //}


			if (TryUpdateModel(p))
			{
				foreach (var car in p.Caracteristicas.ToList())
				{
					Console.WriteLine("0:id -> " + car.ID + " ->" + car.Texto);
					var c = db.Caracteristicas.Find(car.ID);
					Console.WriteLine("1:id -> " + c.ID + " ->" + c.Texto);
					c.Texto = car.Texto;
					c.Especialidade = p;
					//TryUpdateModel(c);
					Console.WriteLine("2:id -> " + c.ID + " ->" + c.Texto);
                    db.Entry(c).State = EntityState.Modified;
				}

				TryUpdateModel(Cars);

				//Console.WriteLine("ID2: " + p.ID);
				//foreach (var car in p.Caracteristicas)
				//{
				//	Console.WriteLine("id-> "+car.ID + " ->"+car.Texto);
				//}
				//Console.WriteLine("Cars 2");
				//foreach (var car in Cars)
				//{
				//	Console.WriteLine("id-> " + car.ID + " ->" + car.Texto);
				//}

				//Console.WriteLine("Pid " + p.PetiscoID);
				//Console.WriteLine("P " + p.Petisco);
    //            Console.WriteLine("Eid " + p.EstabelecimentoID);
				//Console.WriteLine("E " + p.Estabelecimento);
				//Console.WriteLine("Cars " + p.Caracteristicas);
                //Console.WriteLine("Revs " + p.Reviews);

				try
				{
					db.SaveChanges();

					return RedirectToAction("Index");
				}
				catch (RetryLimitExceededException /* dex */)
				{
					//Log the error (uncomment dex variable name and add a line here to write a log.
					ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
				}
			}
			PopulateEstabelecimentosDropDownList(p.EstabelecimentoID);
            PopulatePetiscosDropDownList(p.PetiscoID);
			return View(p);
		}

		public ActionResult Delete(int id)
		{
			Especialidade p = db.Especialidades.Find(id);
			db.Especialidades.Remove(p);
			db.SaveChanges();
			return RedirectToAction("Index");
		}

		[HttpPost]
		public ActionResult Delete(int id, FormCollection collection)
		{
			Especialidade p = db.Especialidades.Find(id);
			db.Especialidades.Remove(p);
			db.SaveChanges();
			return RedirectToAction("Index");
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				db.Dispose();
			}
			base.Dispose(disposing);
		}


		private void PopulateEstabelecimentosDropDownList(object selectedEstabelecimento = null)
		{
			db.Configuration.ProxyCreationEnabled = false;
			//var est = from d in db.Estabelecimentos
							//orderby d.Nome
							//select d;
			var est = db.Estabelecimentos.OrderBy(q => q.Nome).ToList();
			ViewBag.EstabelecimentoID = new SelectList(est, "ID", "Nome", selectedEstabelecimento);
		}

		private void PopulatePetiscosDropDownList(object selectedPetisco = null)
		{
			db.Configuration.ProxyCreationEnabled = false;
            var est = db.Petiscos.OrderBy(q => q.Nome).ToList();
			ViewBag.PetiscoID = new SelectList(est, "ID", "Nome", selectedPetisco);
		}
	}
}