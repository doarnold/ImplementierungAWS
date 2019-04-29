using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using SUN2.Models;

namespace SUN2.Controllers
{
    public class GruppenEintraegeController : Controller
    {
        private SUN2Entities db = new SUN2Entities();

        // GET: GruppenEintraege
        public ActionResult Index()
        {
            return View(db.GruppenEintraeges.ToList());
        }

        // GET: GruppenEintraege/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GruppenEintraege gruppenEintraege = db.GruppenEintraeges.Find(id);
            if (gruppenEintraege == null)
            {
                return HttpNotFound();
            }
            return View(gruppenEintraege);
        }

        // neuen eintrag erstellen = mit gruppenid aus gruppentabelle heraus
        // GET: GruppenEintraege/Create
        public ActionResult Create(int? gruppenid)
        {
            if (gruppenid == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // gruppenid, autor, datum im hintergrund vorbelegen
            GruppenEintraege gruppenEintraege = new GruppenEintraege();
            gruppenEintraege.gruppenid = (int)gruppenid;
            gruppenEintraege.datum = DateTime.Now;

            var userId = User.Identity.GetUserId();
            gruppenEintraege.autor = userId;

            return View(gruppenEintraege);
        }

        // POST: GruppenEintraege/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,gruppenid,datum,autor,inhalt,label1,label2,label3,label4,label5")] GruppenEintraege gruppenEintraege)
        {
            if (ModelState.IsValid)
            {
                db.GruppenEintraeges.Add(gruppenEintraege);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(gruppenEintraege);
        }

        // GET: GruppenEintraege/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GruppenEintraege gruppenEintraege = db.GruppenEintraeges.Find(id);
            if (gruppenEintraege == null)
            {
                return HttpNotFound();
            }
            return View(gruppenEintraege);
        }

        // POST: GruppenEintraege/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,gruppenid,datum,autor,inhalt,label1,label2,label3,label4,label5")] GruppenEintraege gruppenEintraege)
        {
            if (ModelState.IsValid)
            {
                db.Entry(gruppenEintraege).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(gruppenEintraege);
        }

        // GET: GruppenEintraege/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GruppenEintraege gruppenEintraege = db.GruppenEintraeges.Find(id);
            if (gruppenEintraege == null)
            {
                return HttpNotFound();
            }
            return View(gruppenEintraege);
        }

        // POST: GruppenEintraege/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            GruppenEintraege gruppenEintraege = db.GruppenEintraeges.Find(id);
            db.GruppenEintraeges.Remove(gruppenEintraege);
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
