using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Backoffice.Models;
using Backoffice.Models.Backoffice;
using Backoffice.Models.DB;
using Backoffice.Models.Petiscos;
using Newtonsoft.Json;

namespace Backoffice.Controllers
{
    public class PetiscosController : Controller
	{
        private BackofficeContext db = new BackofficeContext();

        public ActionResult Index()
        {
			return View(db.Petiscos.ToList());
        }

        public JsonResult Details(int id)
        {
            return Json(new { foo = id, baz = "Blech" }, JsonRequestBehavior.AllowGet);
		}

  //      public ContentResult Teste()
		//{
   //         String p = (new Random()).Next(100).ToString();
			//StaticBackoffice.bo.adicionaPetisco(p);
        //    string json = JsonConvert.SerializeObject(StaticBackoffice.bo.Petiscos);
        //    return Content(json, "application/json");
        //}


		public ActionResult Create()
        {
            return View(new Petisco());
        } 

        [HttpPost]
        public ActionResult Create(Petisco p)
        {
			try
			{
				if (ModelState.IsValid)
				{
                    db.Petiscos.Add(p);
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
            Petisco p = db.Petiscos.Find(id);
            if (p == null)
            {
                return HttpNotFound();
            }
            return View(p);
        }

        [HttpPost]
        public ActionResult Edit(int? id, FormCollection collection)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
            var p = db.Petiscos.Find(id);
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
            Petisco p = db.Petiscos.Find(id);
            db.Petiscos.Remove(p);
			db.SaveChanges();
			return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
		{
			Petisco p = db.Petiscos.Find(id);
			db.Petiscos.Remove(p);
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