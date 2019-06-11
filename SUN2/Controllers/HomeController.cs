using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using SUN2.Controllers.misc;
using SUN2.Models;



namespace SUN2.Controllers
{
    public class HomeController : Controller
    {
        private SUN2Entities db = new SUN2Entities();

        ////////////////////////
        ///  Lehrstuhleinträge
        ////////////////////


        //liefert die index view /index
        public ActionResult Index()
        {
            List<NewsFeedModel> list = new List<NewsFeedModel>();
            List<Person> zuordnung = new List<Person>();
            Boolean[] lverantwortlich = new Boolean[10000]; // Index lehrstuhlid, Eintrag false -> lehrstuhl lehrstuhlid kein Verantwortlicher
            Boolean[] lautor = new Boolean[10000]; // Index lehrstuhlid, Eintrag false -> lehrstuhl lehrstuhlid kein autor
            Boolean abonniert = false;
            var userId = User.Identity.GetUserId();

            // Alle abonnierten Lehrstuhleinträge und relevanten (=mitglied) lehrstuhleinträge auslesen
            foreach (LehrstuhlEintraege le in db.LehrstuhlEintraeges)
            {
                // Signal an Frontend, ob User auch autor ist und bearbeiten/löschen darf
                lautor[le.id] = AuthCheck.AutorLE(le.lehrstuhlid, userId);

                // Signal an Frontend, ob User auch Verantwortlicher ist und somit bearbeiten/löschen darf
                lverantwortlich[le.lehrstuhlid] = AuthCheck.VerantLehr(le.lehrstuhlid, userId);


                // prüfen, ob user mitarbeiter des lehrstuhls ist oder diesen abonniert hat
                foreach (MitarbeiterLehrstuhl ml in db.MitarbeiterLehrstuhls)
                {
                    if(ml.userid == userId || User.IsInRole("Admin"))
                    {
                        abonniert = true;
                    }
                }

                foreach(AbonnentenLehrstuhl al in db.AbonnentenLehrstuhls)
                {
                    if (al.userid == userId || User.IsInRole("Admin"))
                    {
                        abonniert = true;
                    }
                }

                // Userid richtig darstellen
                foreach (Person person in db.Person)
                {
                    if (person.id == le.autor)
                    {
                        le.autor = HelpFunctions.GetDisplayName(person.id);

                        // zuordnungstabelle für verlinkung erstellen     
                        zuordnung.Add(new Person
                        {
                            id = person.id,
                            name = le.autor
                        });
                    }
                }


                ViewBag.lautor = lautor;
                ViewBag.lverantwortlich = lverantwortlich;


                if (abonniert)
                {
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
                        label5 = le.label5,
                        typ = "l"
                    });
                }

                abonniert = false;
            }

            ////////////////////////
            ///  Gruppeneinträge
            ////////////////////

            Boolean[] verantwortlich = new Boolean[10000]; // Index gruppenid, Eintrag false -> Gruppe gruppenid kein Verantwortlicher
            Boolean[] autor = new Boolean[10000]; // Index gruppenid, Eintrag false -> Gruppe gruppenid kein autor
            Boolean mitglied = false;

