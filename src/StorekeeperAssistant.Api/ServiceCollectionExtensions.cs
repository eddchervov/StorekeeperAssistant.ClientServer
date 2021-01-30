using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
        }
    }
}
