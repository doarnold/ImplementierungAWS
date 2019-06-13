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
    public class MitgliederGruppeController : Controller
    {
        private SUN2Entities db = new SUN2Entities();

        // Gibt eine Liste aller Gruppenmitglieder für eine bestimmte Gruppe zurück
        // (Import: GruppenID, Export: Liste Gruppenmitglieder)
        // GET: MitgliederGruppe
        public ActionResult Index(int? gruppenid)
        {
            List<MitgliederGruppe> entries = new List<MitgliederGruppe>();
            List<Person> zuordnung = new List<Person>();

            if (gruppenid == null)
            {
                return View(db.MitgliederGruppes.ToList());
            }
            else
            {
                foreach (MitgliederGruppe ge in db.MitgliederGruppes)
                {
                    if (ge.gruppenid == gruppenid)
                    {
                        foreach (Person person in db.Person)
                        {
                            if(ge.userid == person.id)
                            {
                                ge.userid = HelpFunctions.GetDisplayName(person.id);
                        }

                            // zuordnungstabelle für verlinkung erstellen
                            Person el = new Person();
                            el.id = person.id;
                            el.name = ge.userid;

                            zuordnung.Add(el);
                     }
   
                        entries.Add(ge);
                    }   
                }

                ViewBag.zuordnung = zuordnung;

                return View(entries.ToList());
            }
        }


        // Funktionalität zum Hinzfügen neuer Mitglieder zu einer Gruppe;
        // Methode stellt eine Liste aller verfügbaren Personen zum Hinzufügen bereit, die nicht bereits
        // Mitglied der entsprechenden Gruppe (s. import parameter gruppenid) sind
        // ~~Import: Gruppenid
        // ~~Export: Liste des PersonAddGruppeModel (userid, name, vorname, email, gruppenid)
        // GET: MitgliederGruppe/Add/5
        public ActionResult Add(int? gruppenid)
        {
            List<PersonAddGruppeModel> entries = new List<PersonAddGruppeModel>();
            List<PersonAddGruppeModel> enthalten = new List<PersonAddGruppeModel>();

            if (gruppenid == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                foreach (Person person in db.Person)
                {
                    //Liste mit allen Personen aufbauen
                    entries.Add(new PersonAddGruppeModel
                    {
                        gruppenid = (int)gruppenid,
                        id = person.id,
                        name = person.name,
                        vorname = person.vorname,
                        email = person.AspNetUsers.Email
                    });        

                //bereits enthaltene Personen ermitteln
                foreach (MitgliederGruppe mitglied in db.MitgliederGruppes)
                    {
                       if (person.id == mitglied.userid && mitglied.gruppenid == gruppenid)
                        {
                            enthalten.Add(new PersonAddGruppeModel {
                                gruppenid = (int)gruppenid,
                                id = person.id,
                                name = person.name,
                                vorname = person.vorname,
                                email = person.AspNetUsers.Email
                                        } );
                       }
                    }
                }

                //Alle Personen, die bereits in der Gruppe enhalten sind, entfernen
                foreach (PersonAddGruppeModel ent in enthalten)
                {
                    entries.RemoveAll(x => x.id == ent.id);
                }
            }

            return View(entries);
        }


        // Funktionalität zum Hinzfügen neuer Mitglieder zu einer Gruppe
        // Fügt eine aus der Liste ausgwählte Perosn zur entsprechenden Gruppe hinzu
        // ~~Import: Gruppenid, userid
        // ~~Export: nichts oder BadRequest View
        // GET: MitgliederGruppe/AddPerson
        public ActionResult AddPerson(int? gruppenid, string id)
        {
            MitgliederGruppe mg = new MitgliederGruppe();
            mg.userid = id;
            mg.gruppenid = (int)gruppenid;

            //Falls die Gruppenid oder die UserID nicht gesetzt ist, Fehler melden
            if (gruppenid == null || id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else

            //Wenn alles okay ist, speichere einen neuen Eintrag in der MitgliederGruppe Tabelle und 
            //leite zur Index Seite von Gruppen um
            if (ModelState.IsValid)
            {
                db.MitgliederGruppes.Add(mg);
                db.SaveChanges();
                return RedirectToAction("Index", "Gruppe");
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
      
        }


        // Funktionalität zum Hinzfügen neuer Mitglieder zu einer Gruppe
        // Fügt den angemeldeten User einer (öffentlichen) Gruppe hinzu
        // ~~Import: userid
        // ~~Export: nichts oder BadRequest View
        // GET: MitgliederGruppe/Beitreten
        public ActionResult Beitreten(int? gruppenid)
        {
            MitgliederGruppe mg = new MitgliederGruppe();
            mg.userid = User.Identity.GetUserId();
            mg.gruppenid = (int)gruppenid;

            //Falls die Gruppenid nicht gesetzt ist, Fehler melden
            if (gruppenid == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else

            //Wenn alles okay ist, speichere einen neuen Eintrag in der MitgliederGruppe Tabelle und 
            //leite zur Index Seite von Gruppen um
            if (ModelState.IsValid)
            {
                db.MitgliederGruppes.Add(mg);
                db.SaveChanges();
                return RedirectToAction("Index", "Gruppe");
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

        }



        // Ermöglicht das Austreten des angemeldeten Users aus einer Gruppe
        // POST: MitgliederGruppe/Delete/5
        public ActionResult Austreten(int? gruppenid)
        {

            var userid = User.Identity.GetUserId();
            var ok = false;
            var id = 0;

            foreach(MitgliederGruppe mit in db.MitgliederGruppes)
            {
                if(mit.userid == userid && mit.gruppenid == gruppenid)
                {
                    ok = true;
                    id = mit.id;

                }
            }

            if(ok)
            {
                MitgliederGruppe mitgliederGruppe = db.MitgliederGruppes.Find(id);
                db.MitgliederGruppes.Remove(mitgliederGruppe);
                db.SaveChanges();
                return RedirectToAction("Index", "Gruppe");
            } else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

        }



        // Ermöglicht das Löschen eines Gruppenmitglieds
        // POST: MitgliederGruppe/Delete/5
        public ActionResult Delete(int id)
        {
            MitgliederGruppe mitgliederGruppe = db.MitgliederGruppes.Find(id);
            db.MitgliederGruppes.Remove(mitgliederGruppe);
            db.SaveChanges();
            return RedirectToAction("Index", "MitgliederGruppe", new { gruppenid = mitgliederGruppe.gruppenid });
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
