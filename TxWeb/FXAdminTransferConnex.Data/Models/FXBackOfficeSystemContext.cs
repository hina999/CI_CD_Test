using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using FXAdminTransferConnex.Data.Helper;
using FXAdminTransferConnex.Data.Models.Mapping;

namespace FXAdminTransferConnex.Data.Models
{
    public partial class FXBackOfficeSystemContext : DbContext
    {
        static FXBackOfficeSystemContext()
        {
            Database.SetInitializer<FXBackOfficeSystemContext>(null);
        }
        private static string encryptedConnectionString = ConfigurationManager.ConnectionStrings["FXBackOfficeSystemContext"].ConnectionString;
        private static string decryptedConnectionString = AESEncryptionDecryptionHelper.Decrypt(encryptedConnectionString);
        public FXBackOfficeSystemContext()
            : base(decryptedConnectionString)
        {
        }

        public DbSet<ClientAgent> ClientAgents { get; set; }
        public DbSet<ClientBroker> ClientBrokers { get; set; }
        public DbSet<ClientCommunication> ClientCommunications { get; set; }
        public DbSet<ClientMaster> ClientMasters { get; set; }
        public DbSet<CommunicationType> CommunicationTypes { get; set; }
        public DbSet<Deal> Deals { get; set; }
        public DbSet<DealBrokerAgent> DealBrokerAgents { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<StagingDeal> StagingDeals { get; set; }
        public DbSet<StagingDealBrokerAgent> StagingDealBrokerAgents { get; set; }
        public DbSet<UserMaster> UserMasters { get; set; }
        public DbSet<UserTypeMenu> UserTypeMenus { get; set; }
        public DbSet<UserType> UserTypes { get; set; }
        public DbSet<ProspectClientAgent> ProspectClientAgents { get; set; }
        public DbSet<ProspectClientBroker> ProspectClientBrokers { get; set; }
        public DbSet<ProspectClientCommunication> ProspectClientCommunications { get; set; }
        public DbSet<ProspectClientMaster> ProspectClientMasters { get; set; }
        public DbSet<UserDashboardConfiguration> UserDashboardConfiguration { get; set; }
        public DbSet<TransactionFollowUp> TransactionFollowUp { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new ClientAgentMap());
            modelBuilder.Configurations.Add(new ClientBrokerMap());
            modelBuilder.Configurations.Add(new ClientCommunicationMap());
            modelBuilder.Configurations.Add(new ClientMasterMap());
            modelBuilder.Configurations.Add(new CommunicationTypeMap());
            modelBuilder.Configurations.Add(new DealMap());
            modelBuilder.Configurations.Add(new DealBrokerAgentMap());
            modelBuilder.Configurations.Add(new MenuMap());
            modelBuilder.Configurations.Add(new StagingDealMap());
            modelBuilder.Configurations.Add(new StagingDealBrokerAgentMap());
            modelBuilder.Configurations.Add(new UserMasterMap());
            modelBuilder.Configurations.Add(new UserTypeMenuMap());
            modelBuilder.Configurations.Add(new UserTypeMap());
            modelBuilder.Configurations.Add(new ProspectClientAgentMap());
            modelBuilder.Configurations.Add(new ProspectClientBrokerMap());
            modelBuilder.Configurations.Add(new ProspectClientCommunicationMap());
            modelBuilder.Configurations.Add(new ProspectClientMasterMap());
            modelBuilder.Configurations.Add(new UserDashboardConfigurationMap());
            modelBuilder.Configurations.Add(new TransactionFollowUpMap());
        }
    }
}
