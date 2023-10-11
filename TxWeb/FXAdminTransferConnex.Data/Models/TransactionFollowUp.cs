using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FXAdminTransferConnex.Data.Models
{
    public partial class TransactionFollowUp
    {
        public int TransactionId { get; set; }

        public int CustomerId { get; set; }

        public string BuyCcy { get; set; }

        public Nullable<double> BuyAmount { get; set; }

        public string SellCcy { get; set; }

        public Nullable<double> SellAmount { get; set; }

        public DateTime TransactionDate { get; set; }
        public DateTime CreatedDate { get; set; }

        public DateTime UpdatedDate { get; set; }

        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }

        public Nullable<bool> IsCompleted { get; set; }

        public string Note { get; set; }
    }
}
