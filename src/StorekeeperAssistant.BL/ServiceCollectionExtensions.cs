using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StorekeeperAssistant.BL.Services;
using StorekeeperAssistant.BL.Services.Implementation;

namespace StorekeeperAssistant.BL
{
    public static class ServiceCollectionExtensions
    {
        public static void AddCustomBLServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IMovingService, MovingService>();
            services.AddScoped<IMovingGetService, MovingGetService>();
            services.AddScoped<IMovingCreationService, MovingCreationService>();
            services.AddScoped<IValidationMovingService, ValidationMovingService>();
            services.AddScoped<IWarehouseService, WarehouseService>();
            services.AddScoped<IWarehouseInventoryItemService, WarehouseInventoryItemService>();
            services.AddScoped<IValidationWarehouseInventoryItemService, ValidationWarehouseInventoryItemService>();
            services.AddScoped<INomenclatureService, NomenclatureService>();
        }
    }
}
