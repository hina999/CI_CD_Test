using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FXAdminTransferConnex.Entities
{
    public class ProspectModel
    {
        public long ProspectId { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        public string CompanyName { get; set; }

        [Required]
        public string MobileNo { get; set; }

        [Required]
        [RegularExpression(RegularExpression.RegexEmail, ErrorMessage = "Invalid Email Address")]
        [StringLength(500, ErrorMessageResourceType = typeof(LocalizationResource.FXAdminTransferConnexResource), ErrorMessageResourceName = "Validation_StringLengthMessage")]
        public string EmailId { get; set; }

        
        public string ProspectAddress { get; set; }

        
        public long?SalesPersonId { get; set; }

        public string SalesPerson { get; set; }

        [Required]
        public long TraderId { get; set; }

        public string Trader { get; set; }

        public string CommunicationDetail { get; set; }

        public long? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public long? UpdatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }
        
        public string LandlineNo { get; set; }

        [DisplayName(@"BoughtCurrencies")]
        [Required]
        public string BoughtCurrencies { get; set; }

        [DisplayName(@"SoldCurrencies")]
        [Required]
        public string SoldCurrencies { get; set; }

        [DisplayName(@"LeadCategories")]
        [Required]
        public string LeadCategories { get; set; }

        [Required]
        public long? LeadCategoryId { get; set; }
        public long? BusinessCategoryId { get; set; }
        public long? BusinessCategorySectorId { get; set; }
        public int TotalCount { get; set; }
        public long? JuniorSalesPersonId { get; set; }

    }
}
