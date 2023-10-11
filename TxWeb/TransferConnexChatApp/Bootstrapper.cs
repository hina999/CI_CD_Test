using Autofac;
using Autofac.Integration.Mvc;
using FXAdminTransferConnex.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TransferConnexChatApp
{
    public static class Bootstrapper
    {
        public static void Resolve(ContainerBuilder builder)
        {
            if (HttpContext.Current != null)
            {
                builder.Register(c =>
                 new HttpContextWrapper(HttpContext.Current) as HttpContextBase)
                   .As<HttpContextBase>()
                   .InstancePerDependency();
            }

            builder.RegisterControllers(typeof(MvcApplication).Assembly);
            builder.Register(c => c.Resolve<HttpContextBase>().Request)
                .As<HttpRequestBase>()
                .InstancePerDependency();
            builder.Register(c => c.Resolve<HttpContextBase>().Response)
                .As<HttpResponseBase>()
                .InstancePerDependency();
            builder.Register(c => c.Resolve<HttpContextBase>().Server)
                .As<HttpServerUtilityBase>()
                .InstancePerDependency();
            builder.Register(c => c.Resolve<HttpContextBase>().Session)
                .As<HttpSessionStateBase>()
                .InstancePerDependency();

            ////builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly()).Where(t => !t.IsAbstract && typeof(ApiController).IsAssignableFrom(t))
            ////    .InstancePerMatchingLifetimeScope();

            builder.RegisterControllers(typeof(MvcApplication).Assembly);
            // builder.RegisterApiControllers(typeof(RestaurantPortalAPI.WebApiApplication).Assembly);
            ServiceDependencyRegister.Resolve(builder);

            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
            //// Create the depenedency resolver.
            //var resolver = new AutofacWebApiDependencyResolver(container);

            //// Configure Web API with the dependency resolver.
            //GlobalConfiguration.Configuration.DependencyResolver = resolver;
        }
    }
    public static class EngineContext
    {
        /// <summary>
        /// Resolves service using Auto FAC
        /// </summary>
        /// <typeparam name="T">T entity</typeparam>
        /// <returns>Return T</returns>
        public static T Resolve<T>()
        {
            return AutofacDependencyResolver.Current.RequestLifetimeScope.Resolve<T>();
        }
    }
}