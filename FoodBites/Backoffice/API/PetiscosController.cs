using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using Backoffice.Models.DB;
using Backoffice.Models.Petiscos;

namespace Backoffice.API
{
    public class PetiscosController : ApiController
    {
		private BackofficeContext db = new BackofficeContext();

        // GET: api/petiscos
        public IEnumerable<Petisco> GetPetiscos() {
            return db.Petiscos.AsEnumerable();
        }


        // GET: api/petiscos/5
        public Petisco GetPetisco(int id) {
            Petisco p = db.Petiscos.Find(id);
            if(p == null) {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }
            return p;
        }
    }
}
