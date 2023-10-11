using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace FXAdminTransferConnex.Entities
{
    public class Last12MonthsCommissionModel
    {
        public int MonthId { get; set; }

        public string Month { get; set; }

        public string Year { get; set; }

        public decimal TotalCommission { get; set; }

        public decimal TargetCommission { get; set; }
    }

    public class TraderCommissions
    {
        public string Name { get; set; }

        public decimal Amount { get; set; }

        public decimal Kamount { get; set; }
    }

    public class SalesPersonCommissions
    {
        public string Name { get; set; }

        public decimal Amount { get; set; }

        public decimal Kamount { get; set; }
    }

    public class Top10ClientCommissions
    {
        public string FullName { get; set; }

        public string CompanyName { get; set; }

        public decimal Commisson { get; set; }
    }

    public class Last30DaysCommissionModel
    {
        public string DateString { get; set; }

        public decimal TotalCommission { get; set; }

        public decimal TargetCommission { get; set; }
    }

    public class Last6WeeksCommissionModel
    {
        public int WeekId { get; set; }

        public string WeekStartDate { get; set; }

        public string WeekEndDate { get; set; }

        public decimal TotalCommission { get; set; }

        public decimal TargetCommission { get; set; }
    }


    public class DashboardTaskReminderModel
    {

        [ScaffoldColumn(false)]
        public long TaskId { get; set; }

        [UIHint("TextBox")]
        [ScaffoldColumn(false)]
        public string Subject { get; set; }

        [ScaffoldColumn(false)]
        [Display(Name = "Task Type")]
        public string TaskType { get; set; }

        [ScaffoldColumn(false)]
        [HiddenInput(DisplayValue = false)]
        public string AssignToName { get; set; }

        [Required]
        [UIHint("DateTimePicker")]
        [Display(Name = "Task Date Time")]
        public DateTime TaskReminderDatetime { get; set; }

        [HiddenInput(DisplayValue = false)]
        public string TaskReminderDatetimeString { get; set; }

        [UIHint("TextArea")]
        public string Notes { get; set; }
        [ScaffoldColumn(false)]
        public long? ClientId { get; set; }
        [ScaffoldColumn(false)]
        public long? ProspectId { get; set; }
        [ScaffoldColumn(false)]
        public long? LeadCategoryId { get; set; }
        [ScaffoldColumn(false)]
        public string ThemeColor { get; set; }
    }

    public class MarketValueNotificationModel
    {
        public long NotificationId { get; set; }
        public long ClientId { get; set; }
        public long MarketOrderId { get; set; }
        public string Message { get; set; }
        public bool IsRead { get; set; }
        public string ClientName { get; set; }
        public string CompanyName { get; set; }
        public string StrNotifyDate { get; set; }
    }
}
