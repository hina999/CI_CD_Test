using System;
using System.Collections.Generic;

namespace FXAdminTransferConnex.Data.Models
{
    public partial class UserTypeMenu
    {
        public int UserTypeMenuId { get; set; }
        public int MenuId { get; set; }
        public byte UserTypeId { get; set; }
        public bool IsView { get; set; }
        public bool IsAdd { get; set; }
        public bool IsDelete { get; set; }
        public bool IsEdit { get; set; }
        public virtual Menu Menu { get; set; }
        public virtual UserType UserType { get; set; }
    }
}
