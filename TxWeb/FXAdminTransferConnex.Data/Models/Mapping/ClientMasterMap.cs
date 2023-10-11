using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace FXAdminTransferConnex.Data.Models.Mapping
{
    public class ClientMasterMap : EntityTypeConfiguration<ClientMaster>
    {
        public ClientMasterMap()
        {
            // Primary Key
            this.HasKey(t => t.ClientId);

            // Properties
            this.Property(t => t.ForeName)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.LastName)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.CompanyName)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Address)
                .HasMaxLength(200);

            this.Property(t => t.MobileNumber)
                .HasMaxLength(50);

            this.Property(t => t.PhoneNumber)
                .IsFixedLength()
                .HasMaxLength(10);

            this.Property(t => t.EmailAddress)
                .HasMaxLength(250);

            this.Property(t => t.BosACNumber)
                .HasMaxLength(50);

            this.Property(t => t.Fax)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("ClientMaster");
            this.Property(t => t.ClientId).HasColumnName("ClientId");
            this.Property(t => t.ForeName).HasColumnName("ForeName");
            this.Property(t => t.LastName).HasColumnName("LastName");
            this.Property(t => t.CompanyName).HasColumnName("CompanyName");
            this.Property(t => t.Type).HasColumnName("Type");
            this.Property(t => t.Address).HasColumnName("Address");
            this.Property(t => t.MobileNumber).HasColumnName("MobileNumber");
            this.Property(t => t.PhoneNumber).HasColumnName("PhoneNumber");
            this.Property(t => t.EmailAddress).HasColumnName("EmailAddress");
            this.Property(t => t.BosACNumber).HasColumnName("BosACNumber");
            this.Property(t => t.IsActive).HasColumnName("IsActive");
            this.Property(t => t.ACOpenedDate).HasColumnName("ACOpenedDate");
            this.Property(t => t.Fax).HasColumnName("Fax");
            this.Property(t => t.RegistrationCost).HasColumnName("RegistrationCost");
            this.Property(t => t.CreatedBy).HasColumnName("CreatedBy");
            this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
            this.Property(t => t.UpdatedBy).HasColumnName("UpdatedBy");
            this.Property(t => t.UpdatedDate).HasColumnName("UpdatedDate");
            this.Property(t => t.DoNotContactAgain).HasColumnName("DoNotContactAgain");
            this.Property(t => t.ClientMasterTitle).HasColumnName("ClientMasterTitle");

        }
    }
}
