using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using Autofac;
using System.Web.Http;
using Autofac.Integration.WebApi;
using System.Reflection;
using Autofac.Integration.Mvc;
using System.Web.Mvc;
using PalMarket.API.Autofac;

[assembly: OwinStartup(typeof(PalMarket.API.Startup))]

namespace PalMarket.API
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=316888
            var builder = new ContainerBuilder();

            // STANDARD WEB API SETUP:

            // Get your HttpConfiguration. In OWIN, you'll create one
            // rather than using GlobalConfiguration.
            var config = new HttpConfiguration();
            // Register your Web API controllers.
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            builder.RegisterWebApiFilterProvider(config);
            builder.RegisterModule(new WebModule(app));

            // Register your MVC controllers.
            builder.RegisterControllers(Assembly.GetExecutingAssembly());

            // OPTIONAL: Register model binders that require DI.
            builder.RegisterModelBinders(Assembly.GetExecutingAssembly());
            builder.RegisterModelBinderProvider();

            // OPTIONAL: Register web abstractions like HttpContextBase.
            builder.RegisterModule<AutofacWebTypesModule>();

            // OPTIONAL: Enable property injection in view pages.
            builder.RegisterSource(new ViewRegistrationSource());

            // OPTIONAL: Enable property injection into action filters.
            builder.RegisterFilterProvider();

            // register config
            builder.Register(ct => config).AsSelf().SingleInstance();
            //HelpPageConfig.Register(config);

            // Run other optional steps, like registering filters,
            // per-controller-type services, etc., then set the dependency resolver
            // to be Autofac.
            var container = builder.Build();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            // OWIN WEB API SETUP:

            // Register the Autofac middleware FIRST, then the Autofac Web API middleware,
            // and finally the standard Web API middleware.
            app.UseAutofacMiddleware(container);
            app.UseAutofacWebApi(config);
            app.UseAutofacMvc();

            WebApiConfig.Register(config);
            ConfigureAuth(app);

            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
            app.UseWebApi(config);
            config.EnsureInitialized();
        }
    }
}
