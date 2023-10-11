using System;
using System.Collections.Generic;

namespace FXAdminTransferConnex.Data.Models
{
    public partial class UserDashboardConfiguration
    {
        public int UserDCId { get; set; }

        public int UserId { get; set; }

        public int WizardId { get; set; }

        public bool Visibility { get; set; }

        public int DisplayOrder { get; set; }

        public Nullable<DateTime> CreatedDate { get; set; }

        public Nullable<DateTime> UpdatedDate { get; set; }

        
    }
}
