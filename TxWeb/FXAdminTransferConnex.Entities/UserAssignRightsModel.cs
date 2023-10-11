using FXAdminTransferConnex.Entities.LocalizationResource;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FXAdminTransferConnex.Entities
{
    public class UserAssignRightsModel
    {
        [Required]
        public long UserId { get; set; }
        [Required]
        public int UserTypeId { get; set; }
        public List<UserRoleOperation> roleOperationList { get; set; }
        public string strRoleRights { get; set; }
    }

    public class UserRoleOperation
    {
        public int MenuId { get; set; }
        public bool IsView { get; set; }
        public bool IsDelete { get; set; }
        public bool IsAdd { get; set; }
        public bool IsEdit { get; set; }
    }
}

