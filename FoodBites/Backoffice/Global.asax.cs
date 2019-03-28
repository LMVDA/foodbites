using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Http;

using System.Data.SqlClient;
using Backoffice.Models.Backoffice;
using System;
using Newtonsoft.Json;
using Backoffice.Models.DB;
using Backoffice.Models.Petiscos;
using System.Data.Common;

namespace Backoffice
{
    public class Global : HttpApplication
    {
		void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            // testar conexão DB
            using (var db = new BackofficeContext())
            {
                DbConnection conn = db.Database.Connection;
                try
                {
                    conn.Open();   // check the database connection
                    Console.WriteLine("\n\nNICE CONNECTION DB\n\n");
                }
                catch
                {
                    Console.WriteLine("\n\nFAILLLLLx2 DB\n\n");
                }
            }




   //         StaticBackoffice.bo = new FacadeBackoffice();
			//StaticBackoffice.bo.adicionaPetisco("inicio");

			//using (var db = new BackofficeContext())
			//{
			//	DbConnection conn = db.Database.Connection;
			//	try
			//	{
			//		conn.Open();   // check the database connection
   //                 Console.WriteLine("\n\nNICE CONNECTION DB\n\n");
			//	}
			//	catch
			//	{
			//		Console.WriteLine("\n\nFAILLLLL DB\n\n");
			//	}
			//}

   //         BackofficeContext db = new BackofficeContext();
			//String p = (new Random()).Next(100).ToString();
			//db.Petiscos.Add(new Petisco { Nome = p });
            //db.SaveChanges();

        }
    }
}
