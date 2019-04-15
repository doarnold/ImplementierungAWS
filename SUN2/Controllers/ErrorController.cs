using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SUN2.Controllers
{
    public class ErrorController : Controller
    {
        // liefert eine Fehlermeldung, wenn User nicht eingeloggt ist
        // GET: Error/NotLoggedIn
        public ActionResult NotLoggedIn()
        {
            return View();
        }

        // liefert eine Fehlermeldung bei fehlender Berechtigung
        // GET Error/Unauthorized
        public ActionResult Unauthorized()
        {
            return View();
        }
    }
}