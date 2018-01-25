using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace THTS.DataAccess
{
    public static class DbContextExtensions
    {
        public static void Update<TEntity>(this DbContext dbContext, object entityID, TEntity entity) where TEntity : class
        {
            if (dbContext == null) throw new ArgumentNullException("dbContext");
            if (entity == null) throw new ArgumentNullException("entity");

            DbSet<TEntity> dbSet = dbContext.Set<TEntity>();
            try
            {
                DbEntityEntry<TEntity> entry = dbContext.Entry(entity);
                if (entry.State == EntityState.Detached)
                {
                    dbSet.Attach(entity);
                    entry.State = EntityState.Modified;
                }
            }
            catch (InvalidOperationException)
            {
                TEntity oldEntity = dbSet.Find(entityID);
                dbContext.Entry(oldEntity).CurrentValues.SetValues(entity);
            }
        }

        public static void Update<TEntity>(this DbContext dbContext, object entityID1,object entityID2, TEntity entity) where TEntity : class
        {
            if (dbContext == null) throw new ArgumentNullException("dbContext");
            if (entity == null) throw new ArgumentNullException("entity");

            DbSet<TEntity> dbSet = dbContext.Set<TEntity>();
            try
            {
                DbEntityEntry<TEntity> entry = dbContext.Entry(entity);
                if (entry.State == EntityState.Detached)
                {
                    dbSet.Attach(entity);
                    entry.State = EntityState.Modified;
                }
            }
            catch (InvalidOperationException)
            {
                TEntity oldEntity = dbSet.Find(entityID1, entityID2);
                dbContext.Entry(oldEntity).CurrentValues.SetValues(entity);
            }
        }


        public static int SaveChanges(this DbContext dbContext, bool validateOnSaveEnabled)
        {
            bool isReturn = dbContext.Configuration.ValidateOnSaveEnabled != validateOnSaveEnabled;
            try
            {
                dbContext.Configuration.ValidateOnSaveEnabled = validateOnSaveEnabled;
                return dbContext.SaveChanges();
            }
            finally
            {
                if (isReturn)
                {
                    dbContext.Configuration.ValidateOnSaveEnabled = !validateOnSaveEnabled;
                }
            }
        }
    }
}
