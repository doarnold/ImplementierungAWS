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
    public class LehrstuhlEintraegeController : Controller
    {
        private SUN2Entities db = new SUN2Entities();

        // GET: LehrstuhlEintraege
        public ActionResult Index()
        {
            return View(db.LehrstuhlEintraeges.ToList());
        }

        // GET: LehrstuhlEintraege/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LehrstuhlEintraege lehrstuhlEintraege = db.LehrstuhlEintraeges.Find(id);
            if (lehrstuhlEintraege == null)
            {
                return HttpNotFound();
            }
            return View(lehrstuhlEintraege);
        }

        // GET: LehrstuhlEintraege/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: LehrstuhlEintraege/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,lehrstuhlid,datum,autor,inhalt,label1,label2,label3,label4,label5")] LehrstuhlEintraege lehrstuhlEintraege)
        {
            if (ModelState.IsValid)
            {
                db.LehrstuhlEintraeges.Add(lehrstuhlEintraege);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(lehrstuhlEintraege);
        }

        // GET: LehrstuhlEintraege/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LehrstuhlEintraege lehrstuhlEintraege = db.LehrstuhlEintraeges.Find(id);
            if (lehrstuhlEintraege == null)
            {
                return HttpNotFound();
            }
            return View(lehrstuhlEintraege);
        }

        // POST: LehrstuhlEintraege/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,lehrstuhlid,datum,autor,inhalt,label1,label2,label3,label4,label5")] LehrstuhlEintraege lehrstuhlEintraege)
        {
            if (ModelState.IsValid)
            {
                db.Entry(lehrstuhlEintraege).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(lehrstuhlEintraege);
        }

        // GET: LehrstuhlEintraege/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LehrstuhlEintraege lehrstuhlEintraege = db.LehrstuhlEintraeges.Find(id);
            if (lehrstuhlEintraege == null)
            {
                return HttpNotFound();
            }
            return View(lehrstuhlEintraege);
        }

        // POST: LehrstuhlEintraege/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            LehrstuhlEintraege lehrstuhlEintraege = db.LehrstuhlEintraeges.Find(id);
            db.LehrstuhlEintraeges.Remove(lehrstuhlEintraege);
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
