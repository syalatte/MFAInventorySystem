//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MFAInventorySystem.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    public partial class tb_report
    {
        [DisplayName("Report ID")]
        public int r_id { get; set; }

        [DisplayName("Name")]
        public string r_name { get; set; }

        [DisplayName("Date Generated")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public Nullable<System.DateTime> r_date { get; set; }

        [DisplayName("Description")]
        public string r_desc { get; set; }

        [DisplayName("Profites Gained (RM)")]
        [DisplayFormat(DataFormatString = "{0:n2}")]
        public Nullable<double> r_profits { get; set; }

        [DisplayName("Capitals (RM)")]
        [DisplayFormat(DataFormatString = "{0:n2}")]
        public Nullable<double> r_capitals { get; set; }
    }
}
