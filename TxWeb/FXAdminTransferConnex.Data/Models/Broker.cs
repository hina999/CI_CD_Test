using System;
using System.Collections.Generic;

namespace FXAdminTransferConnex.Data.Models
{
    public partial class Broker
    {
        public int BrokerId { get; set; }
        public int UserId { get; set; }
        public bool IsActive { get; set; }
        public decimal DefaultCommission { get; set; }
        public System.DateTime RegisteredDate { get; set; }
    }
}
