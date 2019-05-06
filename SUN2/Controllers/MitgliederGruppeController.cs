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
    public class MitgliederGruppeController : Controller
    {
        private SUN2Entities db = new SUN2Entities();

        // GET: MitgliederGruppe
        public ActionResult Index(int? gruppenid)
        {
            List<MitgliederGruppe> entries = new List<MitgliederGruppe>();

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
                        entries.Add(ge);
                    }
                }
                return View(entries.ToList());
            }

        }

        // Funktionalität zum Hinzfügen neuer Mitglieder zu einer Gruppe
        // GET Methode stellt eine Liste aller verfügbaren Personen zum Hinzufügen bereit, die nicht bereits
        // Mitglied der entsprechenden Gruppe (s. import parameter gruppenid) sind
        // GET: MitgliederGruppe/Add/5
        public ActionResult Add(int? gruppenid)
        {
            List<PersonAddGruppeModel> entries = new List<PersonAddGruppeModel>();

            if (gruppenid == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                foreach (Person person in db.Person)
                {
                   // foreach (MitgliederGruppe mitglied in db.MitgliederGruppes)
                   // {
                       // if (person.id != mitglied.userid && mitglied.gruppenid != gruppenid)
                       // {

                            entries.Add(new PersonAddGruppeModel {
                                gruppenid = (int)gruppenid,
                                id = person.id,
                                name = person.name,
                                vorname = person.vorname,
                                email = person.AspNetUsers.Email
                                        } );
                       // }
                    //}
                }
            }

            return View(entries);
        }

        // Funktionalität zum Hinzfügen neuer Mitglieder zu einer Gruppe
        // Fügt eine aus der Liste ausgwählte Perosn zur entsprechenden Gruppe hinzu
        // GET: MitgliederGruppe/AddPerson
        public ActionResult AddPerson(int? gruppenid, string id)
        {
            PersonAddGruppeModel pagm = new PersonAddGruppeModel();
            pagm.gruppenid = (int)gruppenid;
            pagm.id = id;

            if (gruppenid == null || id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                foreach (Person person in db.Person)
                {
                    if (person.id == id)
                    {
                        pagm.name = person.name;
                        pagm.vorname = person.vorname;
                        pagm.email = person.AspNetUsers.Email;
                    }

                }
            }

            MitgliederGruppe mg = new MitgliederGruppe();

            if (pagm.id != null)
            {
                mg.userid = pagm.id;
                mg.gruppenid = (int)pagm.gruppenid;

                if (ModelState.IsValid)
                {
                    db.MitgliederGruppes.Add(mg);
                    db.SaveChanges();
                    return RedirectToAction("Index", "Gruppe");
                }
            }


            return View(pagm);
        }


        // GET: MitgliederGruppe/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MitgliederGruppe mitgliederGruppe = db.MitgliederGruppes.Find(id);
            if (mitgliederGruppe == null)
            {
                return HttpNotFound();
            }
            return View(mitgliederGruppe);
        }

        // GET: MitgliederGruppe/Create
        public ActionResult Create(int? gruppenid)
        {
            if (gruppenid == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            MitgliederGruppe gm = new MitgliederGruppe();
            gm.gruppenid = (int)gruppenid;

            return View(gm);

            }  

        // POST: MitgliederGruppe/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "userid,gruppenid")] MitgliederGruppe mitgliederGruppe)
        {
            if (ModelState.IsValid)
            {
                db.MitgliederGruppes.Add(mitgliederGruppe);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(mitgliederGruppe);
        }

        // GET: MitgliederGruppe/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MitgliederGruppe mitgliederGruppe = db.MitgliederGruppes.Find(id);
            if (mitgliederGruppe == null)
            {
                return HttpNotFound();
            }
            return View(mitgliederGruppe);
        }

        // POST: MitgliederGruppe/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "userid,gruppenid")] MitgliederGruppe mitgliederGruppe)
        {
            if (ModelState.IsValid)
            {
                db.Entry(mitgliederGruppe).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(mitgliederGruppe);
        }

        // GET: MitgliederGruppe/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MitgliederGruppe mitgliederGruppe = db.MitgliederGruppes.Find(id);
            if (mitgliederGruppe == null)
            {
                return HttpNotFound();
            }
            return View(mitgliederGruppe);
        }

        // POST: MitgliederGruppe/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            MitgliederGruppe mitgliederGruppe = db.MitgliederGruppes.Find(id);
            db.MitgliederGruppes.Remove(mitgliederGruppe);
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
