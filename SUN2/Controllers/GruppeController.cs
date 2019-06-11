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
    public class GruppeController : Controller
    {
        private SUN2Entities db = new SUN2Entities();


        // Gibt eine Liste mit allen vorhandenen Gruppen zurück
        // GET: Gruppe
        public ActionResult Index()
        {
            List<Gruppe> entries = new List<Gruppe>();
            List<Person> zuordnung = new List<Person>();
            Boolean ok = false; // für prüfung auf mitgliedschaft
            Boolean[] verantwortlich = new Boolean[10000]; // Index gruppenid, Eintrag false -> Gruppe gruppenid kein Verantwortlicher
            Boolean[] mitglied = new Boolean[10000]; // Index gruppenid, Eintrag false -> Gruppe gruppenid kein mitglied

            foreach (Gruppe gruppe in db.Gruppes)
            {
                // Wenn die Gruppe privat ist, dann prüfe, ob User Mitglied der Gruppe ist
                // falls ja oder gruppe nicht privat, dann OK und weiter (Admin sieht alles immer)

                // Signal an Frontend, ob User auch Verantwortlicher ist und somit bearbeiten/löschen darf
                var userId = User.Identity.GetUserId();
                verantwortlich[gruppe.gruppenid] = AuthCheck.VerantGr(gruppe.gruppenid, userId);
                mitglied[gruppe.gruppenid] = AuthCheck.MitgliedGr(gruppe.gruppenid, userId);

                if (gruppe.privat == true && !User.IsInRole("Admin")) 
                {
                    // hier werden PRIVATEN alle Gruppen ausgeschlossen, in denen der User nicht Mitglied ist
                    ok = false;


                    foreach (MitgliederGruppe mg in db.MitgliederGruppes)
                    {
                        if (mg.gruppenid == gruppe.gruppenid && mg.userid == userId)
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
                        if (person.id == gruppe.verantwortlicher)
                        {
                            gruppe.verantwortlicher = HelpFunctions.GetDisplayName(person.id);

                            // zuordnungstabelle für verlinkung erstellen
                            Person el = new Person();
                            el.id = person.id;
                            el.name = gruppe.verantwortlicher;

                            zuordnung.Add(el);
                        }
                    }
                    entries.Add(gruppe);
                }
            }
                

            ViewBag.zuordnung = zuordnung;
            ViewBag.verantwortlich = verantwortlich;
            ViewBag.mitglied = mitglied;

            return View(entries);
        }


        // Ermöglicht das Erstellen einer neuen Gruppe
        // GET: Gruppe/Create
        public ActionResult Create()
        {
            return View();
        }


        // Ermöglicht das Erstellen einer neuen Gruppe
        // POST: Gruppe/Create
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

                // Mitgliedschaft herstellen
                MitgliederGruppe mg = new MitgliederGruppe();
                mg.gruppenid = gruppe.gruppenid;
                mg.userid = userId;
                db.MitgliederGruppes.Add(mg);

                db.SaveChanges();
                return RedirectToAction("Index");  
            }

            return View(gruppe);
        }


        // Ermöglicht das Bearbeiten einer bestimmten Gruppe (Import: GruppenID, Export GruppeModel)
        // GET: Gruppe/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var userId = User.Identity.GetUserId();
            int idauth = (int)id;
            if (AuthCheck.VerantGr(idauth, userId) || User.IsInRole("Admin"))
            {

                Gruppe gruppe = db.Gruppes.Find(id);
                if (gruppe == null)
                {
                    return HttpNotFound();
                }
                return View(gruppe);
            }

            return RedirectToAction("Unauthorized", "Error");

        }


        // Ermöglicht das Bearbeiten einer bestimmten Gruppe (Import: GruppeModel, Export: GruppeModel)
        // POST: Gruppe/Edit/5
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


        // Ermöglicht das Löschen einer bestimmten Gruppe (Import: GruppenID, Export: GruppeModel)
        // GET: Gruppe/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var userId = User.Identity.GetUserId();
            int idauth = (int)id;
            if (AuthCheck.VerantGr(idauth, userId) || User.IsInRole("Admin"))
            {
                Gruppe gruppe = db.Gruppes.Find(id);
                if (gruppe == null)
                {
                    return HttpNotFound();
                }
                return View(gruppe);
            }

            return RedirectToAction("Unauthorized", "Error");

        }


        // Ermöglicht das Löschen einer bestimmten Gruppe (Import: GruppenID, Export: -)
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
