using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace FXAdminTransferConnex.Data.Models.Mapping
{
    public class ProspectClientAgentMap : EntityTypeConfiguration<ProspectClientAgent>
    {
        public ProspectClientAgentMap()
        {
            // Primary Key
            this.HasKey(t => t.ProspectClientAgentId);

            // Properties
            // Table & Column Mappings
            this.ToTable("ProspectClientAgent");
            this.Property(t => t.ProspectClientAgentId).HasColumnName("ProspectClientAgentId");
            this.Property(t => t.ClientId).HasColumnName("ClientId");
            this.Property(t => t.AgentId).HasColumnName("AgentId");
            this.Property(t => t.Commission).HasColumnName("Commission");
        }
    }
}
