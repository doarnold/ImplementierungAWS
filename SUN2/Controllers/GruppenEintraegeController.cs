using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
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
        public ActionResult Details(string id)
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

        // GET: GruppenEintraege/Create
        public ActionResult Create()
        {
            return View();
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
        public ActionResult Edit(string id)
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
        public ActionResult Delete(string id)
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
        public ActionResult DeleteConfirmed(string id)
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
