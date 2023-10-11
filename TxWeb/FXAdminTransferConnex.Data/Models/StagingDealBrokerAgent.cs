using System;
using System.Collections.Generic;

namespace FXAdminTransferConnex.Data.Models
{
    public partial class StagingDealBrokerAgent
    {
        public int StagingDealBrokerAgentId { get; set; }
        public Nullable<int> StagingDealId { get; set; }
        public Nullable<int> BrokerAgentId { get; set; }
        public Nullable<byte> UserTypeId { get; set; }
        public Nullable<decimal> Commission { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public virtual StagingDeal StagingDeal { get; set; }
        public virtual UserMaster UserMaster { get; set; }
    }
}
