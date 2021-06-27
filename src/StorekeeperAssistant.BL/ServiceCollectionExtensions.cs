using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StorekeeperAssistant.BL.Services.InventoryItems;
using StorekeeperAssistant.BL.Services.InventoryItems.Implementation;
using StorekeeperAssistant.BL.Services.Movings;
using StorekeeperAssistant.BL.Services.Movings.Implementation;
using StorekeeperAssistant.BL.Services.WarehouseInventoryItems;
using StorekeeperAssistant.BL.Services.WarehouseInventoryItems.Implementation;
using StorekeeperAssistant.BL.Services.Warehouses;
using StorekeeperAssistant.BL.Services.Warehouses.Implementation;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("StorekeeperAssistant.BL.Tests")]
namespace StorekeeperAssistant.BL
{
    public static class ServiceCollectionExtensions
    {
        public static void AddCustomBLServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IMovingService, MovingService>();
            services.AddScoped<IGetterMovingService, GetterMovingService>();
            services.AddScoped<ICreatorMovingService, CreatorMovingService>();
            services.AddScoped<IValidationMovingService, ValidationMovingService>();
            services.AddScoped<IWarehouseService, WarehouseService>();
            services.AddScoped<IWarehouseInventoryItemService, WarehouseInventoryItemService>();
            services.AddScoped<IValidationWarehouseInventoryItemService, ValidationWarehouseInventoryItemService>();
            services.AddScoped<IInventoryItemService, InventoryItemService>();
        }
    }
}
