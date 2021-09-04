using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace OrchardOpenId
{
    public class Startup
    {
        public const string CORS_POLICY = "CorsPolicy";

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(name: CORS_POLICY,
                                  builder =>
                                  {
                                      builder.WithOrigins("https://localhost:4200");
                                      builder.AllowAnyMethod();
                                      builder.AllowAnyHeader();
                                  });
            });


            services.AddOrchardCms().ConfigureServices(services =>
            {
                //services.AddOpenIddict()
                //     .AddServer(options =>
                //     {
                //         options.SetIntrospectionEndpointUris("/connect/introspect");
                //         options.RequireProofKeyForCodeExchange();
                //     })
                //     .AddValidation(options=> {
                //         options.AddAudiences("forecastApi");
                //         options.UseLocalServer();
                //         options.UseAspNetCore();
                //     });

                //services.Configure<IdentityOptions>(options =>
                //{
                //    options.ClaimsIdentity.UserNameClaimType = Claims.Name;
                //    options.ClaimsIdentity.UserIdClaimType = Claims.Subject;
                //    options.ClaimsIdentity.RoleClaimType = Claims.Role;
                //});

            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseCors(CORS_POLICY);

            app.UseOrchardCore();
        }
    }
}
