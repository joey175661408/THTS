﻿using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace THTS.DataAccess
{
    public class SQLiteDB : DbContext
    {
        public DbSet<User> Users { get; set; }

        public SQLiteDB() : base("DefaultConnection")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<SQLiteDB, Configuration>(true));
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Configurations.AddFromAssembly(typeof(SQLiteDB).Assembly);
        }

       
    }
}
