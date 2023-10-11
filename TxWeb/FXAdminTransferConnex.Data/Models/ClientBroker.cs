using System;
using System.Collections.Generic;

namespace FXAdminTransferConnex.Data.Models
{
    public partial class ClientBroker
    {
        public int ClientBrokerId { get; set; }
        public int ClientId { get; set; }
        public int BrokerId { get; set; }
        public decimal Commission { get; set; }
    }
}
