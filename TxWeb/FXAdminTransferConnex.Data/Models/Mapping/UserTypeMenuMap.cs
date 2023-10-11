using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace FXAdminTransferConnex.Data.Models.Mapping
{
    public class UserTypeMenuMap : EntityTypeConfiguration<UserTypeMenu>
    {
        public UserTypeMenuMap()
        {
            // Primary Key
            this.HasKey(t => t.UserTypeMenuId);

            // Properties
            // Table & Column Mappings
            this.ToTable("UserTypeMenus");
            this.Property(t => t.UserTypeMenuId).HasColumnName("UserTypeMenuId");
            this.Property(t => t.MenuId).HasColumnName("MenuId");
            this.Property(t => t.UserTypeId).HasColumnName("UserTypeId");
            this.Property(t => t.IsView).HasColumnName("IsView");
            this.Property(t => t.IsAdd).HasColumnName("IsAdd");
            this.Property(t => t.IsDelete).HasColumnName("IsDelete");
            this.Property(t => t.IsEdit).HasColumnName("IsEdit");

            // Relationships
            this.HasRequired(t => t.Menu)
                .WithMany(t => t.UserTypeMenus)
                .HasForeignKey(d => d.MenuId);
            this.HasRequired(t => t.UserType)
                .WithMany(t => t.UserTypeMenus)
                .HasForeignKey(d => d.UserTypeId);

        }
    }
}
