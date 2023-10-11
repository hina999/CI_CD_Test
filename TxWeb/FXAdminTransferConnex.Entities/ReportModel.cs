using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kendo.Mvc.UI;

namespace FXAdminTransferConnex.Entities
{
    public class ClientReproductionReport
    {
        public string ClientName { get; set; }

        public string CompanyName { get; set; }

        public int Deals { get; set; }

        public int col1 { get; set; }
        public int col2 { get; set; }
        public int col3 { get; set; }
        public int col4 { get; set; }
        public int col5 { get; set; }
        public int col6 { get; set; }
        public decimal Avg { get; set; }
        public decimal ReductionChange { get; set; }
        public long ClientId { get; set; }
        //public DateTime TradeDate { get; set; }

    }

    public class ClientRevenueReport
    {
        public string ClientName { get; set; }

        public string CompanyName { get; set; }

        public decimal Deals { get; set; }

        public decimal col1 { get; set; }
        public decimal col2 { get; set; }
        public decimal col3 { get; set; }
        public decimal col4 { get; set; }
        public decimal col5 { get; set; }
        public decimal col6 { get; set; }
        public decimal col7 { get; set; }
        public decimal Avg { get; set; }
        public decimal Total { get; set; }
        public decimal ReductionChange { get; set; }
        public long ClientId { get; set; }
    }
    
    public class MonthModel
    {
        public int MonthNo { get; set; }
        public string MonthName { get; set; }
    }

    public class YearModel
    {
        public int Year { get; set; }
    }

    public class UserDDModel
    {
        public long UserId { get; set; }
        public string FullName { get; set; }
        public string UserType { get; set; }
    }


    public class CurrencyCloudReportModel
    {
        public long ClientId { get; set; }
        public string FullName { get; set; }
        public string AccountNo { get; set; }
        public string CompanyName { get; set; }
        public string EmailAddress { get; set; }
        public string MobileNo { get; set; }
        public string AltMobileNo { get; set; }
        public string RegiterDateString { get; set; }
    }

    public partial class TaskSchedulerModel : ISchedulerEvent
    {
        public long TaskID { get; set; }
        [UIHint("TextBox")]
        [Required]
        public string Title { get; set; }
        [UIHint("TaskTypeDropdown")]
        [Display(Name = "Task Type")]
        public int TaskTypeId { get; set; }
        public int UserID { get; set; }
        public System.DateTime firstDate { get; set; }
        public System.DateTime secondDate { get; set; }
        //public string description { get; set; }
        public int times { get; set; }
        public string TaskReminderDatetimeString { get; set; }
        public string Subject { get; set; }
        [UIHint("TextArea")]
        public string Notes { get; set; }
        public string Description { get; set; }
        [UIHint("DateTimePicker")]
        public System.DateTime Start { get; set; }
        public System.DateTime End { get; set; }
        public string RecurrenceException { get; set; }
        public string RecurrenceRule { get; set; }
        public string StartTimezone { get; set; }
        public string EndTimezone { get; set; }
        public bool IsAllDay { get; set; }
        public string TaskType { get; set; }
        public long? ClientId { get; set; }
        public long? ProspectId { get; set; }
        public bool IsFullDay { get; set; }
        public string ThemeColor { get; set; }
        [UIHint("UserDropDown")]
        public long AssignToId { get; set; }

        public string ClientFullName { get; set; }
        public string ClientCompanyName { get; set; }
        public string ClientEmailAddress { get; set; }
        public string ClientMobileNo { get; set; }
        public string ClientAltMobileNo { get; set; }
        public long? ClientLeadCategoryId { get; set; }

        public string ProspectFullName { get; set; }
        public string ProspectCompanyName { get; set; }
        public string ProspectEmailAddress { get; set; }
        public string ProspectMobileNo { get; set; }
        public string ProspectAltMobileNo { get; set; }
        public long? ProspectLeadCategoryId { get; set; }
    }

    public class TaskReminderReportModel
    {
        public long TaskId { get; set; }

        public long ClientId { get; set; }

        public string ClientName { get; set; }

        public string Subject { get; set; }

        [Display(Name = "Task Type")]
        public int TaskTypeId { get; set; }

        [Display(Name = "Task Type")]
        public string TaskType { get; set; }

        [Display(Name = "Assign To")]
        public long AssignToId { get; set; }

        public string AssignToName { get; set; }

        [Display(Name = "Task Date Time")]
        public string TaskReminderDatetimeString { get; set; }

        public string Notes { get; set; }

        public int? IsDismiss { get; set; }

        public long UserId { get; set; }
    }
    public class ClientMarginReportModel
    {
        public long ClientId { get; set; }
        public string FullName { get; set; }
        public string AccountNo { get; set; }
        public string CompanyName { get; set; }
        public string EmailAddress { get; set; }
        public decimal? Margin { get; set; }
    }

    public class ProfitLossReportModel
    {
        public int TotalCount { get; set; }
        public long? ClientId { get; set; }
        public long? DealId { get; set; }
        public string FullName { get; set; }
        public string AccountNo { get; set; }
        public string CompanyName { get; set; }
        public string DealNo { get; set; }
        public decimal? AdditionalPLAmount { get; set; }
        public string AdditionalPLNotes { get; set; }
        public DateTime? TradeDate { get; set; }
        public string strTradeDate { get; set; }
    }

    public class NewClientReportModel
    {
        public long Client_Id { get; set; }
        public List<long> client_arr { get; set; }
        public int Count30Day { get; set; }
        public int Count180Day { get; set; }
        public double Avg180Day { get; set; }
        public int Count1Year { get; set; }
        public double Avg1Year { get; set; }
        public int CountTillToday { get; set; }
        public double Profit30Day { get; set; }
        public double Profit180Day { get; set; }
        public double AvgProfit180Day { get; set; }
        public double Profit1Year { get; set; }
        public double AvgProfit1Year { get; set; }
        public double ProfitTillToday { get; set; }
        public int TradeCountAfter6MO { get; set; }
        public int TradeCountAfter1Y { get; set; }
        public int ClientCount { get; set; }

       
    }
   
    public class VolumeReportByMonthodelMonthYear
    {
        public string MonthYear { get; set; }
        public string TraderName { get; set; }
        public decimal? ClientSoldAmt { get; set; }
        public decimal? GrossProfit { get; set; }
        public int TotalTrade { get; set; }
        public int ClientCount { get; set; }
        public decimal? AvgTradeSize { get; set; }
        public decimal? AvgTradeProfit { get; set; }
        public decimal? AvgSpread { get; set; }
        public string BusinessCategory { get; set; }
    }

   

}
