using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using SUN2.Controllers.misc;
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
            List<Person> zuordnung = new List<Person>();
            Boolean[] verantwortlich = new Boolean[10000]; // Index lehrstuhlid, Eintrag false -> lehrstuhl kein Verantwortlicher
            Boolean[] mitarbeiter = new Boolean[10000]; // Index lehrstuhlid, Eintrag false -> lehrstuhl kein mitarbeiter
            Boolean ok = false; // mitarbeiter private gruppe
            var userId = User.Identity.GetUserId();

            foreach (Lehrstuhl lehrstuhl in db.Lehrstuhls)
            {
                // Wenn der lehrstuhl privat ist, dann prüfe, ob User mitarbeiter ist
                // falls ja oder lehrstuhl nicht privat, dann OK und weiter (Admin sieht alles immer)

                // Signal an Frontend, ob User auch Verantwortlicher ist und somit bearbeiten/löschen darf
                verantwortlich[lehrstuhl.lehrstuhlid] = AuthCheck.VerantLehr(lehrstuhl.lehrstuhlid, userId);
                mitarbeiter[lehrstuhl.lehrstuhlid] = AuthCheck.MitarbeiterLehr(lehrstuhl.lehrstuhlid, userId);

                if (lehrstuhl.privat == true && !User.IsInRole("Admin"))
                {
                    // hier werden PRIVATEN alle lehrstühle ausgeschlossen, in denen der User nicht Mitarbeiter ist
                    ok = false;


                    foreach (MitarbeiterLehrstuhl ml in db.MitarbeiterLehrstuhls)
                    {
                        if (ml.lehrstuhlid == lehrstuhl.lehrstuhlid && ml.userid == userId)
                        {
                            ok = true;
                        }
                    }
                }
                else
                {
                    ok = true;
                }

                if(ok)
                {
                    foreach (Person person in db.Person)
                    {
                        if (person.id == lehrstuhl.verantwortlicher)
                        {
                            lehrstuhl.verantwortlicher = HelpFunctions.GetDisplayName(person.id);

                            // zuordnungstabelle für verlinkung erstellen
                            Person el = new Person();
                            el.id = person.id;
                            el.name = lehrstuhl.verantwortlicher;

                            zuordnung.Add(el);
                        }
                    }
                    ViewBag.zuordnung = zuordnung;
                    entries.Add(lehrstuhl);
                }

                // Um abonnieren/deabonnieren zu kennzeichnen, ViewBag mit allen Abos des Users an Frontend senden
                List<AbonnentenLehrstuhl> list = new List<AbonnentenLehrstuhl>();

                // Alle Abos des angemeldeten Users suchen
                foreach (AbonnentenLehrstuhl al in db.AbonnentenLehrstuhls)
                {
                    if (al.userid == userId)
                    {
                        list.Add(al);
                    }
                }

                ViewBag.abos = list;

            }
            ViewBag.mitarbeiter = mitarbeiter;
            ViewBag.verantwortlich = verantwortlich;


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

            var userId = User.Identity.GetUserId();
            int idauth = (int)id;
            if (AuthCheck.VerantLehr(idauth, userId) || User.IsInRole("Admin"))
            {
                Lehrstuhl lehrstuhl = db.Lehrstuhls.Find(id);
                if (lehrstuhl == null)
                {
                    return HttpNotFound();
                }
                return View(lehrstuhl);
            }

            return RedirectToAction("Unauthorized", "Error");
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


            var userId = User.Identity.GetUserId();
            int idauth = (int)id;
            if (AuthCheck.VerantLehr(idauth, userId) || User.IsInRole("Admin"))
            {
                Lehrstuhl lehrstuhl = db.Lehrstuhls.Find(id);
                if (lehrstuhl == null)
                {
                    return HttpNotFound();
                }
                return View(lehrstuhl);
             }

            return RedirectToAction("Unauthorized", "Error");
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
