using ExampleApi.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using OpenIddict.Validation.AspNetCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExampleApi
{
    public class Startup
    {
        public const string CORS_POLICY = "CorsPolicy";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
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

            services.AddAuthentication(options => {
                options.DefaultScheme = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = "Bearer";
                options.DefaultChallengeScheme = "Bearer";
            });
            services.AddOpenIddict()
                .AddValidation(options =>
                {
                    options.SetIssuer("https://localhost:5001");
                    options.AddAudiences("forecastApi");

                    options.UseIntrospection()
                           .SetClientId("forecastApi")
                           .SetClientSecret("forecastApiSecret");

                    options.UseSystemNetHttp();
                    options.UseAspNetCore();
                });

            //services.AddSingleton<IAuthorizationHandler, RequireScopeHandler>();

            //services.AddAuthorization(options =>
            //{
            //    //options.AddPolicy("forecastPolicy", policy =>
            //    // {
            //    //     policy.Requirements.Add(new RequireScope());
            //    // });
            //});

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ForecastApi", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ForecastApi v1"));
            }

            app.UseCors(CORS_POLICY);

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
