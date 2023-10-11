using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace FXAdminTransferConnex.Data.Models.Mapping
{
    public class ClientBrokerMap : EntityTypeConfiguration<ClientBroker>
    {
        public ClientBrokerMap()
        {
            // Primary Key
            this.HasKey(t => t.ClientBrokerId);

            // Properties
            // Table & Column Mappings
            this.ToTable("ClientBroker");
            this.Property(t => t.ClientBrokerId).HasColumnName("ClientBrokerId");
            this.Property(t => t.ClientId).HasColumnName("ClientId");
            this.Property(t => t.BrokerId).HasColumnName("BrokerId");
            this.Property(t => t.Commission).HasColumnName("Commission");
        }
    }
}
