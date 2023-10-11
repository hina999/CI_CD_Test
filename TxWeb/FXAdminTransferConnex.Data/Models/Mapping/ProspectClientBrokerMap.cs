using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace FXAdminTransferConnex.Data.Models.Mapping
{
    public class ProspectClientBrokerMap : EntityTypeConfiguration<ProspectClientBroker>
    {
        public ProspectClientBrokerMap()
        {
            // Primary Key
            this.HasKey(t => t.ProspectClientBrokerId);

            // Properties
            // Table & Column Mappings
            this.ToTable("ProspectClientBroker");
            this.Property(t => t.ProspectClientBrokerId).HasColumnName("ProspectClientBrokerId");
            this.Property(t => t.ClientId).HasColumnName("ClientId");
            this.Property(t => t.BrokerId).HasColumnName("BrokerId");
            this.Property(t => t.Commission).HasColumnName("Commission");
        }
    }
}
