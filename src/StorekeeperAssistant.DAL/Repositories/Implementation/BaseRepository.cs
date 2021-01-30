using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StorekeeperAssistant.DAL.Repositories.Implementation
{
    internal class BaseRepository<TEntity> : IBaseRepository<TEntity>
             where TEntity : class
    {
        protected DbContext DbContext { get; private set; }

        protected DbSet<TEntity> DbSet { get; private set; }

        public BaseRepository(DbSet<TEntity> dbSet)
        {
            DbSet = dbSet;
            DbContext = GetDbContext(dbSet);
        }

        public virtual void Insert(TEntity entity)
        {
            DbSet.Add(entity);
            DbContext.SaveChanges();
            DbContext.Entry(entity).State = EntityState.Detached;
        }

        public virtual async Task InsertAsync(TEntity entity)
        {
            DbSet.Add(entity);
            await DbContext.SaveChangesAsync();
            DbContext.Entry(entity).State = EntityState.Detached;
        }

        public virtual void Update(TEntity entity)
        {
            DbContext.Entry(entity).State = EntityState.Modified;
            DbContext.SaveChanges();
            DbContext.Entry(entity).State = EntityState.Detached;
        }

        public virtual async Task UpdateAsync(TEntity entity)
        {
            DbContext.Entry(entity).State = EntityState.Modified;
            await DbContext.SaveChangesAsync();
            DbContext.Entry(entity).State = EntityState.Detached;
        }

        public virtual void Delete(TEntity entity)
        {
            DbSet.Remove(entity);
            DbContext.SaveChanges();
            DbContext.Entry(entity).State = EntityState.Detached;
        }

        public virtual async Task DeleteAsync(TEntity entity)
        {
            DbSet.Remove(entity);
            await DbContext.SaveChangesAsync();
            DbContext.Entry(entity).State = EntityState.Detached;
        }

        private static DbContext GetDbContext<T>(DbSet<T> dbSet) where T : class
        {
            var infrastructure = dbSet as IInfrastructure<IServiceProvider>;
            var serviceProvider = infrastructure.Instance;
            var currentDbContext = serviceProvider.GetService(typeof(ICurrentDbContext))
                                       as ICurrentDbContext;
            return currentDbContext.Context;
        }
    }
}
