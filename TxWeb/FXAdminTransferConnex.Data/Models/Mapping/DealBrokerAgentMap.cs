using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace FXAdminTransferConnex.Data.Models.Mapping
{
    public class DealBrokerAgentMap : EntityTypeConfiguration<DealBrokerAgent>
    {
        public DealBrokerAgentMap()
        {
            // Primary Key
            this.HasKey(t => t.DealBrokerAgentId);

            // Properties
            // Table & Column Mappings
            this.ToTable("DealBrokerAgent");
            this.Property(t => t.DealBrokerAgentId).HasColumnName("DealBrokerAgentId");
            this.Property(t => t.DealId).HasColumnName("DealId");            
            this.Property(t => t.BrokerAgentId).HasColumnName("BrokerAgentId");
            this.Property(t => t.UserTypeId).HasColumnName("UserTypeId");
            this.Property(t => t.Commission).HasColumnName("Commission");
            this.Property(t => t.CreatedBy).HasColumnName("CreatedBy");
            this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
            this.Property(t => t.UpdatedBy).HasColumnName("UpdatedBy");
            this.Property(t => t.UpdatedDate).HasColumnName("UpdatedDate");

            // Relationships
            this.HasOptional(t => t.Deal)
                .WithMany(t => t.DealBrokerAgents)
                .HasForeignKey(d => d.DealId);
            this.HasOptional(t => t.UserMaster)
                .WithMany(t => t.DealBrokerAgents)
                .HasForeignKey(d => d.BrokerAgentId);

        }
    }
}
