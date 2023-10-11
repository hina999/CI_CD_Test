using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace FXAdminTransferConnex.Data.Models.Mapping
{
    public class AgentMap : EntityTypeConfiguration<Agent>
    {
        public AgentMap()
        {
            // Primary Key
            this.HasKey(t => t.AgentId);

            // Properties
            // Table & Column Mappings
            this.ToTable("Agent");
            this.Property(t => t.AgentId).HasColumnName("AgentId");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.IsActive).HasColumnName("IsActive");
            this.Property(t => t.DefaultCommission).HasColumnName("DefaultCommission");
            this.Property(t => t.RegisteredDate).HasColumnName("RegisteredDate");
        }
    }
}
