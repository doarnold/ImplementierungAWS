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


        // Gibt eine Liste aller Lehrstuhlmitarbeiter für einen bestimmten Lehrstuhl zurück
        // (Import: lehrstuhlID, Export: Liste Lehrstuhlmitglieder)
        // GET: MitarbeiterLehrstuhl
        public ActionResult Index(int? lehrstuhlid)
        {
            List<MitarbeiterLehrstuhl> entries = new List<MitarbeiterLehrstuhl>();
            List<Person> zuordnung = new List<Person>();

            if (lehrstuhlid == null)
            {
                return View(db.MitarbeiterLehrstuhls.ToList());
            }
            else
            {
                foreach (MitarbeiterLehrstuhl ml in db.MitarbeiterLehrstuhls)
                {
                    if (ml.lehrstuhlid == lehrstuhlid)
                    {
                        foreach (Person person in db.Person)
                        {
                            if (ml.userid == person.id)
                            {
                                //Kombination aus Vorname+Nachname+E-Mail anzeigen statt techn. User ID
                                if (person.name != null && person.vorname != null)
                                {
                                    ml.userid = person.vorname + " " + person.name + " (" + person.AspNetUsers.Email + ")";
                                }
                                else
                                {
                                    ml.userid = person.AspNetUsers.Email;
                                }

                            }

                            // zuordnungstabelle für verlinkung erstellen
                            Person el = new Person();
                            el.id = person.id;
                            el.name = ml.userid;

                            zuordnung.Add(el);

                        }
                        entries.Add(ml);
                    }
                }

                ViewBag.zuordnung = zuordnung;

                return View(entries.ToList());
            }
        }


        // Funktionalität zum Hinzfügen neuer Mitarbeiter zu einem Lehrstuhl;
        // Methode stellt eine Liste aller verfügbaren Personen zum Hinzufügen bereit, die nicht bereits
        // Mitarbeiter des entsprechenden Lehrstuhls sind
        // ~~Import: lehrstuhlid
        // ~~Export: Liste des PersonAddLehrstuhlModel (userid, name, vorname, email, lehrstuhlid)
        // GET: MitarbeiterLehrstuhl/Add/5
        public ActionResult Add(int? lehrstuhlid)
        {
            List<PersonAddLehrstuhlModel> entries = new List<PersonAddLehrstuhlModel>();
            List<PersonAddLehrstuhlModel> enthalten = new List<PersonAddLehrstuhlModel>();

            if (lehrstuhlid == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                foreach (Person person in db.Person)
                {
                    //Liste mit allen Personen aufbauen
                    entries.Add(new PersonAddLehrstuhlModel
                    {
                        lehrstuhlid = (int)lehrstuhlid,
                        id = person.id,
                        name = person.name,
                        vorname = person.vorname,
                        email = person.AspNetUsers.Email
                    });

                    //bereits enthaltene Personen ermitteln
                    foreach (MitarbeiterLehrstuhl mit in db.MitarbeiterLehrstuhls)
                    {
                        if (person.id == mit.userid && mit.lehrstuhlid == lehrstuhlid)
                        {
                            enthalten.Add(new PersonAddLehrstuhlModel
                            {
                                lehrstuhlid = (int)lehrstuhlid,
                                id = person.id,
                                name = person.name,
                                vorname = person.vorname,
                                email = person.AspNetUsers.Email
                            });
                        }
                    }
                }

                //Alle Personen, die bereits im lehrstuhl enhalten sind, entfernen
                foreach (PersonAddLehrstuhlModel ent in enthalten)
                {
                    entries.RemoveAll(x => x.id == ent.id);
                }
            }

            return View(entries);
        }


        // Funktionalität zum Hinzfügen neuer Mitarbeiter zu einem Lehrstuhl
        // Fügt eine aus der Liste ausgwählte Perosn zum entsprechenden Lehrstuhl hinzu
        // ~~Import: lehrstuhlid, userid
        // ~~Export: nichts oder BadRequest View
        // GET: MitarbeiterLehrstuhl/AddPerson
        public ActionResult AddPerson(int? lehrstuhlid, string id)
        {
            MitarbeiterLehrstuhl mg = new MitarbeiterLehrstuhl();
            mg.userid = id;
            mg.lehrstuhlid = (int)lehrstuhlid;

            //Falls die lehrstuhlid oder die UserID nicht gesetzt ist, Fehler melden
            if (lehrstuhlid == null || id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else

            //Wenn alles okay ist, speichere einen neuen Eintrag in der MitarbeiterLehrstuhl Tabelle und 
            //leite zur Index Seite von Lerhstuhl um
            if (ModelState.IsValid)
            {
                db.MitarbeiterLehrstuhls.Add(mg);
                db.SaveChanges();
                return RedirectToAction("Index", "Lehrstuhl");
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

        }


        // Ermöglicht das Entfernen eines Mitarbeiters (Import: id der Verknüpfung, Export: -)
        // POST: MitarbeiterLehrstuhl/Delete/5

        public ActionResult Delete(int id)
        {
            MitarbeiterLehrstuhl mitarbeiterLehrstuhl = db.MitarbeiterLehrstuhls.Find(id);
            db.MitarbeiterLehrstuhls.Remove(mitarbeiterLehrstuhl);
            db.SaveChanges();
            return RedirectToAction("Index", "MitarbeiterLehrstuhl", new { lehrstuhlid = mitarbeiterLehrstuhl.lehrstuhlid });
        }


        // wird zurzeit nicht verwendet, bitte drinlassen
        /*
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
        }     */


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
