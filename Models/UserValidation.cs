using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MFAInventorySystem.Models
{

    [MetadataType(typeof(tb_userMetaData))]
    public partial class tb_user
    {
        public class tb_userMetaData
        {
            [DisplayName("UserID")]
            [Required(ErrorMessage = "This field is required.")]
            public string u_id { get; set; }

            [DisplayName("Name")]
            public string u_name { get; set; }

            [DisplayName("Contact")]
            public string u_contact { get; set; }

            [DisplayName("Email")]
            public string u_email { get; set; }

            [DisplayName("Password")]
            [DataType(DataType.Password)]
            [Required(ErrorMessage = "This field is required.")]
            public string u_pw { get; set; }

            [DisplayName("User Type")]
            public Nullable<int> u_type { get; set; }

            public string LoginErrorMessage { get; set; }

        }
    }
}