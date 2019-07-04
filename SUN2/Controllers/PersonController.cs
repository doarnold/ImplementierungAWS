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
    public class PersonController : Controller
    {
        private SUN2Entities db = new SUN2Entities();

        // Liefert eine Übersicht aller Personen
        // import: keine
        // export: personenModel als liste
        // GET: Person
        public ActionResult Index()
        {
            var person = db.Person.Include(p => p.AspNetUsers);
            return View(person.ToList());
        }


        // Ermöglicht das Anzeigen von personendetails
        // import: userid
        // export: personenmodel
        // GET: Person/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Person person = db.Person.Find(id);
            if (person == null)
            {
                return HttpNotFound();
            }
            return View(person);
        }


        // Ermöglicht das Anlegen einer neuen Person
        // GET: Person/Create
        public ActionResult Create()
        {
            ViewBag.id = new SelectList(db.AspNetUsers, "Id", "Email");
            return View();
        }

        // Ermöglicht das Anlegen einer neuen Person als POST
        // import: personenmodel
        // export: personenmodel
        // POST: Person/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,name,vorname,matnr,hsemester,fsemester,studienbeginn,studiengang,studienfach,email")] Person person)
        {
            if (ModelState.IsValid)
            {
                db.Person.Add(person);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.id = new SelectList(db.AspNetUsers, "Id", "Email", person.id);
            return View(person);
        }


        // Ermgöglicht das Bearbeiten einer Person
        // import: userid
        // export: personenmodel
        // GET: Person/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Person person = db.Person.Find(id);
            if (person == null)
            {
                return HttpNotFound();
            }
            ViewBag.id = new SelectList(db.AspNetUsers, "Id", "Email", person.id);
            return View(person);
        }


        // Ermöglicht das Bearbeiten einer Person
        // import: personenmodel
        // export: personenmodel
        // POST: Person/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,name,vorname,matnr,hsemester,fsemester,studienbeginn,studiengang,studienfach,email")] Person person)
        {
            if (ModelState.IsValid)
            {
                db.Entry(person).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.id = new SelectList(db.AspNetUsers, "Id", "Email", person.id);
            return View(person);
        }


        // Ermöglicht dem eingeloggten User, seine eigenen Daten zu bearbeiten
        // import: keine
        // export: personenmodel
        // profiledaten des eingeloggten users laden
        // GET: Person/EditMe
        public ActionResult EditMe()
        {
            var id = User.Identity.GetUserId();

            if (id == null)
            {
                return RedirectToAction("NotLoggedIn", "Error", "");
                //return new HttpStatusCodeResult(HttpStatusCode.Unauthorized, "Sie sind nicht eingeloggt.");
            }
            Person person = db.Person.Find(id);
            if (person == null)
            {
                return HttpNotFound();
            }
            ViewBag.id = new SelectList(db.AspNetUsers, "Id", "Email", person.id);
            return View(person);
        }


        // speichert die geänderten profildaten des eingeloggten users
        // import: personenmodel
        // export: personenmodel
        // POST: Person/EditMe
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditMe([Bind(Include = "id,name,vorname,matnr,hsemester,fsemester,studienbeginn,studiengang,studienfach,email")] Person person)
        {
            if (ModelState.IsValid)
            {
                db.Entry(person).State = EntityState.Modified;
                db.SaveChanges();
                ViewBag.Message = "Daten wurden erfolgreich gespeichert.";
                return RedirectToAction("EditMe");
            }
            var id = User.Identity.GetUserId();
            ViewBag.id = new SelectList(db.AspNetUsers, id, "Email", person.id);
            return View(person);
        }


        // Ermöglicht das Löschen eineer Person
        // import: userid
        // export: personenmodel
        // GET: Person/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Person person = db.Person.Find(id);
            if (person == null)
            {
                return HttpNotFound();
            }
            return View(person);
        }


        // Ermöglicht das Löschen einer Person als POST
        // import: userid
        // export: redirect
        // POST: Person/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Person person = db.Person.Find(id);
            db.Person.Remove(person);
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
