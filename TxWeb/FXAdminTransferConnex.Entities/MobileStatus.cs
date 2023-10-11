using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FXAdminTransferConnex.Entities
{
    public class MobileStatus
    {
        public bool isDND { get; set; }
        public bool isTPS { get; set; }
        public bool isClient { get; set; }
        public bool isProspect { get; set; }
    }

    public class DNDNumbers
    {
        public long DNDNumberID { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        [RegularExpression(@"^\d+$", ErrorMessage = "Please enter valid mobile number.")]
        public string MobileNo { get; set; }

        public string Comments { get; set; }
    }
}
