using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using Backoffice.Models.DB;
using Backoffice.Models.Petiscos;

namespace Backoffice.Controllers
{
    public class EstabelecimentosController : Controller
    {

		private BackofficeContext db = new BackofficeContext();

		public ActionResult Index()
		{

            db.Configuration.ProxyCreationEnabled = false;
            List<Estabelecimento> es = db.Estabelecimentos
                                         .Include("Localizacao")
                                         //.Include("Horarios")
                                         .ToList();
			return View(es);
		}

		public JsonResult Details(int id)
		{
			return Json(new { foo = id, baz = "Blech" }, JsonRequestBehavior.AllowGet);
		}

		public ActionResult Create()
		{
            var e = new Estabelecimento
            {
                Horarios = new List<HorarioFuncionamento>{
					new HorarioFuncionamento{Dia = DayOfWeek.Monday},
                    new HorarioFuncionamento{Dia = DayOfWeek.Tuesday},
                    new HorarioFuncionamento{Dia = DayOfWeek.Wednesday},
                    new HorarioFuncionamento{Dia = DayOfWeek.Thursday},
                    new HorarioFuncionamento{Dia = DayOfWeek.Friday},
					new HorarioFuncionamento{Dia = DayOfWeek.Saturday},
                    new HorarioFuncionamento{Dia = DayOfWeek.Sunday}
                }
            };
			return View(e);
		}

		[HttpPost]
		public ActionResult Create(Estabelecimento p)
		{
			try
			{
				if (ModelState.IsValid)
				{
					db.Estabelecimentos.Add(p);
					db.SaveChanges();
					return RedirectToAction("Index");
				}
			}
			catch (RetryLimitExceededException /* dex */)
			{
				//Log the error (uncomment dex variable name and add a line here to write a log.
				ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
			}
			return View(p);

		}

		public ActionResult Edit(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}

			db.Configuration.ProxyCreationEnabled = false;

            Estabelecimento p = db.Estabelecimentos
                                  .Include("Horarios")
                                  .Include("Localizacao")
                                  .ToList()
                                  .First(x => x.ID == id);
			//Estabelecimento p = db.Estabelecimentos.Find(id);
			if (p == null)
			{
				return HttpNotFound();
			}

            Console.WriteLine("ID:" +p.ID);
            // isto vai dar erro estranho a fazer load dos horários
            // tentar guardar uma string no datetime...
            //db.Entry(p).Collection(x => x.Horarios).Load();

            if(p.Horarios != null) {
				foreach (var h in p.Horarios)
				{
					Console.WriteLine("abre:" + h.HoraAbertura.ToString());
					Console.WriteLine("fecha:" + h.HoraFecho.ToString());
				}
            }
            Console.WriteLine("acabei");
			return View(p);
		}

		[HttpPost]
		public ActionResult Edit(int? id, FormCollection collection)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			var p = db.Estabelecimentos.Find(id);
			if (TryUpdateModel(p))
			{
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
			return View(p);
		}

		public ActionResult Delete(int id)
		{
			Estabelecimento p = db.Estabelecimentos.Find(id);
			db.Estabelecimentos.Remove(p);
			db.SaveChanges();
			return RedirectToAction("Index");
		}

		[HttpPost]
		public ActionResult Delete(int id, FormCollection collection)
		{
			Estabelecimento p = db.Estabelecimentos.Find(id);
			db.Estabelecimentos.Remove(p);
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

    }
}
