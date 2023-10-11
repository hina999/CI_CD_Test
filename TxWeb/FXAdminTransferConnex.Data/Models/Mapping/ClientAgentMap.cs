using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace FXAdminTransferConnex.Data.Models.Mapping
{
    public class ClientAgentMap : EntityTypeConfiguration<ClientAgent>
    {
        public ClientAgentMap()
        {
            // Primary Key
            this.HasKey(t => t.ClientAgentId);

            // Properties
            // Table & Column Mappings
            this.ToTable("ClientAgent");
            this.Property(t => t.ClientAgentId).HasColumnName("ClientAgentId");
            this.Property(t => t.ClientId).HasColumnName("ClientId");
            this.Property(t => t.AgentId).HasColumnName("AgentId");
            this.Property(t => t.Commission).HasColumnName("Commission");
        }
    }
}
