using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace FXAdminTransferConnex.Data.Models.Mapping
{
    public class BrokerMap : EntityTypeConfiguration<Broker>
    {
        public BrokerMap()
        {
            // Primary Key
            this.HasKey(t => t.BrokerId);

            // Properties
            // Table & Column Mappings
            this.ToTable("Broker");
            this.Property(t => t.BrokerId).HasColumnName("BrokerId");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.IsActive).HasColumnName("IsActive");
            this.Property(t => t.DefaultCommission).HasColumnName("DefaultCommission");
            this.Property(t => t.RegisteredDate).HasColumnName("RegisteredDate");
        }
    }
}
