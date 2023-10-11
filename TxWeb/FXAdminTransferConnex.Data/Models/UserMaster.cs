using System;
using System.Collections.Generic;

namespace FXAdminTransferConnex.Data.Models
{
    public partial class UserMaster
    {
        public UserMaster()
        {
            this.DealBrokerAgents = new List<DealBrokerAgent>();
            this.StagingDealBrokerAgents = new List<StagingDealBrokerAgent>();
        }

        public int UserId { get; set; }
        public string UserTitle { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public byte UserTypeId { get; set; }
        public System.DateTime RegisteredOn { get; set; }
        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public Nullable<int> ClientId { get; set; }

        //public bool IsTakeLoss { get; set; }
        //public string AgnetTitle { get; set; }
        //public int AssignedBrokerId { get; set; }
        //public int ParentAgentId { get; set; }
        //public decimal CommissionAllocated { get; set; }
        //public int AboveTurnOverLimit { get; set; }
        //public decimal CommissionSplit { get; set; }

        public virtual ICollection<DealBrokerAgent> DealBrokerAgents { get; set; }
        public virtual ICollection<StagingDealBrokerAgent> StagingDealBrokerAgents { get; set; }
    }
}
