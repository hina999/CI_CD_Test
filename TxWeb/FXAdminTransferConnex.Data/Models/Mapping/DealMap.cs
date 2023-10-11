using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace FXAdminTransferConnex.Data.Models.Mapping
{
    public class DealMap : EntityTypeConfiguration<Deal>
    {
        public DealMap()
        {
            // Primary Key
            this.HasKey(t => t.DealId);

            // Properties
            this.Property(t => t.TransactionReceipt)
                .HasMaxLength(50);

            this.Property(t => t.Status)
                .HasMaxLength(50);

            this.Property(t => t.AccountNumber)
                .HasMaxLength(50);

            this.Property(t => t.AccountName)
                .HasMaxLength(250);

            this.Property(t => t.FirstTrade)
                .HasMaxLength(20);

            this.Property(t => t.AccountOwnedBy)
                .HasMaxLength(50);

            this.Property(t => t.TradeType)
                .HasMaxLength(50);

            this.Property(t => t.BuyCcy)
                .HasMaxLength(50);

            this.Property(t => t.SellCcy)
                .HasMaxLength(50);

            this.Property(t => t.LC)
                .HasMaxLength(50);

            this.Property(t => t.DealType)
                .HasMaxLength(50);

            this.Property(t => t.Type)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Deal");
            this.Property(t => t.DealId).HasColumnName("DealId");
            this.Property(t => t.ClientId).HasColumnName("ClientId");
            this.Property(t => t.TransactionReceipt).HasColumnName("TransactionReceipt");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.AccountNumber).HasColumnName("AccountNumber");
            this.Property(t => t.AccountName).HasColumnName("AccountName");
            this.Property(t => t.OrderDate).HasColumnName("OrderDate");
            this.Property(t => t.FirstTrade).HasColumnName("FirstTrade");
            this.Property(t => t.AccountOwnedBy).HasColumnName("AccountOwnedBy");
            this.Property(t => t.TradeType).HasColumnName("TradeType");
            this.Property(t => t.BuyCcy).HasColumnName("BuyCcy");
            this.Property(t => t.BuyAmount).HasColumnName("BuyAmount");
            this.Property(t => t.SellCcy).HasColumnName("SellCcy");
            this.Property(t => t.SellAmount).HasColumnName("SellAmount");
            this.Property(t => t.ClientRate).HasColumnName("ClientRate");
            this.Property(t => t.Payment).HasColumnName("Payment");
            this.Property(t => t.LC).HasColumnName("LC");
            this.Property(t => t.LPBuyAmount).HasColumnName("LPBuyAmount");
            this.Property(t => t.AvgRate).HasColumnName("AvgRate");
            this.Property(t => t.LPSellAmount).HasColumnName("LPSellAmount");
            this.Property(t => t.RevenueBuyCcy).HasColumnName("RevenueBuyCcy");
            this.Property(t => t.RevenueSellCcy).HasColumnName("RevenueSellCcy");
            this.Property(t => t.GBPFee).HasColumnName("GBPFee");
            this.Property(t => t.GBPGrossRevenue).HasColumnName("GBPGrossRevenue");
            this.Property(t => t.GBPGrossProfit).HasColumnName("GBPGrossProfit");
            this.Property(t => t.GBPFinananceP_L).HasColumnName("GBPFinananceP&L");
            this.Property(t => t.GBPGPLF).HasColumnName("GBPGPLF");
            this.Property(t => t.TransferCost).HasColumnName("TransferCost");
            this.Property(t => t.GBPNetProfit).HasColumnName("GBPNetProfit");
            this.Property(t => t.GBPVolume).HasColumnName("GBPVolume");
            this.Property(t => t.Spread).HasColumnName("Spread");
            this.Property(t => t.ParentId).HasColumnName("ParentId");
            this.Property(t => t.CreatedBy).HasColumnName("CreatedBy");
            this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
            this.Property(t => t.UpdatedBy).HasColumnName("UpdatedBy");
            this.Property(t => t.UpdatedDate).HasColumnName("UpdatedDate");
            this.Property(t => t.DealType).HasColumnName("DealType");
            this.Property(t => t.Type).HasColumnName("Type");
            this.Property(t => t.ValueDate).HasColumnName("ValueDate");
        }
    }
}
