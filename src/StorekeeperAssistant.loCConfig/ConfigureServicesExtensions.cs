using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;

namespace StorekeeperAssistant.loCConfig
{
    public static class ConfigureServicesExtensions
    {
        public static void AddCustomSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(setupAction =>
            {
                setupAction.SwaggerDoc(
                   name: "StorekeeperAssistant",
                   info: new OpenApiInfo()
                   {
                       Title = "StorekeeperAssistant API",
                       Version = "1.0",
                       Description = "API..",
                       Contact = new OpenApiContact()
                       {
                           Email = "wiji.ed@mail.ru",
                           Name = "Eddy Chervov",
                           Url = new Uri("https://github.com/eddchervov")
                       },
                       License = new OpenApiLicense()
                       {
                           Name = "MIT License",
                           Url = new Uri("https://opensource.org/licenses/MIT")
                       }
                   });

                setupAction.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new List<string>()
                    }
                });
            });
        }

        public static void UseCustomSwagger(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(setupAction =>
            {
                setupAction.SwaggerEndpoint(
                    url: "/swagger/StorekeeperAssistant/swagger.json",
                    name: "StorekeeperAssistant API");
                //setupAction.RoutePrefix = ""; --> To be able to access it from this URL: https://localhost:5001/swagger/index.html

                setupAction.DefaultModelExpandDepth(2);
                setupAction.DefaultModelRendering(Swashbuckle.AspNetCore.SwaggerUI.ModelRendering.Model);
                setupAction.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
                setupAction.EnableDeepLinking();
                setupAction.DisplayOperationId();
            });
        }
    }
}
