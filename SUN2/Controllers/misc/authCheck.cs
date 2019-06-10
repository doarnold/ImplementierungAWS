using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using SUN2.Models;

namespace SUN2.Controllers.misc
{

    // Klasse mit statischen Methoden zur Realisierung bzw. Kapselung von Funktionalitäten im
    // Zusammenhang mit Berechtigungen und Autoritätschecks
    public class authCheck
    {

        public static SUN2Entities db = new SUN2Entities();

        // Eingabe: GruppenEintragsID und UserID
        // Ausgabe: liefert true, wenn der User den Eintrag verfasst hat
        public static Boolean AutorGE(int id, String userid)
        {
           
            foreach (GruppenEintraege ge in db.GruppenEintraeges)
            {
                if(ge.id == id && ge.autor == userid)
                {
                    return true;
                }
            }

            return false;
        }

        // Eingabe: LehrstuhleintragsID und UserID
        // Ausgabe: Liefert true, wenn der User den Eintrag verfasst hat
        public static Boolean AutorLE(int id, String userid)
        {
            foreach(LehrstuhlEintraege le in db.LehrstuhlEintraeges)
            {
                if(le.id == id && le.autor == userid)
                {
                    return true;
                }
            }

            return false;
        }

        // Eingbabe: GruppenID und UserID
        // Ausgabe: Liefert true, wenn der User Verantwortlicher der Gruppe ist
        public static Boolean VerantGr(int gruppenid, String userid)
        {
            foreach(Gruppe gr in db.Gruppes)
            {
                if(gr.gruppenid == gruppenid && gr.verantwortlicher == userid)
                {
                    return true;
                }
            }
            return false;
        }

        // Eingabe: Lehrstuhlid und userId
        // Ausgabe: Liefert true, wenn der User der Verantwortliche des Lehrstuhls ist
        public static Boolean VerantLehr(int lehrstuhlid, String userid)
        {
            foreach(Lehrstuhl l in db.Lehrstuhls)
            {
                if (l.lehrstuhlid == lehrstuhlid && l.verantwortlicher == userid)
                    return true;
            }
            return false;
        }

        // Eingbabe: GruppenId und userID
        // Ausgabe: liefert true, wenn der User Mitglied der Gruppe ist
        public static Boolean MitgliedGr(int gruppenid, String userid)
        {
            foreach(MitgliederGruppe mg in db.MitgliederGruppes)
            {
                if(mg.gruppenid == gruppenid && mg.userid == userid)
                {
                    return true;
                }
            }
            return false;
        }

        // Eingabe: lehrstuhlid und userid
        // Ausgabe: liefert true, wenn der User Mitarbeiter des Lehrstuhls ist
        public static Boolean MitarbeiterLehr(int lehrstuhlid, String userid)
        {
            foreach(MitarbeiterLehrstuhl ml in db.MitarbeiterLehrstuhls)
            {
                if(ml.lehrstuhlid == lehrstuhlid && ml.userid == userid)
                {
                    return true;
                }
            }
            return false;
        }

    }
}