using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SUN2.Models
{

    public class NewsFeedModel
    {
        public int id { get; set; } // brauchen wir voraussichtlich zb zum bearbeiten
        public int entityid { get; set; } //lehrstuhlid oder gruppenid
        public System.DateTime datum { get; set; }
        public string autor { get; set; }
        public string inhalt { get; set; }
        public string label1 { get; set; }
        public string label2 { get; set; }
        public string label3 { get; set; }
        public string label4 { get; set; }
        public string label5 { get; set; }
        public string typ { get; set; }  //g = gruppe, l = lehrstuhl
        public string entityname { get; set; }

    }
}