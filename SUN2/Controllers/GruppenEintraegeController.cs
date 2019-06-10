using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using SUN2.Models;

namespace SUN2.Controllers
{
    public class GruppenEintraegeController : Controller
    {
        private SUN2Entities db = new SUN2Entities();


        // Liefert eine Liste von Gruppeneinträgen. Diese List enthält nur die Einträge einer bestimmten Gruppe, falls
        // eine GruppenID übergeben wird, ansonsten werden die Einträge aller Gruppen zurückgegeben 
        // (Import optional: GruppenID, Export: GruppenEinträge List)
        // GET: GruppenEintraege
        public ActionResult Index(int? gruppenid)
        {
            List <GruppenEintraege> entries = new List<GruppenEintraege>();
            List<Person> zuordnung = new List<Person>();
            String userid = "";
            Boolean ok = false; // für prüfung auf mitgliedschaft
            Boolean[] verantwortlich = new Boolean[10000]; // Index gruppenid, Eintrag false -> Gruppe gruppenid kein Verantwortlicher
            Boolean[] autor = new Boolean[10000]; // Index gruppenid, Eintrag false -> Gruppe gruppenid kein autor

            foreach (Gruppe gruppe in db.Gruppes)
            {
                // Wenn die Gruppe privat ist, dann prüfe, ob User Mitglied der Gruppe ist
                // falls ja oder gruppe nicht privat, dann OK und weiter (Admin sieht alles immer)
                if (gruppe.gruppenid == gruppenid && gruppe.privat == true && !User.IsInRole("Admin"))
                {
                    // hier werden PRIVATEN alle Gruppen ausgeschlossen, in denen der User nicht Mitglied ist
                    var userId = User.Identity.GetUserId();
                    ok = false;

                    // Signal an Frontend, ob User auch Verantwortlicher ist und somit bearbeiten/löschen darf
                    if (gruppe.verantwortlicher == userId)
                    {
                        verantwortlich[gruppe.gruppenid] = true;
                    }
                    else
                    {
                        verantwortlich[gruppe.gruppenid] = false;
                    }

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
            }

            if (ok)
            {
                if (gruppenid == null)
                {
                    return View(db.GruppenEintraeges.ToList());
                }
                else
                {
                    foreach (GruppenEintraege ge in db.GruppenEintraeges)
                    {
                        if (ge.gruppenid == gruppenid)
                        {

                            // Signal an Frontend, ob User auch autor ist und bearbeiten/löschen darf
                            var userId = User.Identity.GetUserId();

                            if (ge.autor == userId)
                            {
                                autor[ge.id] = true;
                            }
                            else
                            {
                                autor[ge.id] = false;
                            }


                            foreach (Person person in db.Person)
                            {
                                if (person.id == ge.autor)
                                {
                                    //Kombination aus Vorname+Nachname+E-Mail anzeigen statt techn. User ID als Verantwortlicher
                                    if (person.name != null && person.vorname != null)
                                    {
                                        ge.autor = person.vorname + " " + person.name + " (" + person.AspNetUsers.Email + ")";
                                    }
                                    else
                                    {
                                        ge.autor = person.AspNetUsers.Email;
                                    }

                                    userid = person.id;
                                }
                            }

                            // zuordnungstabelle für verlinkung erstellen
                            Person el = new Person();
                            el.id = userid;
                            el.name = ge.autor;

                            zuordnung.Add(el);

                            entries.Add(ge);
                        }
                    }

                    ViewBag.zuordnung = zuordnung;
                    ViewBag.autor = autor;
                    ViewBag.verantwortlich = verantwortlich;

                    // Bezeichnung der Gruppe in View übergeben
                    foreach (Gruppe gr in db.Gruppes)
                    {
                        if (gr.gruppenid == gruppenid)
                        {
                            ViewBag.bezeichnung = gr.bezeichnung;
                        }
                    }
                }



            }
            return View(entries.ToList());
           
            
        }


        // Ermöglicht es, einen neuen Gruppeneintrag in einer bestimmten Gruppe zu erstellen 
        // (Import: GruppenID, Export: gruppenEintraegeModel)
        // GET: GruppenEintraege/Create
        public ActionResult Create(int? gruppenid)
        {
            if (gruppenid == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // gruppenid, autor, datum im hintergrund vorbelegen
            GruppenEintraege gruppenEintraege = new GruppenEintraege();
            gruppenEintraege.gruppenid = (int)gruppenid;
            gruppenEintraege.datum = DateTime.Now;

            var userId = User.Identity.GetUserId();
            gruppenEintraege.autor = userId;

            return View(gruppenEintraege);
        }


        // Ermöglicht es, einen neuen Gruppeneintrag in einer bestimmten Gruppe zu erstellen 
        // (Import: gruppenEintrageModel, Export: gruppenEintraegeModel)
        // POST: GruppenEintraege/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,gruppenid,datum,autor,inhalt,label1,label2,label3,label4,label5")] GruppenEintraege gruppenEintraege)
        {
            if (ModelState.IsValid)
            {
                db.GruppenEintraeges.Add(gruppenEintraege);
                db.SaveChanges();
                return RedirectToAction("Index", new { gruppenid = gruppenEintraege.gruppenid } );
            }

            return View(gruppenEintraege);
        }


        // Ermöglicht es, einen bestimmten Gruppeneintrag zu bearbeiten (Import: GruppenEintragsID, Export: gruppenEintrageModel)
        // GET: GruppenEintraege/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GruppenEintraege gruppenEintraege = db.GruppenEintraeges.Find(id);
            if (gruppenEintraege == null)
            {
                return HttpNotFound();
            }
            return View(gruppenEintraege);
        }


        // Ermöglicht es, einen bestimmten Gruppeneintrag zu bearbeiten (Import: gruppenEintrageModel, Export: gruppenEintrageModel)
        // POST: GruppenEintraege/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,gruppenid,datum,autor,inhalt,label1,label2,label3,label4,label5")] GruppenEintraege gruppenEintraege)
        {
            if (ModelState.IsValid)
            {
                db.Entry(gruppenEintraege).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { gruppenid = gruppenEintraege.gruppenid });
            }
            return View(gruppenEintraege);
        }


        // Ermöglicht es, einen bestimmten Gruppeneintrag zu löschen (Import: gruppenEintragID, Export: gruppenEintrageModel)
        // GET: GruppenEintraege/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GruppenEintraege gruppenEintraege = db.GruppenEintraeges.Find(id);
            if (gruppenEintraege == null)
            {
                return HttpNotFound();
            }
            return View(gruppenEintraege);
        }


        // Ermöglicht es, einen bestimmten Gruppeneintrag zu löschen (Import: gruppenEintrageModel, Export: gruppenEintrageModel)
        // POST: GruppenEintraege/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            GruppenEintraege gruppenEintraege = db.GruppenEintraeges.Find(id);
            db.GruppenEintraeges.Remove(gruppenEintraege);
            db.SaveChanges();
            return RedirectToAction("Index", new { gruppenid = gruppenEintraege.gruppenid });
        }


        // Wird zurzeit nicht benötigt und ist deswegen auskommentiert!
        /*
        // GET: GruppenEintraege/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GruppenEintraege gruppenEintraege = db.GruppenEintraeges.Find(id);
            if (gruppenEintraege == null)
            {
                return HttpNotFound();
            }
            return View(gruppenEintraege);
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
