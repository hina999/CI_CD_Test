using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace FXAdminTransferConnex.Data.Models.Mapping
{
    public class ProspectClientCommunicationMap : EntityTypeConfiguration<ProspectClientCommunication>
    {
        public ProspectClientCommunicationMap()
        {
            // Primary Key
            this.HasKey(t => t.ProspectClientCommunicationId);

            // Properties
            this.Property(t => t.CommunicationNotes)
                .HasMaxLength(200);

            // Table & Column Mappings
            this.ToTable("ProspectClientCommunication");
            this.Property(t => t.ProspectClientCommunicationId).HasColumnName("ProspectClientCommunicationId");
            this.Property(t => t.ClientId).HasColumnName("ClientId");
            this.Property(t => t.CommunicationDate).HasColumnName("CommunicationDate");
            this.Property(t => t.CommunicationType).HasColumnName("CommunicationType");
            this.Property(t => t.CommunicationNotes).HasColumnName("CommunicationNotes");
            this.Property(t => t.NextFollowUpDate).HasColumnName("NextFollowUpDate");
        }
    }
}
