using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FXAdminTransferConnex.Data.Models.Mapping
{
    public class TransactionFollowUpMap : EntityTypeConfiguration<TransactionFollowUp>
    {
        public TransactionFollowUpMap()
        {
            // Primary Key
            this.HasKey(t => t.TransactionId);

            // Properties
            // Table & Column Mappings
            this.ToTable("TransactionFollowUp");
            this.Property(t => t.TransactionId).HasColumnName("TransactionId");
            this.Property(t => t.CustomerId).HasColumnName("CustomerId");
            this.Property(t => t.BuyCcy).HasColumnName("BuyCcy");
            this.Property(t => t.BuyAmount).HasColumnName("BuyAmount");
            this.Property(t => t.SellCcy).HasColumnName("SellCcy");
            this.Property(t => t.SellAmount).HasColumnName("SellAmount");
            this.Property(t => t.TransactionDate).HasColumnName("TransactionDate");
            this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
            this.Property(t => t.UpdatedDate).HasColumnName("UpdatedDate");
            this.Property(t => t.CreatedBy).HasColumnName("CreatedBy");
            this.Property(t => t.UpdatedBy).HasColumnName("UpdatedBy");
            this.Property(t => t.IsCompleted).HasColumnName("IsCompleted");
            this.Property(t => t.Note).HasColumnName("Note");

        }
    }
}
