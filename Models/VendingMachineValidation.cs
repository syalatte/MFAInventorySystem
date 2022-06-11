using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MFAInventorySystem.Models
{
    [MetadataType(typeof(tb_vendingmachineMetaData))]
    public partial class tb_vendingmachine
    {
        public class tb_vendingmachineMetaData
        {
            [DisplayName("Vending ID")]
            public int v_id { get; set; }

            [DisplayName("Location")]
            public string v_location { get; set; }

            [DisplayName("Cash In Slot")]
            public Nullable<double> v_cashInSlot { get; set; }

            [DisplayName("Profits")]
            public Nullable<double> v_profit { get; set; }
        }
    }
}