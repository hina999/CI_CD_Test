using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FXAdminTransferConnex.Entities
{
    public class TraderCommissionModel
    {
        [ScaffoldColumn(false)]
        public long TraderCommissionId { get; set; }

        [ScaffoldColumn(false)]
        public long TraderId { get; set; }

        [DisplayName(@"Deal From Amount")]
        [Required]
        public int DealFromAmt { get; set; }

        [DisplayName(@"Deal To Amount")]
        [Required]
        public int DealToAmt { get; set; }

        [Required]
        public decimal Commission { get; set; }

        [ScaffoldColumn(false)]
        public long UserId { get; set; }
    }
}
