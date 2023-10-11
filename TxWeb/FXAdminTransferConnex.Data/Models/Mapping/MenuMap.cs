using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace FXAdminTransferConnex.Data.Models.Mapping
{
    public class MenuMap : EntityTypeConfiguration<Menu>
    {
        public MenuMap()
        {
            // Primary Key
            this.HasKey(t => t.MenuId);

            // Properties
            this.Property(t => t.MenuId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.ImagePath)
                .HasMaxLength(100);

            this.Property(t => t.Controller)
                .HasMaxLength(50);

            this.Property(t => t.Action)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Menu");
            this.Property(t => t.MenuId).HasColumnName("MenuId");
            this.Property(t => t.ParentMenuId).HasColumnName("ParentMenuId");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.ImagePath).HasColumnName("ImagePath");
            this.Property(t => t.Controller).HasColumnName("Controller");
            this.Property(t => t.Action).HasColumnName("Action");
            this.Property(t => t.IsActive).HasColumnName("IsActive");
            this.Property(t => t.DispalyOrder).HasColumnName("DispalyOrder");

            // Relationships
            this.HasOptional(t => t.Menu2)
                .WithMany(t => t.Menu1)
                .HasForeignKey(d => d.ParentMenuId);

        }
    }
}
