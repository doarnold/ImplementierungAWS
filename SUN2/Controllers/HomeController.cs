using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using SUN2.Models;

namespace SUN2.Controllers
{
    public class HomeController : Controller
    {
        private SUN2Entities db = new SUN2Entities();
        //liefert die index view /index
        public ActionResult Index()
        {

            var lehrstuehle = db.Lehrstuhls.ToList();

            return View(lehrstuehle);
        }

        //liefert die about view /about
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        //liefert die contact view /contact
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        //liefert die administrationsübersicht
        public ActionResult Administration()
        {
            return View();
        }
    }
}