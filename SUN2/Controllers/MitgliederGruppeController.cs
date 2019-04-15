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
    public class MitgliederGruppeController : Controller
    {
        private SUN2Entities db = new SUN2Entities();

        // GET: MitgliederGruppe
        public ActionResult Index()
        {
            return View(db.MitgliederGruppes.ToList());
        }

        // GET: MitgliederGruppe/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MitgliederGruppe mitgliederGruppe = db.MitgliederGruppes.Find(id);
            if (mitgliederGruppe == null)
            {
                return HttpNotFound();
            }
            return View(mitgliederGruppe);
        }

        // GET: MitgliederGruppe/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: MitgliederGruppe/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "userid,gruppenid")] MitgliederGruppe mitgliederGruppe)
        {
            if (ModelState.IsValid)
            {
                db.MitgliederGruppes.Add(mitgliederGruppe);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(mitgliederGruppe);
        }

        // GET: MitgliederGruppe/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MitgliederGruppe mitgliederGruppe = db.MitgliederGruppes.Find(id);
            if (mitgliederGruppe == null)
            {
                return HttpNotFound();
            }
            return View(mitgliederGruppe);
        }

        // POST: MitgliederGruppe/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "userid,gruppenid")] MitgliederGruppe mitgliederGruppe)
        {
            if (ModelState.IsValid)
            {
                db.Entry(mitgliederGruppe).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(mitgliederGruppe);
        }

        // GET: MitgliederGruppe/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MitgliederGruppe mitgliederGruppe = db.MitgliederGruppes.Find(id);
            if (mitgliederGruppe == null)
            {
                return HttpNotFound();
            }
            return View(mitgliederGruppe);
        }

        // POST: MitgliederGruppe/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            MitgliederGruppe mitgliederGruppe = db.MitgliederGruppes.Find(id);
            db.MitgliederGruppes.Remove(mitgliederGruppe);
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
