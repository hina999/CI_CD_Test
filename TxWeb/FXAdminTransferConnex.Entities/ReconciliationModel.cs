using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FXAdminTransferConnex.Entities
{
    public class ReconciliationModel
    {

        public long ReconciliationId { get; set; }
        
        public string CustomerName { get; set; }

        [Required]
        public string Source { get; set; }

        [Required]
        public long ReportMonth { get; set; }

        [Required]
        public long ReportYear { get; set; }
     
    }
}
