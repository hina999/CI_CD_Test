using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace FXAdminTransferConnex.Data.Models.Mapping
{
    public class StagingDealBrokerAgentMap : EntityTypeConfiguration<StagingDealBrokerAgent>
    {
        public StagingDealBrokerAgentMap()
        {
            // Primary Key
            this.HasKey(t => t.StagingDealBrokerAgentId);

            // Properties
            // Table & Column Mappings
            this.ToTable("StagingDealBrokerAgent");
            this.Property(t => t.StagingDealBrokerAgentId).HasColumnName("StagingDealBrokerAgentId");
            this.Property(t => t.StagingDealId).HasColumnName("StagingDealId");
            this.Property(t => t.BrokerAgentId).HasColumnName("BrokerAgentId");
            this.Property(t => t.UserTypeId).HasColumnName("UserTypeId");
            this.Property(t => t.Commission).HasColumnName("Commission");
            this.Property(t => t.CreatedBy).HasColumnName("CreatedBy");
            this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
            this.Property(t => t.UpdatedBy).HasColumnName("UpdatedBy");
            this.Property(t => t.UpdatedDate).HasColumnName("UpdatedDate");

            // Relationships
            this.HasOptional(t => t.StagingDeal)
                .WithMany(t => t.StagingDealBrokerAgents)
                .HasForeignKey(d => d.StagingDealId);
            this.HasOptional(t => t.UserMaster)
                .WithMany(t => t.StagingDealBrokerAgents)
                .HasForeignKey(d => d.BrokerAgentId);

        }
    }
}
