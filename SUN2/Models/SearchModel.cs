using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SUN2.Models
{

    public class SearchModel
    {
        public int id { get; set; } // zb gruppeneintragsID
        public int entityid { get; set; } //lehrstuhlid oder gruppenid

        public string field1 { get; set; }
        public string field2 { get; set; }
        public string field3 { get; set; }
        public string field4 { get; set; }
        public string typ { get; set; }  //g = gruppe, ge = gruppeneinträge, l = lehrstuhl, le = lehrstuhleinträge, p = person

    }
}