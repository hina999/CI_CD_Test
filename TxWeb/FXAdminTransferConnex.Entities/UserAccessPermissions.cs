namespace FXAdminTransferConnex.Entities
{
    public class UserAccessPermissions
    {
        public int? MenuId { get; set; }

        public int? ParentMenuId { get; set; }

        public string Name { get; set; }

        public string ImagePath { get; set; }

        public string Controller { get; set; }

        public string Action { get; set; }

        public int? DispalyOrder { get; set; }

        public bool? IsView { get; set; }

        public bool? IsDelete { get; set; }

        public bool? IsAdd { get; set; }

        public bool? IsEdit { get; set; }
    }
}
