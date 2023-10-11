using System;
using System.Collections.Generic;

namespace FXAdminTransferConnex.Data.Models
{
    public partial class Menu
    {
        public Menu()
        {
            this.Menu1 = new List<Menu>();
            this.UserTypeMenus = new List<UserTypeMenu>();
        }

        public int MenuId { get; set; }
        public Nullable<int> ParentMenuId { get; set; }
        public string Name { get; set; }
        public string ImagePath { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public bool IsActive { get; set; }
        public Nullable<int> DispalyOrder { get; set; }
        public virtual ICollection<Menu> Menu1 { get; set; }
        public virtual Menu Menu2 { get; set; }
        public virtual ICollection<UserTypeMenu> UserTypeMenus { get; set; }
    }
}
