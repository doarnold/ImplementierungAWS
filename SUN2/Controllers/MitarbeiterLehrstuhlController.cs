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
    public class MitarbeiterLehrstuhlController : Controller
    {
        private SUN2Entities db = new SUN2Entities();

        // GET: MitarbeiterLehrstuhl
        public ActionResult Index()
        {
            return View(db.MitarbeiterLehrstuhls.ToList());
        }

        // GET: MitarbeiterLehrstuhl/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MitarbeiterLehrstuhl mitarbeiterLehrstuhl = db.MitarbeiterLehrstuhls.Find(id);
            if (mitarbeiterLehrstuhl == null)
            {
                return HttpNotFound();
            }
            return View(mitarbeiterLehrstuhl);
        }

        // GET: MitarbeiterLehrstuhl/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: MitarbeiterLehrstuhl/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "userid,lehrstuhlid")] MitarbeiterLehrstuhl mitarbeiterLehrstuhl)
        {
            if (ModelState.IsValid)
            {
                db.MitarbeiterLehrstuhls.Add(mitarbeiterLehrstuhl);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(mitarbeiterLehrstuhl);
        }

        // GET: MitarbeiterLehrstuhl/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MitarbeiterLehrstuhl mitarbeiterLehrstuhl = db.MitarbeiterLehrstuhls.Find(id);
            if (mitarbeiterLehrstuhl == null)
            {
                return HttpNotFound();
            }
            return View(mitarbeiterLehrstuhl);
        }

        // POST: MitarbeiterLehrstuhl/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "userid,lehrstuhlid")] MitarbeiterLehrstuhl mitarbeiterLehrstuhl)
        {
            if (ModelState.IsValid)
            {
                db.Entry(mitarbeiterLehrstuhl).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(mitarbeiterLehrstuhl);
        }

        // GET: MitarbeiterLehrstuhl/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MitarbeiterLehrstuhl mitarbeiterLehrstuhl = db.MitarbeiterLehrstuhls.Find(id);
            if (mitarbeiterLehrstuhl == null)
            {
                return HttpNotFound();
            }
            return View(mitarbeiterLehrstuhl);
        }

        // POST: MitarbeiterLehrstuhl/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            MitarbeiterLehrstuhl mitarbeiterLehrstuhl = db.MitarbeiterLehrstuhls.Find(id);
            db.MitarbeiterLehrstuhls.Remove(mitarbeiterLehrstuhl);
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
