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
    public class LehrstuhlController : Controller
    {
        private SUN2Entities db = new SUN2Entities();

        // GET: Lehrstuhl
        public ActionResult Index()
        {
            return View(db.Lehrstuhls.ToList());
        }

        // GET: Lehrstuhl/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Lehrstuhl lehrstuhl = db.Lehrstuhls.Find(id);
            if (lehrstuhl == null)
            {
                return HttpNotFound();
            }
            return View(lehrstuhl);
        }

        // GET: Lehrstuhl/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Lehrstuhl/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "lehrstuhlid,bezeichnung,beschreibung,verantwortlicher,privat")] Lehrstuhl lehrstuhl)
        {
            if (ModelState.IsValid)
            {
                db.Lehrstuhls.Add(lehrstuhl);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(lehrstuhl);
        }

        // GET: Lehrstuhl/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Lehrstuhl lehrstuhl = db.Lehrstuhls.Find(id);
            if (lehrstuhl == null)
            {
                return HttpNotFound();
            }
            return View(lehrstuhl);
        }

        // POST: Lehrstuhl/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "lehrstuhlid,bezeichnung,beschreibung,verantwortlicher,privat")] Lehrstuhl lehrstuhl)
        {
            if (ModelState.IsValid)
            {
                db.Entry(lehrstuhl).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(lehrstuhl);
        }

        // GET: Lehrstuhl/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Lehrstuhl lehrstuhl = db.Lehrstuhls.Find(id);
            if (lehrstuhl == null)
            {
                return HttpNotFound();
            }
            return View(lehrstuhl);
        }

        // POST: Lehrstuhl/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Lehrstuhl lehrstuhl = db.Lehrstuhls.Find(id);
            db.Lehrstuhls.Remove(lehrstuhl);
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
