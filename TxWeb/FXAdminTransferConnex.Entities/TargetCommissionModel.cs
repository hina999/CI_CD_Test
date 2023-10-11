using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FXAdminTransferConnex.Entities
{
    public class TargetCommissionModel
    {
        public long TargetCommissionId { get; set; }
        public long? ClientId { get; set; }
        public string CompanyName { get; set; }
        public long? TraderId { get; set; }
        public string TraderName { get; set; }
        public long? SalesPersonId { get; set; }
        public string SalesPersonName { get; set; }
        public string TargetToPerson { get; set; }
        public int? TargetYear { get; set; }
        public decimal? TargetCommission { get; set; }
        public string TargetType { get; set; }
        public bool IsActive { get; set; }
        public long LoggedinUserId { get; set; }
        public string Status { get; set; }

    }
    public class TargetDurationModel
    {
        public long TargetCommissionId { get; set; }
        public string Name { get; set; }
        public string Month { get; set; }
        public int weekNum { get; set; }
        public DateTime? weekStart { get; set; }
        public DateTime? weekFinish { get; set; }
        public string strweekStart { get; set; }
        public string strweekFinish { get; set; }
        public decimal? TargetCommission { get; set; }
        public string StrDay { get; set; }
        public DateTime? DTDay { get; set; }
        public string strDTDay { get; set; }
        public string strDayDT { get; set; }


    }
}
