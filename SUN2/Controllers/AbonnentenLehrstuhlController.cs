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

                db.AbonnentenLehrstuhls.Add(al);
                db.SaveChanges();
                return RedirectToAction("Index", "Lehrstuhl");
            } else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }


        // Ermöglich das Deabonnieren eines bestimtmen Lehrstuhls (Import: lehrstuhlid,  Export: redirect)
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


        // wird zurzeit nicht benötigt, bitte drinlassen!
        /*
        // GET: AbonnentenLehrstuhl
        public ActionResult Index()
        {
            return View(db.AbonnentenLehrstuhls.ToList());
        }

        // GET: AbonnentenLehrstuhl/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AbonnentenLehrstuhl abonnentenLehrstuhl = db.AbonnentenLehrstuhls.Find(id);
            if (abonnentenLehrstuhl == null)
            {
                return HttpNotFound();
            }
            return View(abonnentenLehrstuhl);
        }

        // GET: AbonnentenLehrstuhl/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AbonnentenLehrstuhl/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "userid,lehrstuhlid")] AbonnentenLehrstuhl abonnentenLehrstuhl)
        {
            if (ModelState.IsValid)
            {
                db.AbonnentenLehrstuhls.Add(abonnentenLehrstuhl);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(abonnentenLehrstuhl);
        }

        // GET: AbonnentenLehrstuhl/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AbonnentenLehrstuhl abonnentenLehrstuhl = db.AbonnentenLehrstuhls.Find(id);
            if (abonnentenLehrstuhl == null)
            {
                return HttpNotFound();
            }
            return View(abonnentenLehrstuhl);
        }

        // POST: AbonnentenLehrstuhl/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "userid,lehrstuhlid")] AbonnentenLehrstuhl abonnentenLehrstuhl)
        {
            if (ModelState.IsValid)
            {
                db.Entry(abonnentenLehrstuhl).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(abonnentenLehrstuhl);
        }

        // GET: AbonnentenLehrstuhl/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AbonnentenLehrstuhl abonnentenLehrstuhl = db.AbonnentenLehrstuhls.Find(id);
            if (abonnentenLehrstuhl == null)
            {
                return HttpNotFound();
            }
            return View(abonnentenLehrstuhl);
        }

        // POST: AbonnentenLehrstuhl/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            AbonnentenLehrstuhl abonnentenLehrstuhl = db.AbonnentenLehrstuhls.Find(id);
            db.AbonnentenLehrstuhls.Remove(abonnentenLehrstuhl);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        */


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
