using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FXAdminTransferConnex.Entities
{
    public class CategoryModel
    {
        public long CategoryId { get; set; }

        public string CategoryName { get; set; }

        public bool IsActive { get; set; }
    }
}
