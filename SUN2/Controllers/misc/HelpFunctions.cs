using SUN2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SUN2.Controllers.misc
{
    public class HelpFunctions
    {
        public static SUN2Entities db = new SUN2Entities();


        // Eingabe: User ID
        // Ausgabe: Anzeige Name Kombination aus Vorname, Nachname, E-Mail Adresse
        public static String GetDisplayName(String userid)
        {
            foreach(Person p in db.Person)
            {
                if(p.id == userid)
                {
                    //Kombination aus Vorname+Nachname+E-Mail anzeigen statt techn. User ID 
                    if (p.name != null && p.vorname != null)
                    {
                        return p.vorname + " " + p.name + " (" + p.AspNetUsers.Email + ")";
                    }
                    else
                    {
                        return p.AspNetUsers.Email;
                    }
                } 

            }
            return "";
        }
      
    }
}