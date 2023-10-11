using System;
using System.Collections.Generic;

namespace FXAdminTransferConnex.Data.Models
{
    public partial class CommunicationType
    {
        public byte CommunicationTypeId { get; set; }
        public string TypeName { get; set; }
        public bool Active { get; set; }
    }
}
