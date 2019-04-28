using System;
using System.ComponentModel.DataAnnotations;


namespace SUN2.Models
{
    public class PersonMetadata
    {
        [Display(Name = "ID")]
        public string id;

        [StringLength(50)]
        [Display(Name = "Name")]
        public string name;

        [Display(Name = "Vorname")]
        public string vorname;

        [Display(Name = "Persönliche Informationen")]
        public string persinfos;

        [Display(Name = "Matrikelnummer")]
        public Nullable<int> matnr;

        [Display(Name = "Hochschulsemester")]
        public Nullable<int> hsemester;

        [Display(Name = "Fachsemester")]
        public Nullable<int> fsemester;

        [Display(Name = "Studienbeginn")]
        public Nullable<System.DateTime> studienbeginn;

        [Display(Name = "Studiengang")]
        public string studiengang;

        [Display(Name = "Studienfach")]
        public string studienfach;

        [Display(Name = "Benutzer")]
        public AspNetUsers AspNetUsers;
    }
}