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
    public class HomeController : Controller
    {
        private SUN2Entities db = new SUN2Entities();


        //liefert die index view /index
        public ActionResult Index()
        {
            List<NewsFeedModel> list = new List<NewsFeedModel>();

            // Alle abonnierten Lehrstuhleinträge und relevanten (=mitglied) Gruppeneinträge auslesen
            foreach (LehrstuhlEintraege le in db.LehrstuhlEintraeges)
            {
                // Userid richtig darstellen
                foreach (Person person in db.Person)
                {
                    if (person.id == le.autor)
                    {
                        //Kombination aus Vorname+Nachname+E-Mail anzeigen statt techn. User ID als Verantwortlicher
                        if (person.name != null && person.vorname != null)
                        {
                            le.autor = person.vorname + " " + person.name + " (" + person.AspNetUsers.Email + ")";
                        }
                        else
                        {
                            le.autor = person.AspNetUsers.Email;
                        }
                    }
                }

                list.Add(new NewsFeedModel
                {    
                    id = le.id,
                    entityid = le.lehrstuhlid,
                    datum = le.datum,
                    autor = le.autor,
                    inhalt = le.inhalt,
                    label1 = le.label1,
                    label2 = le.label2,
                    label3 = le.label3,
                    label4 = le.label4,
                    label5 = le.label5
                });
            }

            foreach (GruppenEintraege le in db.GruppenEintraeges)
            {
                // Userid richtig darstellen
                foreach (Person person in db.Person)
                {
                    if (person.id == le.autor)
                    {
                        //Kombination aus Vorname+Nachname+E-Mail anzeigen statt techn. User ID als Verantwortlicher
                        if (person.name != null && person.vorname != null)
                        {
                            le.autor = person.vorname + " " + person.name + " (" + person.AspNetUsers.Email + ")";
                        }
                        else
                        {
                            le.autor = person.AspNetUsers.Email;
                        }
                    }
                }

                list.Add(new NewsFeedModel
                {
                    id = le.id,
                    entityid = le.gruppenid,
                    datum = le.datum,
                    autor = le.autor,
                    inhalt = le.inhalt,
                    label1 = le.label1,
                    label2 = le.label2,
                    label3 = le.label3,
                    label4 = le.label4,
                    label5 = le.label5
                });
            }



            // Gruppen und Lehrstühle müssen in eienr Partial View angezeigt werden!
            // var lehrstuehle = db.Lehrstuhls.ToList();

            return View(list);
        }


        // Liefert eine Liste aller abonnierten Lehrstühle und Gruppenmitglieschaften
        public ActionResult AbonniertMitglied()
        {
            //angemeldeter User ist Verantwortlicher
            var userId = User.Identity.GetUserId();

            // Abos und Mitgliedschaften ermitteln
            List<AboMitgliedModel> amm = new List<AboMitgliedModel>();

            foreach (MitgliederGruppe mg in db.MitgliederGruppes) // relevante gruppen suchen
            {
                if(mg.userid == userId)
                {
                    foreach(Gruppe gr in db.Gruppes) // gruppendaten laden
                    {
                        if (gr.gruppenid == mg.gruppenid)
                        {
                            foreach (Person person in db.Person)
                            {
                                if (gr.verantwortlicher == person.id)
                                {
                                    //Kombination aus Vorname+Nachname+E-Mail anzeigen statt techn. User ID
                                    if (person.name != null && person.vorname != null)
                                    {
                                        gr.verantwortlicher = person.vorname + " " + person.name + " (" + person.AspNetUsers.Email + ")";
                                    }
                                    else
                                    {
                                        gr.verantwortlicher = person.AspNetUsers.Email;
                                    }

                                }
                            }

                            // zur liste hinzufügen
                            amm.Add(new AboMitgliedModel
                            {
                                id = mg.gruppenid,
                                bezeichnung = gr.bezeichnung,
                                beschreibung = gr.beschreibung,
                                verantwortlicher = gr.verantwortlicher,
                                privat = gr.privat
                            });
                        }
                    }

                }
            }

            foreach (AbonnentenLehrstuhl al in db.AbonnentenLehrstuhls) // relevante lehrstühle suchen
            {
                if (al.userid == userId)
                {
                    foreach (Lehrstuhl l in db.Lehrstuhls) // lehrstuhldaten laden
                    {
                        if (l.lehrstuhlid == al.lehrstuhlid)
                        {
                                foreach (Person person in db.Person)
                                {
                                    if (l.verantwortlicher == person.id)
                                    {
                                        //Kombination aus Vorname+Nachname+E-Mail anzeigen statt techn. User ID
                                        if (person.name != null && person.vorname != null)
                                        {
                                            l.verantwortlicher = person.vorname + " " + person.name + " (" + person.AspNetUsers.Email + ")";
                                        }
                                        else
                                        {
                                            l.verantwortlicher = person.AspNetUsers.Email;
                                        }

                                    }
                                }

                                // zur liste hinzufügen
                                amm.Add(new AboMitgliedModel
                                {
                                id = al.lehrstuhlid,
                                bezeichnung = l.bezeichnung,
                                beschreibung = l.beschreibung,
                                verantwortlicher = l.verantwortlicher,
                                privat = l.privat
                            });
                        }
                    }

                }
            }


            return View(amm);
        }
         


        /*
        //liefert die about view /about
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        //liefert die contact view /contact
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        } */

        //liefert die administrationsübersicht
        public ActionResult Administration()
        {
            return View();
        }

        // liefert die Suchergebnisse
        /*
        public ActionResult Search(String searchstr)
        {


        }
        */


    }
}