using FXAdminTransferConnex.Entities.LocalizationResource;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FXAdminTransferConnex.Entities
{
    public class StagingDealModel
    {
        public int TotalCount { get; set; }

        public long StagingDealId { get; set; }

        public string DealNo { get; set; }

        [DisplayName(@"Contact Name")]
        [StringLength(50, ErrorMessageResourceType = typeof(FXAdminTransferConnexResource), ErrorMessageResourceName = "Validation_StringLengthMessage")]
        public string ContactName { get; set; }

        public string AccountNo { get; set; }

        [DisplayName(@"Company Name")]
        [StringLength(500, ErrorMessageResourceType = typeof(FXAdminTransferConnexResource), ErrorMessageResourceName = "Validation_StringLengthMessage")]
        public string CompanyName { get; set; }

        public long? ClientId { get; set; }

        [DisplayName(@"Trade Date")]
        [Required]
        public DateTime TradeDate { get; set; }

        [DisplayName(@"Trade Date")]
        public string TradeDateString { get; set; }

        [DisplayName(@"Value Date")]
        [Required]
        public DateTime ValueDate { get; set; }

        [DisplayName(@"Value Date")]
        public string ValueDateString { get; set; }

        public int TradeTypeId { get; set; }

        [DisplayName(@"Trade Type")]
        public string TradeType { get; set; }

        public decimal? ClientSoldAmt { get; set; }

        public string ClientSoldCCY { get; set; }

        public decimal? ClientSoldGBP { get; set; }

        public decimal? ClientBoughtAmt { get; set; }

        public string ClientBoughtCCY { get; set; }

        public decimal? GrossCommGBP { get; set; }

        public decimal? WLTotalCommGBP { get; set; }

        public long? DealProcessedId { get; set; }

        public string TStatus { get; set; }

        public DateTime? ImportDate { get; set; }

        public string ImportFileName { get; set; }

        public long? ImportBy { get; set; }

        public bool IsDiscard { get; set; }

        [ScaffoldColumn(false)]
        public int IsDuplicate { get; set; }

        public long UserId { get; set; }

    }
}
