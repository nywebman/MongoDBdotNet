using MongoDB.Driver;
using MongoDBdotNet.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MongoDBdotNet.Controllers
{
    public class HomeController : Controller
    {

        public RealEstateContext Context = new RealEstateContext();
        public ActionResult Index()
        {
          //  return View();
            Context.Database.GetStats();
            return Json(Context.Database.Server.BuildInfo, JsonRequestBehavior.AllowGet);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}