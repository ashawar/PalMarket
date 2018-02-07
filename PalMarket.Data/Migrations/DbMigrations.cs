using PalMarket.Data.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using System.Data.Entity.Migrations.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PalMarket.Data.Migrations
{
    public class DbMigrations
    {
        public void InitializeDatabase()
        {
            //using (var context = new PalMarketEntities())
            //{
            //    if (!context.Database.Exists() || !context.Database.CompatibleWithModel(false))
            //    {
            //        var configuration = new Configuration();
            //        var migrator = new DbMigrator(configuration);
            //        migrator.Configuration.TargetDatabase = new DbConnectionInfo(context.Database.Connection.ConnectionString, "System.Data.SqlClient");
            //        var migrations = migrator.GetPendingMigrations();
            //        if (migrations.Any())
            //        {
            //            var scriptor = new MigratorScriptingDecorator(migrator);
            //            var script = scriptor.ScriptUpdate(null, migrations.Last());

            //            if (!string.IsNullOrEmpty(script))
            //            {
            //                context.Database.ExecuteSqlCommand(script);
            //            }
            //        }
            //    }
            //}
        }
    }
}
