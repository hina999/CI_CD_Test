using System;
using System.Collections.Generic;

namespace FXAdminTransferConnex.Data.Models
{
    public partial class ClientAgent
    {
        public int ClientAgentId { get; set; }
        public int ClientId { get; set; }
        public int AgentId { get; set; }
        public decimal Commission { get; set; }
    }
}
