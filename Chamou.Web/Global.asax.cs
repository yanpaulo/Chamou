using AutoMapper;
using Chamou.Web.Models.DTOs;
using Chamou.Web.Models.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Chamou.Web
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            Mapper.Initialize(cfg => {
                cfg.CreateMap<Place, PlaceDTO>();
                cfg.CreateMap<Attendant, AttendantDTO>();
            });
            if (!System.Diagnostics.Debugger.IsAttached)
            {
                Database.SetInitializer(new MigrateDatabaseToLatestVersion<ChamouContext, Migrations.Configuration>());
            }
            SqlServerTypes.Utilities.LoadNativeAssemblies(Server.MapPath("~/bin"));
            //Tcp.TcpManagerConfig.Initialize();
        }
    }
}
