using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MFAInventorySystem.Models
{
    [MetadataType(typeof(tb_stockhistoryMetaData))]
    public partial class tb_stockhistory
    {
        public class tb_stockhistoryMetaData
        {
            [DisplayName("StockHistory ID")]
            public int sh_id { get; set; }

            [DisplayName("Stock ID")]
            public Nullable<int> sh_sid { get; set; }

            [DisplayName("Quantity Slot")]
            public Nullable<int> sh_qtySlot { get; set; }

            [DisplayName("VendingMachine ID")]
            public Nullable<int> sh_vmID { get; set; }

            [DisplayName("Net Profits")]
            [DisplayFormat(DataFormatString = "{0:n2}")]
            public Nullable<double> sh_untungBersih { get; set; }

            [DisplayName("Date")]
            [Required]
            [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
            public Nullable<System.DateTime> sh_date { get; set; }

            [DisplayName("Name")]
            public string sh_uid { get; set; }

            [DisplayName("Quantity Sold")]
            public Nullable<int> sh_qtySold { get; set; }
        }
    }
}