using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FXAdminTransferConnex.Entities
{
    public class GlobalSearchModel
    {
        public long ID { get; set; }
        public string FullName { get; set; }
        public string CompanyName { get; set; }
        public string MobileNo { get; set; }
        public string AltMobileNo { get; set; }
        public string EmailAddress { get; set; }
        public string AccountNo { get; set; }
        public string PastCommDetail { get; set; }
        public string ResutType { get; set; }
    }
}
