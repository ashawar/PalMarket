using Autofac;
using Microsoft.Owin.Security.DataProtection;
using Owin;
using PalMarket.Data.Infrastructure;
using PalMarket.Data.Repositories;
using PalMarket.Domain;
using PalMarket.Domain.Contracts;
using System;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Web;

namespace PalMarket.API.Autofac
{
    public class WebModule : Module
    {
        private readonly IAppBuilder _app;

        public WebModule(IAppBuilder app)
        {
            _app = app;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerRequest();
            builder.RegisterType<DbFactory>().As<IDbFactory>().InstancePerRequest();

            // Repositories
            builder.RegisterAssemblyTypes(typeof(OfferRepository).Assembly)
                .Where(t => t.Name.EndsWith("Repository"))
                .AsImplementedInterfaces().InstancePerRequest();

            // Services
            builder.RegisterAssemblyTypes(typeof(OfferService).Assembly)
               .Where(t => t.Name.EndsWith("Service"))
               .AsImplementedInterfaces().InstancePerRequest();

            //builder.RegisterAssemblyModules(Assembly.GetExecutingAssembly());

            base.Load(builder);
        }
    }
}