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
    public class LehrstuhlController : Controller
    {
        private SUN2Entities db = new SUN2Entities();

        // Gibt eine Liste mit allen vorhandenen Lehrstühlen zurück
        // GET: Lehrstuhl
        public ActionResult Index()
        {
            List<Lehrstuhl> entries = new List<Lehrstuhl>();

            foreach (Lehrstuhl lehrstuhl in db.Lehrstuhls)
            {
                foreach (Person person in db.Person)
                {
                    if (person.id == lehrstuhl.verantwortlicher)
                    {
                        //Kombination aus Vorname+Nachname+E-Mail anzeigen statt techn. User ID als Verantwortlicher
                        if (person.name != null && person.vorname != null)
                        {
                            lehrstuhl.verantwortlicher = person.vorname + " " + person.name + " (" + person.AspNetUsers.Email + ")";
                        }
                        else
                        {
                            lehrstuhl.verantwortlicher = person.AspNetUsers.Email;
                        }
                    }
                }
                entries.Add(lehrstuhl);
            }

            // Um abonnieren/deabonnieren zu kennzeichnen, ViewBag mit allen Abos des Users an Frontend senden
            List<AbonnentenLehrstuhl> list = new List<AbonnentenLehrstuhl>();

            //angemeldeter User 
            var userId = User.Identity.GetUserId();

            // Alle Abos des angemeldeten Users suchen
            foreach (AbonnentenLehrstuhl al in db.AbonnentenLehrstuhls)
            {
                if (al.userid == userId)
                {
                    list.Add(al);
                }
            }

            ViewBag.abos = list;

            return View(entries);
        }


        // Ermöglicht das Erstellen eines neuen Lehrstuhls
        // GET: Lehrstuhl/Create
        public ActionResult Create()
        {
            return View();
        }


        // Ermöglicht das Erstellen eines neuen Lehrstuhls (Import: LehrstuhlModel, Export: LehrstuhlModel)
        // POST: Lehrstuhl/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "lehrstuhlid,bezeichnung,beschreibung")] Lehrstuhl lehrstuhl)
        {
            if (ModelState.IsValid)
            {
                //angemeldeter User ist Verantwortlicher
                var userId = User.Identity.GetUserId();
                lehrstuhl.verantwortlicher = userId;

                db.Lehrstuhls.Add(lehrstuhl);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(lehrstuhl);
        }


        // Ermglicht das Bearbeiten eines Lehrstuhls (Import: LehrstuhlID, Export: LehrstuhlModel)
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


        // Ermglicht das Bearbeiten eines Lehrstuhls (Import: LehrstuhlID, Export: LehrstuhlModel)
        // POST: Lehrstuhl/Edit/5
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


        // Wird zurzeit nicht verwendet, bitte drinlassen!
        /*
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
        } */


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
