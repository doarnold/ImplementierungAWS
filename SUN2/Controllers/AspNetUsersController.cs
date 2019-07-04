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

/**
 * Diese Klasse wurde systemgeneriert und wird als Bibliothek verwendet.
 * Es wurden minimale Änderungen (Viewbag füllen) vorgenommen.
 * Auf Kommentierungen wird deshalb weitgehend verzichtet.
 * 
 * */

namespace SUN2.Controllers
{
    public class AspNetUsersController : Controller
    {
        private SUN2Entities db = new SUN2Entities();

        // GET: AspNetUsers
        public ActionResult Index()
        {
            return View(db.AspNetUsers.ToList());
        }

        // GET: AspNetUsers/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetUsers aspNetUsers = db.AspNetUsers.Find(id);
            if (aspNetUsers == null)
            {
                return HttpNotFound();
            }
            return View(aspNetUsers);
        }

        // GET: AspNetUsers/Create
        public ActionResult Create()
        {
            ViewBag.Id = new SelectList(db.Person, "id", "name");
            return View();
        }

        // POST: AspNetUsers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName")] AspNetUsers aspNetUsers)
        {
            if (ModelState.IsValid)
            {
                db.AspNetUsers.Add(aspNetUsers);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Id = new SelectList(db.Person, "id", "name", aspNetUsers.Id);
            return View(aspNetUsers);
        }

        // GET: AspNetUsers/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetUsers aspNetUsers = db.AspNetUsers.Find(id);
            if (aspNetUsers == null)
            {
                return HttpNotFound();
            }

            //ViewBag mit Rolle zum Editieren füllen
            ViewBag.aspnetroles = db.AspNetRoles;

            ViewBag.Id = new SelectList(db.Person, "id", "name", aspNetUsers.Id);
            return View(aspNetUsers);
        }

        // POST: AspNetUsers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName,Role")] AspNetUsers aspNetUsers)
        {
            if (ModelState.IsValid)
            {
                // UserManager holen und Rollen entfernen und neue Rolle hinzufuegen
                var UserManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                UserManager.RemoveFromRoles(db.Entry(aspNetUsers).Entity.Id, "Admin");
                UserManager.RemoveFromRoles(db.Entry(aspNetUsers).Entity.Id, "Dozent");
                UserManager.RemoveFromRoles(db.Entry(aspNetUsers).Entity.Id, "Student");
                UserManager.AddToRole(db.Entry(aspNetUsers).Entity.Id, db.Entry(aspNetUsers).Entity.Role);

                db.Entry(aspNetUsers).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Id = new SelectList(db.Person, "id", "name", aspNetUsers.Id);

            //ViewBag mit Rolle zum Editieren füllen
            ViewBag.aspnetroles = db.AspNetRoles;
            return View(aspNetUsers);
        }

        // GET: AspNetUsers/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetUsers aspNetUsers = db.AspNetUsers.Find(id);
            if (aspNetUsers == null)
            {
                return HttpNotFound();
            }
            return View(aspNetUsers);
        }

        // POST: AspNetUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            AspNetUsers aspNetUsers = db.AspNetUsers.Find(id);
            db.AspNetUsers.Remove(aspNetUsers);
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
