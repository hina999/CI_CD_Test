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
    public class DealModel
    {

        public int TotalCount { get; set; }

        public long DealId { get; set; }

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

        public string GrossCommCurrency { get; set; }

        public decimal? GrossCommGBP { get; set; }

        public decimal? WLTotalCommGBP { get; set; }

        public decimal? GrossComm { get; set; }

        public string TStatus { get; set; }

        public long UserId { get; set; }

        public decimal? GrossCommGBPFinal { get; set; }

        public string DealSource { get; set; }

        public decimal? AdditionalPLAmount { get; set; }

        public string AdditionalPLNotes { get; set; }

        public decimal? ProfitOrLoss { get; set; }
        public DateTime? EventDate { get; set; }
        public string IsMatch { get; set; }
        public string ExsistingCompanyName { get; set; }
        public decimal? MarketConversionRate { get; set; }
        public decimal? GBPConversationRate { get; set; }
        public string ScioPayAccountId { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool? IsNewlyModified { get; set; }
        public decimal? ProfitGBPEst { get; set; }
        public string ProfitCCY { get; set; }
        public string Spread { get; set; }
        public decimal? VolumeGBPEst { get; set; }
        public string WLCommsCCY { get; set; }
        public string WLRevShare { get; set; }
        public decimal? WLCommsInCCY { get; set; }
        public string Brand { get; set; }
        public string AccountCountry { get; set; }
        public string Owner { get; set; }
        public string Contact { get; set; }
        public string Creator { get; set; }
        public string AccountId { get; set; }

    }
}
