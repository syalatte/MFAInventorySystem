using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MFAInventorySystem.Models
{
    [MetadataType(typeof(tb_usertypeMetaData))]
    public partial class tb_usertype
    {
        public class tb_usertypeMetaData
        {
            public int ut_id { get; set; }

            [DisplayName("User Type")]
            public string ut_desc { get; set; }
        }
    }
}