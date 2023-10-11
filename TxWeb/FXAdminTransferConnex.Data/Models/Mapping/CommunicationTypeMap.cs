using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace FXAdminTransferConnex.Data.Models.Mapping
{
    public class CommunicationTypeMap : EntityTypeConfiguration<CommunicationType>
    {
        public CommunicationTypeMap()
        {
            // Primary Key
            this.HasKey(t => t.CommunicationTypeId);

            // Properties
            this.Property(t => t.TypeName)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("CommunicationType");
            this.Property(t => t.CommunicationTypeId).HasColumnName("CommunicationTypeId");
            this.Property(t => t.TypeName).HasColumnName("TypeName");
            this.Property(t => t.Active).HasColumnName("Active");
        }
    }
}
