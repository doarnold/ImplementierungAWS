using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SUN2.Models
{

    public class PersonAddGruppeModel
    {
        public string id { get; set; }
        public int gruppenid { get; set; }
        public string name { get; set; }
        public string vorname { get; set; }
        public string email { get; set; }

    }
}