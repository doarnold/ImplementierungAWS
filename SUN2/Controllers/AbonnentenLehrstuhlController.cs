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
    public class AbonnentenLehrstuhlController : Controller
    {
        private SUN2Entities db = new SUN2Entities();


        // Ermöglicht das Abonnieren eines bestimmten Lehrstuhls (import: Lehrstuhlid, Export: redirect)
        // leitet bei erfolg zur der lehrstuhlübersicht weiter
        // POST: AbonnentenLehrstuhl/Abo/5
        public ActionResult Abo(int? lehrstuhlid)
        {
            if (lehrstuhlid != null)
            {
                AbonnentenLehrstuhl al = new AbonnentenLehrstuhl();
                al.lehrstuhlid = (int)lehrstuhlid;

                //angemeldeter User 
                var userId = User.Identity.GetUserId();
                al.userid = userId;


                // Prüfen, ob diese Verknüpfung bereits exisitert
                // Falls ja, dann nicht erneut hinzufügen
                Boolean exist = false;
                foreach ( AbonnentenLehrstuhl abonnenten in db.AbonnentenLehrstuhls)
                {
                    if(abonnenten.userid == userId && abonnenten.lehrstuhlid == lehrstuhlid)
                    {
                        exist = true;
                    }
                }

                if(!exist)
                {
                    db.AbonnentenLehrstuhls.Add(al);
                    db.SaveChanges();
                }

                return RedirectToAction("Index", "Lehrstuhl");

            } else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }


        // Ermöglich das Deabonnieren eines bestimtmen Lehrstuhls (Import: lehrstuhlid,  Export: redirect)
        // leitet bei erfolg zur Seite der Lehrstuhlübersicht weiter
        // POST: AbonnentenLehrstuhl/Deabo/5
        public ActionResult Deabo(int? lehrstuhlid)
        {
            //angemeldeter User 
            var userId = User.Identity.GetUserId();
            AbonnentenLehrstuhl abonnentenLehrstuhl = new AbonnentenLehrstuhl();

            // Korrekten Eintrag suchen
            foreach ( AbonnentenLehrstuhl al in db.AbonnentenLehrstuhls )
            {
                if ( al.userid == userId && al.lehrstuhlid == lehrstuhlid)
                {
                    abonnentenLehrstuhl = al;
                }
            }

            db.AbonnentenLehrstuhls.Remove(abonnentenLehrstuhl);
            db.SaveChanges();
            return RedirectToAction("Index", "Lehrstuhl");
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
