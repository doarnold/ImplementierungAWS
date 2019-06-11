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
    public class LehrstuhlEintraegeController : Controller
    {
        private SUN2Entities db = new SUN2Entities();

        // Liefert eine Liste von Lehrstuhleinträgen. Diese List enthält nur die Einträge eines bestimmten Lehrstuhls, falls
        // eine lehrstuhlID übergeben wird, ansonsten werden die Einträge aller lehrstühle zurückgegeben 
        // (Import optional: lehrstuhlID, Export: lehrstuhlEinträge List)
        // GET: LehrstuhlEintraege
        public ActionResult Index(int? lehrstuhlid)
        {
            List<LehrstuhlEintraege> entries = new List<LehrstuhlEintraege>();
            List<Person> zuordnung = new List<Person>();
            String userid = "";
            Boolean ok = false; // für prüfung auf mitgliedschaft
            Boolean[] verantwortlich = new Boolean[10000]; // Index gruppenid, Eintrag false -> Gruppe gruppenid kein Verantwortlicher
            Boolean[] autor = new Boolean[10000]; // Index gruppenid, Eintrag false -> Gruppe gruppenid kein autor
            var userId = User.Identity.GetUserId();

            foreach (Lehrstuhl l in db.Lehrstuhls)
            {
                // Wenn der lehrstuhl privat ist, dann prüfe, ob User mitarbeiter des lehrstuhls ist
                // falls ja oder lehrstuhl nicht privat, dann OK und weiter (Admin sieht alles immer)
                if (l.lehrstuhlid == lehrstuhlid && l.privat == true && !User.IsInRole("Admin"))
                {
                    // hier werden PRIVATEN alle lehrstühle ausgeschlossen, in denen der User nicht mitarbeiter ist
                    ok = false;

                    // Signal an Frontend, ob User auch Verantwortlicher ist und somit bearbeiten/löschen darf
                    autor[l.lehrstuhlid] = AuthCheck.AutorLE(l.lehrstuhlid, userId);
                    verantwortlich[l.lehrstuhlid] = AuthCheck.VerantGr(l.lehrstuhlid, userId);

                    foreach (MitarbeiterLehrstuhl ml in db.MitarbeiterLehrstuhls)
                    {
                        if (ml.lehrstuhlid == l.lehrstuhlid && ml.userid == userId)
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

        
            if(ok)
            {
                if (lehrstuhlid == null)
                {
                    return View(db.LehrstuhlEintraeges.ToList());
                }
                else
                {
                    foreach (LehrstuhlEintraege le in db.LehrstuhlEintraeges)
                    {
                        if (le.lehrstuhlid == lehrstuhlid)
                        {
                            foreach (Person person in db.Person)
                            {
                                if (person.id == le.autor)
                                {
                                    le.autor = HelpFunctions.GetDisplayName(person.id);

                                    userid = person.id;
                                }
                            }
                            // zuordnungstabelle für verlinkung erstellen
                            Person el = new Person();
                            el.id = userid;
                            el.name = le.autor;

                            zuordnung.Add(el);

                            entries.Add(le);
                        }
                    }

                    ViewBag.zuordnung = zuordnung;

                    // Bezeichnung des lehrstuhls in View übergeben
                    foreach (Lehrstuhl le in db.Lehrstuhls)
                    {
                        if (le.lehrstuhlid == lehrstuhlid)
                        {
                            ViewBag.bezeichnung = le.bezeichnung;
                        }
                    }
                }
  
            }

            ViewBag.autor = autor;
            ViewBag.verantwortlich = verantwortlich;

            return View(entries.ToList());
        }

        // Ermöglicht es, einen neuen Lehrstuhleintrag in einem bestimmten Lerhstuhl zu erstellen 
        // (Import: lehrstuhlID, Export: lehrstuhlEintraegeModel)
        // GET: LehrstuhlEintraege/Create
        public ActionResult Create(int? lehrstuhlid)
        {
            if (lehrstuhlid == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // lehrstuhlid, autor, datum im hintergrund vorbelegen
            LehrstuhlEintraege lehrstuhlEintraege = new LehrstuhlEintraege();
            lehrstuhlEintraege.lehrstuhlid = (int)lehrstuhlid;
            lehrstuhlEintraege.datum = DateTime.Now;

            var userId = User.Identity.GetUserId();
            lehrstuhlEintraege.autor = userId;

            return View(lehrstuhlEintraege);
        }


        // Ermöglicht es, einen neuen Lehrstuhleintrag in einem bestimmten Lerhstuhl zu erstellen 
        // (Import: lehrstuhlEintraegeModel, Export: lehrstuhlEintraegeModel)
        // POST: LehrstuhlEintraege/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,lehrstuhlid,datum,autor,inhalt,label1,label2,label3,label4,label5")] LehrstuhlEintraege lehrstuhlEintraege)
        {
            if (ModelState.IsValid)
            {
                db.LehrstuhlEintraeges.Add(lehrstuhlEintraege);
                db.SaveChanges();
                return RedirectToAction("Index", new { lehrstuhlid = lehrstuhlEintraege.lehrstuhlid });
            }

            return View(lehrstuhlEintraege);
        }


        // GET: LehrstuhlEintraege/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var userId = User.Identity.GetUserId();
            int idauth = (int)id;
            if (AuthCheck.VerantLehr(idauth, userId) || AuthCheck.AutorLE(idauth, userId) || User.IsInRole("Admin"))
            {
                LehrstuhlEintraege lehrstuhlEintraege = db.LehrstuhlEintraeges.Find(id);
                if (lehrstuhlEintraege == null)
                {
                    return HttpNotFound();
                }
                return View(lehrstuhlEintraege);
            }

            return RedirectToAction("Unauthorized", "Error");
        }


        // POST: LehrstuhlEintraege/Edit/5
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
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var userId = User.Identity.GetUserId();
            int idauth = (int)id;
            if (AuthCheck.VerantLehr(idauth, userId) || AuthCheck.AutorLE(idauth, userId) || User.IsInRole("Admin"))
            {
                    LehrstuhlEintraege lehrstuhlEintraege = db.LehrstuhlEintraeges.Find(id);
                if (lehrstuhlEintraege == null)
                {
                    return HttpNotFound();
                }
                return View(lehrstuhlEintraege);
            }

            return RedirectToAction("Unauthorized", "Error");
        }


        // POST: LehrstuhlEintraege/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
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
