using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;

namespace StorekeeperAssistant.DAL.DBContext
{
    public interface IBaseDbContext
    {
        IDbContextTransaction BeginTransaction();

        int SaveChanges();

        EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
    }
}
