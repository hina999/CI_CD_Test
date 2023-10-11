using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace FXAdminTransferConnex.Data.Models.Mapping
{
    public class UserMasterMap : EntityTypeConfiguration<UserMaster>
    {
        public UserMasterMap()
        {
            // Primary Key
            this.HasKey(t => t.UserId);

            // Properties
            this.Property(t => t.FirstName)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.LastName)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.EmailAddress)
                .HasMaxLength(250);

            this.Property(t => t.Password)
                .HasMaxLength(500);

            // Table & Column Mappings
            this.ToTable("UserMaster");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.UserTitle).HasColumnName("UserTitle");
            this.Property(t => t.FirstName).HasColumnName("FirstName");
            this.Property(t => t.LastName).HasColumnName("LastName");
            this.Property(t => t.EmailAddress).HasColumnName("EmailAddress");
            this.Property(t => t.Password).HasColumnName("Password");
            this.Property(t => t.UserTypeId).HasColumnName("UserTypeId");
            this.Property(t => t.RegisteredOn).HasColumnName("RegisteredOn");
            this.Property(t => t.IsActive).HasColumnName("IsActive");
            this.Property(t => t.CreatedBy).HasColumnName("CreatedBy");
            this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
            this.Property(t => t.UpdatedBy).HasColumnName("UpdatedBy");
            this.Property(t => t.UpdatedDate).HasColumnName("UpdatedDate");
            this.Property(t => t.ClientId).HasColumnName("ClientId");
            
        }
    }
}
