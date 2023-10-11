using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FXAdminTransferConnex.Entities
{
    public class LeadCategoryModel
    {
        public long LeadId { get; set; }

        [Required]
        public string LeadCategory { get; set; }
    }
}