            foreach (GruppenEintraege le in db.GruppenEintraeges)
            {
                // Signal an Frontend, ob User auch autor ist und bearbeiten/löschen darf
                autor[le.id] = AuthCheck.AutorGE(le.gruppenid, userId);

                // Signal an Frontend, ob User auch Verantwortlicher ist und somit bearbeiten/löschen darf
                verantwortlich[le.gruppenid] = AuthCheck.VerantGr(le.gruppenid, userId);

                // prüfen, ob user mitglied der gruppe ist
                foreach(MitgliederGruppe mg in db.MitgliederGruppes)
                {
                    if(mg.userid == userId || User.IsInRole("Admin"))
                    {
                        mitglied = true;
                    }
                }


                // Userid richtig darstellen
                foreach (Person person in db.Person)
                {
                    if (person.id == le.autor)
                    {
                        le.autor = HelpFunctions.GetDisplayName(person.id);

                        // zuordnungstabelle für verlinkung erstellen     
                        zuordnung.Add(new Person
                        {
                            id = person.id,
                            name = le.autor
                        });
                    }
                }


                ViewBag.autor = autor;
                ViewBag.verantwortlich = verantwortlich;

            if(mitglied)
                {
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
                        label5 = le.label5,
                        typ = "g"
                    });
                   
                }
                mitglied = false;

            }



            // Gruppen und Lehrstühle müssen in eienr Partial View angezeigt werden!
            // var lehrstuehle = db.Lehrstuhls.ToList();
            ViewBag.zuordnung = zuordnung.Distinct();

            return View(list);
        }


        // Liefert eine Liste aller abonnierten Lehrstühle und Gruppenmitglieschaften
        public ActionResult AbonniertMitglied()
        {
            //angemeldeter User ist Verantwortlicher
            var userId = User.Identity.GetUserId();

            // Abos und Mitgliedschaften ermitteln
            List<AboMitgliedModel> amm = new List<AboMitgliedModel>();
            List<Person> zuordnung = new List<Person>();

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

                                    gr.verantwortlicher = HelpFunctions.GetDisplayName(person.id);

                                    // zuordnungstabelle für verlinkung erstellen     
                                    zuordnung.Add(new Person
                                    {
                                        id = person.id,
                                        name = gr.verantwortlicher
                                    } );
                                }


                            }

                            // zur liste hinzufügen
                            amm.Add(new AboMitgliedModel
                            {
                                id = mg.gruppenid,
                                bezeichnung = gr.bezeichnung,
                                beschreibung = gr.beschreibung,
                                verantwortlicher = gr.verantwortlicher,
                                privat = gr.privat,
                                typ = "g"
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
                                    l.verantwortlicher = HelpFunctions.GetDisplayName(person.id);

                                    // zuordnungstabelle für verlinkung erstellen     
                                    zuordnung.Add(new Person
                                    {
                                        id = person.id,
                                        name = l.verantwortlicher
                                    });

                                }
                                }

                                // zur liste hinzufügen
                                amm.Add(new AboMitgliedModel
                                {
                                id = al.lehrstuhlid,
                                bezeichnung = l.bezeichnung,
                                beschreibung = l.beschreibung,
                                verantwortlicher = l.verantwortlicher,
                                privat = l.privat,
                                typ = "l"
                            });
                        }
                    }

                }
            }


            ViewBag.zuordnung = zuordnung.Distinct();

            return View(amm);
        }
        

        //liefert die administrationsübersicht
        public ActionResult Administration()
        {
            return View();
        }


        // liefert die Suchergebnisse für generelle Suche auf alle DB Tabellen und Felder
        // mit einem Suchwort als Eingabe
        public ActionResult Search(String searchstr)
        {
            // liefert die Ergebnisse
            List<SearchModel> sml = new List<SearchModel>();

            // in gruppen suchen
            var gruppe = db.Gruppes.Where(p => p.bezeichnung.Contains(searchstr) || 
                                   p.beschreibung.Contains(searchstr) ||
                                   p.verantwortlicher.Contains(searchstr));

            foreach(Gruppe gr in gruppe)
            {
                sml.Add(new SearchModel
                {
                    id = 0,
                    entityid = gr.gruppenid,
                    field1 = gr.bezeichnung,
                    field2 = HelpFunctions.GetDisplayName(gr.verantwortlicher),
                    field3 = "",
                    field4 = "",
                    typ = "g"
                });
            }

            // in lehrstühlen suchen
            var lehrstuhl = db.Lehrstuhls.Where(p => p.bezeichnung.Contains(searchstr) ||
                                   p.beschreibung.Contains(searchstr) ||
                                   p.verantwortlicher.Contains(searchstr));

            foreach (Lehrstuhl l in lehrstuhl)
            {
                sml.Add(new SearchModel
                {
                    id = 0,
                    entityid = l.lehrstuhlid,
                    field1 = l.bezeichnung,
                    field2 = HelpFunctions.GetDisplayName(l.verantwortlicher),
                    field3 = "",
                    field4 = "",
                    typ = "l"
                });
            }

            // in gruppeneinträgen suchen
            var gruppeneintraege = db.GruppenEintraeges.Where(p => p.autor.Contains(searchstr) ||
                                   p.inhalt.Contains(searchstr) ||
                                   p.label1.Contains(searchstr) ||
                                   p.label2.Contains(searchstr) ||
                                   p.label3.Contains(searchstr) ||
                                   p.label4.Contains(searchstr) ||
                                   p.label5.Contains(searchstr));

            foreach (GruppenEintraege ge in gruppeneintraege)
            {
                sml.Add(new SearchModel
                {
                    id = ge.id,
                    entityid = ge.gruppenid,
                    field1 = ge.inhalt,
                    field2 = HelpFunctions.GetDisplayName(ge.autor),
                    field3 = ge.datum.ToString(),
                    field4 = "",
                    typ = "ge"
                });
            }


            // in lehrstuhleinträgen suchen
            var lehrstuhleintraege = db.LehrstuhlEintraeges.Where(p => p.autor.Contains(searchstr) ||
                                   p.inhalt.Contains(searchstr) ||
                                   p.label1.Contains(searchstr) ||
                                   p.label2.Contains(searchstr) ||
                                   p.label3.Contains(searchstr) ||
                                   p.label4.Contains(searchstr) ||
                                   p.label5.Contains(searchstr));

            foreach (LehrstuhlEintraege le in lehrstuhleintraege)
            {
                sml.Add(new SearchModel
                {
                    id = le.id,
                    entityid = le.lehrstuhlid,
                    field1 = le.inhalt,
                    field2 = HelpFunctions.GetDisplayName(le.autor),
                    field3 = le.datum.ToString(),
                    field4 = "",
                    typ = "le"
                });
            }


            // in personen suchen
            var personen = db.Person.Where(p => p.name.Contains(searchstr) ||
                                   p.vorname.Contains(searchstr));

            foreach (Person p in personen)
            {
                sml.Add(new SearchModel
                {
                    id = 0,
                    entityid = 0,
                    field1 = HelpFunctions.GetDisplayName(p.id),
                    field2 = p.AspNetUsers.Role,
                    field3 = p.AspNetUsers.Email,
                    field4 = p.id,
                    typ = "p"
                });
            }

            return View(sml);
        }


    }
}