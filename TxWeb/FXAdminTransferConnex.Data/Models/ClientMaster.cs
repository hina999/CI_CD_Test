using System;
using System.Collections.Generic;

namespace FXAdminTransferConnex.Data.Models
{
    public partial class ClientMaster
    {
        public ClientMaster()
        {
            this.DealBrokerAgents = new List<DealBrokerAgent>();
        }

        public int ClientId { get; set; }
        public string ClientMasterTitle { get; set; }
        public string ForeName { get; set; }
        public string LastName { get; set; }
        public string CompanyName { get; set; }
        public byte Type { get; set; }
        public string Address { get; set; }
        public string MobileNumber { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public string BosACNumber { get; set; }
        public bool IsActive { get; set; }
        public Nullable<System.DateTime> ACOpenedDate { get; set; }
        public string Fax { get; set; }
        public Nullable<decimal> RegistrationCost { get; set; }
        public Nullable<int> Title { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public Nullable<bool> DoNotContactAgain { get; set; }
        public virtual ICollection<DealBrokerAgent> DealBrokerAgents { get; set; }
    }
}
