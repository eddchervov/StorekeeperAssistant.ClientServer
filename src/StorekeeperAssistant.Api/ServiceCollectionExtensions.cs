using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StorekeeperAssistant.Api.Services;
using StorekeeperAssistant.Api.Services.Implementation;
using StorekeeperAssistant.Api.Utils;
using StorekeeperAssistant.Api.Utils.Implementation;
using System;

namespace StorekeeperAssistant.Api
{
    public static class ServiceCollectionExtensions
    {
        public static void AddCustomApiServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ICacheProvider, CacheProvider>();
            services.AddScoped<IMovingRemoteCallService, MovingRemoteCallService>();
            services.AddScoped<IWarehouseRemoteCallService, WarehouseRemoteCallService>();
            services.AddScoped<IWarehouseInventoryItemRemoteCallService, WarehouseInventoryItemRemoteCallService>();
            services.AddScoped<INomenclatureRemoteCallService, NomenclatureRemoteCallService>();
        }
    }
}
