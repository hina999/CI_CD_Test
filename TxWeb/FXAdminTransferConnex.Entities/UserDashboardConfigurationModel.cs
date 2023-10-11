using FXAdminTransferConnex.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FXAdminTransferConnex.Entities
{
    public class UserDashboardConfigurationModel
    {
        [ScaffoldColumn(false)]
        public int UserDCId { get; set;}

        public int UserId { get; set; }

        public int WizardId { get; set; }

        public bool Visibility { get; set; }

        public int DisplayOrder { get; set; }

        public Nullable<DateTime> CreatedDate { get; set; }

        public Nullable<DateTime> UpdatedDate { get; set; }

        public string WizardName { get; set; }

        public string PartialViewName { get; set; }

        public List<UserDashboardConfigurationModel> listdata { get; set; }

        public string UserName { get; set; }

        public string UserTypeName { get; set; }
    }
}
