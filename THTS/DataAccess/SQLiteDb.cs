using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace THTS.DataAccess
{
    public class SQLiteDB : DbContext
    {
        public SQLiteDB() : base("DefaultConnection")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<SQLiteDB, Configuration>(true));
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Configurations.AddFromAssembly(typeof(SQLiteDB).Assembly);
        }

        public DbSet<User> Users { get; set; }
        public DbSet<TestInfo> TestInfos { get; set; }
        public DbSet<TemperatureTolerance> TemperatureTolerances { get; set; }
        public DbSet<Device> Devices { get; set; }
        public DbSet<DebugTiredTest> DebugTiredTests { get; set; }
        public DbSet<Settings> SettingsSet { get; set; }
    }
}
