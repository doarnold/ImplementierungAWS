//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SUN2.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    [MetadataType(typeof(PersonMetadata))]
    public partial class Person
    {
        public string id { get; set; }
        [StringLength(50, MinimumLength = 2)]
        public string name { get; set; }
        [StringLength(50, MinimumLength = 2)]
        public string vorname { get; set; }
        [StringLength(500, MinimumLength = 0)]
        public string persinfos { get; set; }
        public Nullable<int> matnr { get; set; }
        public Nullable<int> hsemester { get; set; }
        public Nullable<int> fsemester { get; set; }
        public Nullable<System.DateTime> studienbeginn { get; set; }
        public string studiengang { get; set; }
        public string studienfach { get; set; }
    
        public virtual AspNetUsers AspNetUsers { get; set; }
    }
}
