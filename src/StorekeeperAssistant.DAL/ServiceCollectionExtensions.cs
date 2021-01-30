using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StorekeeperAssistant.DAL.DBContext;
using StorekeeperAssistant.DAL.DBContext.Implementation;
using StorekeeperAssistant.DAL.Repositories;
using StorekeeperAssistant.DAL.Repositories.Implementation;
using System;
using System.Collections.Generic;
using System.Text;

namespace StorekeeperAssistant.DAL
{
    public static class ServiceCollectionExtensions
    {
        public static void AddCustomDbContext(this IServiceCollection services, Func<string> GetConnectionString)
        {
            services.AddDbContext<AppDBContext>(options =>
            {
                options.UseSqlServer(GetConnectionString());
                options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            });

            services.AddScoped<IAppDBContext, AppDBContext>();
        }

        public static void AddCustomDALServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IMovingRepository, MovingRepository>();
            services.AddScoped<IWarehouseRepository, WarehouseRepository>();
            services.AddScoped<IMovingInventoryItemsRepository, MovingInventoryItemsRepository>();
        }
    }
}
