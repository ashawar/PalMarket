using Newtonsoft.Json;
using PalMarket.Data;
using PalMarket.Data.Migrations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace PalMarket.API
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            // Init database
            System.Data.Entity.Database.SetInitializer(new PalMarketSeedData());

            GlobalConfiguration.Configure(WebApiConfig.Register);

            // Autofac and Automapper configurations
            Bootstrapper.Run();
            
            //new DbMigrations().InitializeDatabase();
        }
    }
}
