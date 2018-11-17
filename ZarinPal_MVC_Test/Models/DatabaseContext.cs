using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web;
using ZarinPal_MVC_Test.Models;

namespace ZarinPal_MVC_Test.Models
{
    public class DatabaseContext : System.Data.Entity.DbContext
    {
        #region CTOR

        static DatabaseContext()
        {
            //System.Data.Entity.Database.SetInitializer(
            //    new System.Data.Entity.MigrateDatabaseToLatestVersion<DatabaseContext, Migrations.Configuration>());

            System.Data.Entity.Database.SetInitializer(
                new System.Data.Entity.CreateDatabaseIfNotExists<DatabaseContext>());

        }
        public DatabaseContext() : base("ZarinPal_DB")
        {

        }

        #endregion

        #region Tables

        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }
        #endregion
    }
}