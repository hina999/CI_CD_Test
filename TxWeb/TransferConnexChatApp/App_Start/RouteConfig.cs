﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace TransferConnexChatApp
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapMvcAttributeRoutes();
            routes.MapRoute("Default", "{controller}/{action}/{id}", new
            {
                controller = "Login",
                action = "Login",
                id = UrlParameter.Optional
            });
        }
    }
}
