using System;
using System.Collections.Generic;

namespace FXAdminTransferConnex.Data.Models
{
    public partial class StagingDeal
    {
        public StagingDeal()
        {
            this.StagingDealBrokerAgents = new List<StagingDealBrokerAgent>();
        }

        public int StagingDealId { get; set; }
        public Nullable<int> ClientId { get; set; }
        public string TransactionReceipt { get; set; }
        public string Status { get; set; }
        public string AccountNumber { get; set; }
        public string AccountName { get; set; }
        public Nullable<System.DateTime> OrderDate { get; set; }
        public string FirstTrade { get; set; }
        public string AccountOwnedBy { get; set; }
        public string TradeType { get; set; }
        public string BuyCcy { get; set; }
        public Nullable<double> BuyAmount { get; set; }
        public string SellCcy { get; set; }
        public Nullable<double> SellAmount { get; set; }
        public Nullable<double> ClientRate { get; set; }
        public Nullable<int> Payment { get; set; }
        public string LC { get; set; }
        public Nullable<double> LPBuyAmount { get; set; }
        public Nullable<double> AvgRate { get; set; }
        public Nullable<double> LPSellAmount { get; set; }
        public Nullable<double> RevenueBuyCcy { get; set; }
        public Nullable<double> RevenueSellCcy { get; set; }
        public Nullable<double> GBPFee { get; set; }
        public Nullable<double> GBPGrossRevenue { get; set; }
        public Nullable<double> GBPGrossProfit { get; set; }
        public Nullable<double> GBPFinananceP_L { get; set; }
        public Nullable<double> GBPGPLF { get; set; }
        public Nullable<double> TransferCost { get; set; }
        public Nullable<double> GBPNetProfit { get; set; }
        public Nullable<double> GBPVolume { get; set; }
        public Nullable<double> Spread { get; set; }
        public bool IsDealProceeded { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public string DealType { get; set; }
        public string Type { get; set; }
        public Nullable<System.DateTime> ValueDate { get; set; }
        public Nullable<System.DateTime> ImportDate { get; set; }
        public string ImportFileName { get; set; }
        public Nullable<int> ImportBy { get; set; }
        public virtual ICollection<StagingDealBrokerAgent> StagingDealBrokerAgents { get; set; }
    }
}
