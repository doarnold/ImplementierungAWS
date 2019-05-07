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
    public class GruppeController : Controller
    {
        private SUN2Entities db = new SUN2Entities();

        // GET: Gruppe
        public ActionResult Index()
        {
            List<Gruppe> entries = new List<Gruppe>();

            foreach (Gruppe gruppe in db.Gruppes)
            {
                foreach (Person person in db.Person)
                {
                    if(person.id == gruppe.verantwortlicher)
                    {
                        //Kombination aus Vorname+Nachname+E-Mail anzeigen statt techn. User ID als Verantwortlicher
                        if (person.name != null && person.vorname != null)
                        {
                            gruppe.verantwortlicher = person.vorname + " " + person.name + " (" + person.AspNetUsers.Email + ")";
                        }
                        else
                        {
                            gruppe.verantwortlicher = person.AspNetUsers.Email;
                        }
                    }
                }

                entries.Add(gruppe);
            }

            return View(entries);
        }

        // GET: Gruppe/Details/5
        public ActionResult Details(int? id)
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
        public ActionResult Create([Bind(Include = "gruppenid,bezeichnung,beschreibung,privat")] Gruppe gruppe)
        {
            if (ModelState.IsValid)
            {
                //angemeldeter User ist Verantwortlicher
                var userId = User.Identity.GetUserId();
                gruppe.verantwortlicher = userId;

                db.Gruppes.Add(gruppe);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(gruppe);
        }

        // GET: Gruppe/Edit/5
        public ActionResult Edit(int? id)
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
        public ActionResult Edit([Bind(Include = "gruppenid,bezeichnung,verantwortlicher,beschreibung,privat")] Gruppe gruppe)
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
        public ActionResult Delete(int? id)
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
        public ActionResult DeleteConfirmed(int id)
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
