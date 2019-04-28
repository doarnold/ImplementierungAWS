using System;
using System.ComponentModel.DataAnnotations;


namespace SUN2.Models
{
    //Metadaten für Person 
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


    //Metdadaten für Abonnenten Lehrstuhl Zuordnungen
    public class AbonnentenLehrstuhlMetadata
    {
        [Display(Name = "User ID")]
        public string userid;

        [Display(Name = "Lehrstuhl ID")]
        public int lehrstuhlid;
    }


    //Metdadaten für Gruppe
    public class GruppeMetadata
    {
        [Display(Name = "Gruppen ID")]
        public int gruppenid;

        [Display(Name = "Bezeichnung")]
        public string bezeichnung;

        [Display(Name = "Beschreibung")]
        public string beschreibung;

        [Display(Name = "Verantwortlicher")]
        public string verantwortlicher;

        [Display(Name = "Private Gruppe?")]
        public Nullable<bool> privat;
    }


    //Metdadata für GruppenEintraege
    public class GruppenEintraegeMetadata
    {
        [Display(Name = "ID")]
        public int id;

        [Display(Name = "Gruppen ID")]
        public int gruppenid;

        [Display(Name = "Datum")]
        public System.DateTime datum;

        [Display(Name = "Autor")]
        public string autor;

        [Display(Name = "Inhalt")]
        public string inhalt;
    }


    //Metdadata für Lehrstuhl
    public class LehrstuhlMetadata
    {
        [Display(Name = "Lehrstuhl ID")]
        public int lehrstuhlid;

        [Display(Name = "Bezeichnung")]
        public string bezeichnung;

        [Display(Name = "Beschreibung")]
        public string beschreibung;

        [Display(Name = "Verantwortlicher")]
        public string verantwortlicher;

        [Display(Name = "Privat?")]
        public Nullable<bool> privat;
    }


    //Metdadata für LehrstuhlEintraege
    public class LehrstuhlEintraegeMetadata
    {
        [Display(Name = "ID")]
        public int id;

        [Display(Name = "Lehrstuhl ID")]
        public int lehrstuhlid;

        [Display(Name = "Datum")]
        public System.DateTime datum;

        [Display(Name = "Autor")]
        public string autor;

        [Display(Name = "Inhalt")]
        public string inhalt;
    }


    //Metdadata für Mitarbeiter Lehrstuhl Zuordnung
    public class MitarbeiterLehrstuhlMetadata
    {
        [Display(Name = "User ID")]
        public string userid { get; set; }

        [Display(Name = "Lehrstuhl ID")]
        public int lehrstuhlid { get; set; }
    }


    //Metdadata für Mitglieder Gruppe Zuordnung
    public class MitgliederGruppeMetadata
    {
        [Display(Name = "User ID")]
        public string userid { get; set; }

        [Display(Name = "Gruppen ID")]
        public int gruppenid { get; set; }
    }

}