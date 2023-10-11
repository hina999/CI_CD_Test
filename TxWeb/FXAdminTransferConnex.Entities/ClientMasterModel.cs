using FXAdminTransferConnex.Entities.LocalizationResource;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace FXAdminTransferConnex.Entities
{
    public class ClientMasterModel
    {
        public int TotalCount { get; set; }

        public long ClientId { get; set; }

        [DisplayName(@"Full Name")]
        [Required]
        [StringLength(500, ErrorMessageResourceType = typeof(FXAdminTransferConnexResource), ErrorMessageResourceName = "Validation_StringLengthMessage")]
        public string FullName { get; set; }

        [DisplayName(@"Company Name")]
        [Required]
        [StringLength(500, ErrorMessageResourceType = typeof(FXAdminTransferConnexResource), ErrorMessageResourceName = "Validation_StringLengthMessage")]
        public string CompanyName { get; set; }

        [DisplayName(@"Address Line1")]
        //[Required]
        public string AddressLine1 { get; set; }

        [DisplayName(@"Address Line2")]
        public string AddressLine2 { get; set; }

        [DisplayName(@"City/Town")]
        //[Required]
        [StringLength(50, ErrorMessageResourceType = typeof(FXAdminTransferConnexResource), ErrorMessageResourceName = "Validation_StringLengthMessage")]
        public string City_Town { get; set; }

        [DisplayName(@"State/County")]
        //[Required]
        [StringLength(50, ErrorMessageResourceType = typeof(FXAdminTransferConnexResource), ErrorMessageResourceName = "Validation_StringLengthMessage")]
        public string State_County { get; set; }

        [DisplayName(@"Country")]
        //[Required]
        [StringLength(50, ErrorMessageResourceType = typeof(FXAdminTransferConnexResource), ErrorMessageResourceName = "Validation_StringLengthMessage")]
        public string Country { get; set; }

        [DisplayName(@"Mobile No.")]
        //[Required]
        [StringLength(50, ErrorMessageResourceType = typeof(FXAdminTransferConnexResource), ErrorMessageResourceName = "Validation_StringLengthMessage")]
        public string MobileNo { get; set; }

        [DisplayName(@"Alt Mobile No.")]
        [StringLength(50, ErrorMessageResourceType = typeof(FXAdminTransferConnexResource), ErrorMessageResourceName = "Validation_StringLengthMessage")]
        public string AltMobileNo { get; set; }

        [DisplayName(@"Email Address")]
        [Required]
        [RegularExpression(RegularExpression.RegexEmail, ErrorMessage = "Invalid Email Address")]
        [StringLength(500, ErrorMessageResourceType = typeof(FXAdminTransferConnexResource), ErrorMessageResourceName = "Validation_StringLengthMessage")]
        public string EmailAddress { get; set; }

        [DisplayName(@"Account No.")]
        //[Required]
        [StringLength(100, ErrorMessageResourceType = typeof(FXAdminTransferConnexResource), ErrorMessageResourceName = "Validation_StringLengthMessage")]
        public string AccountNo { get; set; }
        [DisplayName(@"ScioPay Account No.")]
        public string ScioPayAccountId { get; set; }


        [DisplayName(@"Regiter Date")]
        //[Required]
        public DateTime? RegiterDate { get; set; }

        [DisplayName(@"Register Date")]
        public string RegiterDateString { get; set; }

        [DisplayName(@"Default Margin")]
        //[Required]
        public string DefaultMargin { get; set; }

        [DisplayName(@"Currencies")]
        //[Required]
        public string Currencies { get; set; }

        [DisplayName(@"Sales Person")]
        //[Required]
        public long? SalesPersonId { get; set; }
        public string SalesPersonName { get; set; }

        [DisplayName(@"Trader")]
        //[Required]
        public long? TraderId { get; set; }

        public string TraderName { get; set; }

        [DisplayName(@"Affiliate")]
        public long? AffiliateId { get; set; }
        public string AffiliateName { get; set; }

        [DisplayName(@"Category")]
        public long? CategoryId { get; set; }
        public string CategoryName { get; set; }


        [DisplayName(@"Past Comm. Detail")]
        public string PastCommDetail { get; set; }


        [DisplayName(@"Awaiting Action")]
        public string AwaitingAction { get; set; }

        public bool IsAwaitingAction { get; set; }

        [DisplayName(@"Market Order")]
        public string MarketOrder { get; set; }

        public bool IsMarketOrder { get; set; }

        public bool IsActive { get; set; }

        public long UserId { get; set; }

        public string CCAccount_id { get; set; }
        [DisplayName(@"Source")]
        [Required]
        public string Source { get; set; }
        //public long? GcPartnerClientId { get; set; }
        public string GcPartnerClientId { get; set; }

        [DisplayName(@"Margin Posted")]
        [DisplayFormat(DataFormatString = "{0:n4}", ApplyFormatInEditMode = true)]
        public decimal? MarginPosted { get; set; }

        [DisplayName(@"Bought Currencies")]
        public string BoughtCurrencies { get; set; }

        [DisplayName(@"Sold Currencies")]
        public string SoldCurrencies { get; set; }

        public long? LeadCategoryId { get; set; }

        public string LeadCategory { get; set; }
        public string ClientSource { get; set; }

       public NewClientReportModel newclientReport { get; set; }

        [DisplayName(@"Sector")]
        public long? SectorId { get; set; }
        public string SectorName { get; set; }

        public string Is_match { get; set; }

        [DisplayName(@"Junior Sales Person")]
        //[Required]
        public long? JuniorSalesPersonId { get; set; }
        public string JuniorSalesPersonName { get; set; }
        public string ContactName { get; set; }

    }

    public class TaskReminderModel
    {
        [ScaffoldColumn(false)]
        public long TaskId { get; set; }
        [ScaffoldColumn(false)]
        public long ClientId { get; set; }

        [ScaffoldColumn(false)]
        public string ClientName { get; set; }

        [ScaffoldColumn(false)]
        public string ClientCompanyName { get; set; }

        [ScaffoldColumn(false)]
        public long ProspectId { get; set; }

        [ScaffoldColumn(false)]
        public string ProspectName { get; set; }

        [ScaffoldColumn(false)]
        public string ProspectCompanyName { get; set; }

        [Required]
        [UIHint("TextBox")]
        //[Autocomplete(false)]
        public string Subject { get; set; }

        [UIHint("TaskTypeDropdown")]
        [Display(Name = "Task Type")]
        public int TaskTypeId { get; set; }

        [ScaffoldColumn(false)]
        [Display(Name = "Task Type")]
        public string TaskType { get; set; }

        [Required]
        [UIHint("UserDropDown")]
        [Display(Name = "Assign To")]
        public long AssignToId { get; set; }

        [ScaffoldColumn(false)]
        public string AssignToName { get; set; }

        //[Required]
        //[UIHint("DateTimePicker")]
        //[Display(Name ="Task Date Time")]
        [ScaffoldColumn(false)]
        public DateTime TaskReminderDatetime { get; set; }

        //[HiddenInput(DisplayValue = false)]
        [Required]
        [UIHint("DateTimePicker")]
        [Display(Name = "Task Date Time")]
        public string TaskReminderDatetimeString { get; set; }

        [UIHint("TextArea")]
        public string Notes { get; set; }
        [ScaffoldColumn(false)]
        public int? IsDismiss { get; set; }
        [ScaffoldColumn(false)]
        public long UserId { get; set; }
        [ScaffoldColumn(false)]
        public long? ClientLeadCategoryId { get; set; }
        [ScaffoldColumn(false)]
        public long? ProspectLeadCategoryId { get; set; }
        [ScaffoldColumn(false)]
        public string ThemeColor { get; set; }
        
    }


    public class ImportLogModel
    {
        public DateTime ImportDate { get; set; }
        public DateTime ImportFrom { get; set; }
        public DateTime ImportTo { get; set; }
        public Boolean Importstatus { get; set; }
        public int DealCount { get; set; }
        public string DealShortReference { get; set; }
    }

    public class MarketOrderSettingModel
    {
        [ScaffoldColumn(false)]
        public long MarketOrderId { get; set; }

        [ScaffoldColumn(false)]
        public long ClientId { get; set; }

        [ScaffoldColumn(false)]
        public long ProspectId { get; set; }

        [UIHint("MOFromCurrencyDropdown")]
        [Display(Name = "From Currency")]
        public int FromCurrency { get; set; }

        [UIHint("MOToCurrencyDropdown")]
        [Display(Name = "To Currency")]
        public int ToCurrency { get; set; }

        [UIHint("MOOperatorDropdown")]
        [Display(Name = "Condition")]
        public int ConditionId { get; set; }

        [UIHint("MOMarketRateNumericTextBox")]
        [DisplayFormat(DataFormatString = "{0:n4}", ApplyFormatInEditMode = true)]
        [Required]
        [DisplayName("Market Rate")]
        public decimal MarketRate { get; set; }

        [UIHint("MOClientRateNumericTextBox")]
        [DisplayFormat(DataFormatString = "{0:n4}", ApplyFormatInEditMode = true)]
        [DisplayName("Client Rate")]
        [Required]
        public decimal ClientRate { get; set; }

        [UIHint("MOAmountNumericTextBox")]
        [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
        [DisplayName("Amount")]
        [Required]
        public decimal Amount { get; set; }

        [UIHint("MONotifySettingDropdown")]
        [Display(Name = "Filter")]
        public int FilterId { get; set; }

        [UIHint("DatePicker")]
        [Display(Name = "Start Date")]
        public DateTime? StartDate { get; set; }

        [UIHint("DatePicker")]
        [Display(Name = "End Date")]
        public DateTime? EndDate { get; set; }

        [UIHint("MOCommentTextBox")]
        public string Comments { get; set; }

        [ScaffoldColumn(false)]
        public string FromCurrencyName { get; set; }
        [ScaffoldColumn(false)]
        public string ToCurrencyName { get; set; }
        [ScaffoldColumn(false)]
        [Display(Name = "Date Condition")]
        public string Filter { get; set; }
        [ScaffoldColumn(false)]
        public string OperatorStr { get; set; }
        [ScaffoldColumn(false)]
        public string FromDate { get; set; }
        [ScaffoldColumn(false)]
        public string ToDate { get; set; }
        [ScaffoldColumn(false)]
        public bool? IsNotify { get; set; }
        [ScaffoldColumn(false)]
        public string Operator { get; set; }
        [ScaffoldColumn(false)]
        public string Status { get; set; }
        [ScaffoldColumn(false)]
        public string strCreatedDate { get; set; }
        [ScaffoldColumn(false)]
        public string FullName { get; set; }
        [ScaffoldColumn(false)]
        public string CompanyName { get; set; }
        [ScaffoldColumn(false)]
        public string TraderName { get; set; }
        [ScaffoldColumn(false)]
        public string AccountNo { get; set; }
        [ScaffoldColumn(false)]
        public string From_To_currency { get { return string.Format("{0} - {1}", FromCurrencyName, ToCurrencyName); } }
        [ScaffoldColumn(false)]
        public string DateCondition { get { return string.Format("{0} {1}", Filter, FromDate); } }
        [ScaffoldColumn(false)]
        public string MarketRate_condition { get { return string.Format("{0}{1}", Operator, MarketRate); } }
        //[ScaffoldColumn(false)]
        //public string MarketRate_condition { get { return string.Format("{0}{1}", Operator, MarketRate); } }
    }

    public class NotificationSettingModel
    {
        public int NotifictionFilterId { get; set; }
        public string Filter { get; set; }

    }

    public class ConditionalOperatorModel
    {
        public int ConditionalOperatorID { get; set; }
        public string Operator { get; set; }
        public string OperatorStr { get; set; }
    }

}
