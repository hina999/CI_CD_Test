using System;
using System.Collections.Generic;

namespace FXAdminTransferConnex.Data.Models
{
    public partial class ProspectClientCommunication
    {
        public int ProspectClientCommunicationId { get; set; }
        public int ClientId { get; set; }
        public System.DateTime CommunicationDate { get; set; }
        public Nullable<byte> CommunicationType { get; set; }
        public string CommunicationNotes { get; set; }
        public Nullable<System.DateTime> NextFollowUpDate { get; set; }
    }
}
