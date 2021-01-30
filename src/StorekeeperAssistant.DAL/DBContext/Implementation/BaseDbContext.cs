using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Text;

namespace StorekeeperAssistant.DAL.DBContext.Implementation
{
    public abstract class BaseDbContext : DbContext, IBaseDbContext
    {
        public BaseDbContext(DbContextOptions options)
            : base(options)
        {

        }

        public IDbContextTransaction BeginTransaction()
        {
            return Database.BeginTransaction();
        }
    }
}
