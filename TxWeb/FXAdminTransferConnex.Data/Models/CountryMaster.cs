using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FXAdminTransferConnex.Data.Models
{
    public partial class CountryMaster
    {        
        public Int32 CountryID { get; set; }       
        public String CountryName { get; set; }        
        public Int32 RegionID { get; set; }
        public String RegionName { get; set; }
        public DateTime CreatedDate { get; set; }        
        public Int32 CreatedBy { get; set; }        
        public DateTime? UpdatedDate { get; set; }        
        public Int32? UpdateBy { get; set; }
    }
}
