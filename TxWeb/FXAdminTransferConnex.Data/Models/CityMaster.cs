using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FXAdminTransferConnex.Data.Models
{
    public partial class CityMaster
    {
        
        public Int32 CityID { get; set; }        
        public String CityName { get; set; }
        
        public Int32 CountryID { get; set; }

        public String CountryName { get; set; }
    }
}
