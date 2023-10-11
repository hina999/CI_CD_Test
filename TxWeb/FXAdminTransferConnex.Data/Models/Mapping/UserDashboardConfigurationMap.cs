using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace FXAdminTransferConnex.Data.Models.Mapping
{
    public class UserDashboardConfigurationMap : EntityTypeConfiguration<UserDashboardConfiguration>
    {
        public UserDashboardConfigurationMap()
        {
            // Primary Key
            this.HasKey(t => t.UserDCId);

            // Properties
            // Table & Column Mappings
            this.ToTable("UserDashboardConfiguration");
            this.Property(t => t.UserDCId).HasColumnName("UserDCId");
            this.Property(t => t.WizardId).HasColumnName("WizardId");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.Visibility).HasColumnName("Visibility");
            this.Property(t => t.DisplayOrder).HasColumnName("DisplayOrder");
            this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
            this.Property(t => t.UpdatedDate).HasColumnName("UpdatedDate");
        }
    }
}
