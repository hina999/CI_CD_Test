using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FXAdminTransferConnex.Entities
{
    public class ReconciliationMissingOrMismatchModel
    {
        public long ReconciliationId { get; set; }
        public string Source { get; set; }
        public string CustomerName { get; set; }
        public string ReportMonth { get; set; }
        public string ReportYear { get; set; }
        //public string ReportMonthAndYear { get; set; }
        public string AccountName { get; set; }
        public string ParentTrade { get; set; }
        public string OriginalTrade { get; set; }
        public string AccountId { get; set; }
        public string DealRef { get; set; }
        public DateTime? TradeDate { get; set; }
        public string strTradeDate { get; set; }
        public DateTime? ValueDate { get; set; }
        public string strValueDate { get; set; }
        public decimal? ClientBoughtAmount { get; set; }
        public string ClientBoughtCCY { get; set; }
        public decimal? ClientSoldAmount { get; set; }
        public string ClientSoldCCY { get; set; }
        public decimal? ClientSoldAmountGBP { get; set; }
        public decimal? GrossCommsGBP { get; set; }
        public string OurPaymentFees { get; set; }
        public decimal? NetComms { get; set; }
        public decimal? IBCommsPercent { get; set; }
        public decimal? IBCommissiondue { get; set; }
        public string IntroducerBroker { get; set; }
        public string MismatchColumn { get; set; }
        public long? DealId { get; set; }
        public int TotalCount { get; set; }
    }
}
