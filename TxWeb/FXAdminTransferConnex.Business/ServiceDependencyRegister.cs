using Autofac;
using Autofac.Core;
using FXAdminTransferConnex.Business.Caching;
using FXAdminTransferConnex.Business.User;
using FXAdminTransferConnex.Data.Databases;
using FXAdminTransferConnex.Data.Repository;
using FXAdminTransferConnex.Business.Common;
using FXAdminTransferConnex.Business.DashboardConfiguration;
using FXAdminTransferConnex.Business.Region;
using FXAdminTransferConnex.Business.Country;
using FXAdminTransferConnex.Business.City;
using FXAdminTransferConnex.Business.Client;
using FXAdminTransferConnex.Business.StagingDeal;
using FXAdminTransferConnex.Business.Deal;
using FXAdminTransferConnex.Business.Trader;
using FXAdminTransferConnex.Business.Dashboard;
using FXAdminTransferConnex.Business.Report;
using FXAdminTransferConnex.Business.BulkEmail;
using FXAdminTransferConnex.Business.TaskReminder;
using FXAdminTransferConnex.Business.Prospect;
using FXAdminTransferConnex.Business.ClientChat;
using FXAdminTransferConnex.Business.Ringcentral;
using FXAdminTransferConnex.Business.ReconciliationTeam;
using FXAdminTransferConnex.Business.Commission;

namespace FXAdminTransferConnex.Business
{
    /// <summary>
    /// Class ServiceDependencyRegister contains all ServiceDependencyRegister related methods and variable.
    /// </summary>
    // ReSharper disable once PartialTypeWithSinglePart
    public static partial class ServiceDependencyRegister
    {
        /// <summary>
        /// Resolves the specified builder.
        /// </summary>
        /// <param name="builder">The builder.</param>
        public static void Resolve(ContainerBuilder builder)
        {
            builder.RegisterGeneric(typeof(EfRepository<>)).As(typeof(IRepository<>)).InstancePerDependency();
            builder.RegisterType<MemoryCacheManager>().As<ICacheManager>().Named<ICacheManager>("Sundance_cache_static").SingleInstance();
            builder.RegisterType<FXBackOfficeSystemContext>().As<IDbContext>().InstancePerDependency();

            ////  DashboardService
            builder.RegisterType<DashboardService>().As<IDashboardService>()
                .WithParameter(ResolvedParameter.ForNamed<ICacheManager>("Sundance_cache_static")).InstancePerDependency();

            ////  UserService
            builder.RegisterType<UserService>().As<IUserService>()
                .WithParameter(ResolvedParameter.ForNamed<ICacheManager>("Sundance_cache_static")).InstancePerDependency();

            ////  CityService
            builder.RegisterType<CityService>().As<ICityService>()
                .WithParameter(ResolvedParameter.ForNamed<ICacheManager>("Sundance_cache_static")).InstancePerDependency();

            ////  CountryService
            builder.RegisterType<CountryServices>().As<ICountryServices>()
                .WithParameter(ResolvedParameter.ForNamed<ICacheManager>("Sundance_cache_static")).InstancePerDependency();

            /// CommonService
            builder.RegisterType<CommonService>().As<ICommonService>()
               .WithParameter(ResolvedParameter.ForNamed<ICacheManager>("Sundance_cache_static")).InstancePerDependency();

            //User Dashboard COnfiguration
            builder.RegisterType<DashboardConfigurationService>().As<IDashboardConfigurationService>()
              .WithParameter(ResolvedParameter.ForNamed<ICacheManager>("Sundance_cache_static")).InstancePerDependency();

            ////  RegionService
            builder.RegisterType<RegionService>().As<IRegionService>()
                .WithParameter(ResolvedParameter.ForNamed<ICacheManager>("Sundance_cache_static")).InstancePerDependency();

            ////  ClientService
            builder.RegisterType<ClientService>().As<IClientService>()
                .WithParameter(ResolvedParameter.ForNamed<ICacheManager>("Sundance_cache_static")).InstancePerDependency();

            ////  StagingDealService
            builder.RegisterType<StagingDealService>().As<IStagingDealService>()
                .WithParameter(ResolvedParameter.ForNamed<IStagingDealService>("Sundance_cache_static")).InstancePerDependency();

            ////  DealService
            builder.RegisterType<DealService>().As<IDealService>()
                .WithParameter(ResolvedParameter.ForNamed<IDealService>("Sundance_cache_static")).InstancePerDependency();

            ////  TraderService
            builder.RegisterType<TraderService>().As<ITraderService>()
                .WithParameter(ResolvedParameter.ForNamed<ITraderService>("Sundance_cache_static")).InstancePerDependency();

            ////  ReportService
            builder.RegisterType<ReportService>().As<IReportService>()
                .WithParameter(ResolvedParameter.ForNamed<IReportService>("Sundance_cache_static")).InstancePerDependency();

            ////  BulkEmailService
            builder.RegisterType<BulkEmailService>().As<IBulkEmailService>()
                .WithParameter(ResolvedParameter.ForNamed<IBulkEmailService>("Sundance_cache_static")).InstancePerDependency();

            ////  TaskReminderService
            builder.RegisterType<TaskReminderService>().As<ITaskReminderService>()
                .WithParameter(ResolvedParameter.ForNamed<ITaskReminderService>("Sundance_cache_static")).InstancePerDependency();

            ////  TaskReminderService
            builder.RegisterType<ProspectService>().As<IProspectService>()
                .WithParameter(ResolvedParameter.ForNamed<IProspectService>("Sundance_cache_static")).InstancePerDependency();

            //// SignalR Chat Service
            builder.RegisterType<ClientChatService>().As<IClientChatService>()
                .WithParameter(ResolvedParameter.ForNamed<IClientChatService>("Sundance_cache_static")).InstancePerDependency();

            //// Ringcentral Service
            builder.RegisterType<RingcentralService>().As<IRingcentralService>()
                .WithParameter(ResolvedParameter.ForNamed<IRingcentralService>("Sundance_cache_static")).InstancePerDependency();

            //// Reconciliation Team Service
            builder.RegisterType<ReconciliationTeamService>().As<IReconciliationTeamService>()
                .WithParameter(ResolvedParameter.ForNamed<IReconciliationTeamService>("Sundance_cache_static")).InstancePerDependency();
           
            ////  CommissionService
            builder.RegisterType<CommissionService>().As<ICommissionService>()
                .WithParameter(ResolvedParameter.ForNamed<ICacheManager>("Sundance_cache_static")).InstancePerDependency();
        }
    }
}
