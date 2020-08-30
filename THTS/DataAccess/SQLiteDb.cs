using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using THTS.DataAccess.Entity;

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
        public DbSet<PositionAndSensor> PositionAndSensors { get; set; }
        public DbSet<TemperatureTolerance> TemperatureTolerances { get; set; }
        public DbSet<Device> Devices { get; set; }
        public DbSet<DebugTiredTest> DebugTiredTests { get; set; }
        public DbSet<Setting> Settingses { get; set; }
        public DbSet<TestData> TestDatas { get; set; }
    }
}
