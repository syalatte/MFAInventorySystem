using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MFAInventorySystem.Models
{
    [MetadataType(typeof(tb_stockMetaData))]
    public partial class tb_stock
    {
        public class tb_stockMetaData
        {
            [DisplayName("Stock ID")]
            public int s_id { get; set; }

            [DisplayName("Product")]
            public string s_product { get; set; }

            [DisplayName("Quantity")]
            public Nullable<int> s_qty { get; set; }

            [DisplayName("Cost")]
            [DisplayFormat(DataFormatString = "{0:n2}")]
            public Nullable<double> s_modal { get; set; }

            [DisplayName("Selling Price")]
            [DisplayFormat(DataFormatString = "{0:n2}")]
            public Nullable<double> s_hargaJualan { get; set; }

            [DisplayName("Net Profit/Tin")]
            [DisplayFormat(DataFormatString = "{0:n2}")]
            public Nullable<double> s_untungBersihPerTin { get; set; }
        }
    }
}