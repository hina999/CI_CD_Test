using System;
using System.Collections.Generic;

namespace FXAdminTransferConnex.Data.Models
{
    public partial class UserType
    {
        public UserType()
        {
            this.UserTypeMenus = new List<UserTypeMenu>();
        }

        public byte UserTypeId { get; set; }
        public string Name { get; set; }
        public virtual ICollection<UserTypeMenu> UserTypeMenus { get; set; }
    }
}
