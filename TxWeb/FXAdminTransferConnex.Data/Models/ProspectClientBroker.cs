using System;
using System.Collections.Generic;

namespace FXAdminTransferConnex.Data.Models
{
    public partial class ProspectClientBroker
    {
        public int ProspectClientBrokerId { get; set; }
        public int ClientId { get; set; }
        public int BrokerId { get; set; }
        public decimal Commission { get; set; }
    }
}
