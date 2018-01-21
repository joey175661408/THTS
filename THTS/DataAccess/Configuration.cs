using System.Data.Entity.Migrations;
using System.Data.SQLite.EF6.Migrations;

namespace THTS.DataAccess
{
    public class Configuration : DbMigrationsConfiguration<SQLiteDB>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
            SetSqlGenerator("System.Data.SQLite", new SQLiteMigrationSqlGenerator());
        }

        protected override void Seed(SQLiteDB context)
        {

        }
    }
}
