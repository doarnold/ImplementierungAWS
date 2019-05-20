using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SUN2.Models
{

    public class AboMitgliedModel
    {
        public int id { get; set; } //gruppenid oder lehrstuhlid
        public string bezeichnung { get; set; }
        public string beschreibung { get; set; }
        public string verantwortlicher { get; set; }
        public Nullable<bool> privat { get; set; }
        public String typ { get; set; } //g = gruppe, l = lehrstuhl
    }

}
