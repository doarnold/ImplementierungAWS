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
    public class GruppeController : Controller
    {
        private SUN2Entities db = new SUN2Entities();

        // GET: Gruppe
        public ActionResult Index()
        {
            return View(db.Gruppes.ToList());
        }

        // GET: Gruppe/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Gruppe gruppe = db.Gruppes.Find(id);
            if (gruppe == null)
            {
                return HttpNotFound();
            }
            return View(gruppe);
        }

        // GET: Gruppe/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Gruppe/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "gruppenid,bezeichnung,beschreibung,verantwortlicher,privat")] Gruppe gruppe)
        {
            if (ModelState.IsValid)
            {
                db.Gruppes.Add(gruppe);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(gruppe);
        }

        // GET: Gruppe/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Gruppe gruppe = db.Gruppes.Find(id);
            if (gruppe == null)
            {
                return HttpNotFound();
            }
            return View(gruppe);
        }

        // POST: Gruppe/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "gruppenid,bezeichnung,beschreibung,verantwortlicher,privat")] Gruppe gruppe)
        {
            if (ModelState.IsValid)
            {
                db.Entry(gruppe).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(gruppe);
        }

        // GET: Gruppe/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Gruppe gruppe = db.Gruppes.Find(id);
            if (gruppe == null)
            {
                return HttpNotFound();
            }
            return View(gruppe);
        }

        // POST: Gruppe/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Gruppe gruppe = db.Gruppes.Find(id);
            db.Gruppes.Remove(gruppe);
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
