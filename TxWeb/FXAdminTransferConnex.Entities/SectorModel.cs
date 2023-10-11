using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FXAdminTransferConnex.Entities
{
   public  class SectorModel
    {
        public long SectorId { get; set; }
        public long CategoryId { get; set; }
        public string SectorName { get; set; }
        public bool IsActive { get; set; }
    }
}
