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
    
    public partial class tb_stockhistory
    {
        public int sh_id { get; set; }
        public Nullable<int> sh_sid { get; set; }
        public Nullable<int> sh_qtySlot { get; set; }
        public Nullable<int> sh_vmID { get; set; }
        public Nullable<double> sh_untungBersih { get; set; }
        public Nullable<System.DateTime> sh_date { get; set; }
        public string sh_uid { get; set; }
        public Nullable<int> sh_qtySold { get; set; }
    
        public virtual tb_stock tb_stock { get; set; }
        public virtual tb_user tb_user { get; set; }
        public virtual tb_vendingmachine tb_vendingmachine { get; set; }
    }
}
