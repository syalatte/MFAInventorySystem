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
    
    public partial class tb_stock
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tb_stock()
        {
            this.tb_stockhistory = new HashSet<tb_stockhistory>();
            this.tb_report = new HashSet<tb_report>();
        }
    
        public int s_id { get; set; }
        public string s_product { get; set; }
        public Nullable<int> s_qty { get; set; }
        public Nullable<double> s_modal { get; set; }
        public Nullable<double> s_hargaJualan { get; set; }
        public Nullable<double> s_untungBersihPerTin { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tb_stockhistory> tb_stockhistory { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tb_report> tb_report { get; set; }
    }
}
