using ExampleApi.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
//using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using OpenIddict.Validation.AspNetCore;

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
                                      builder.AllowCredentials();
                                      builder.WithOrigins("https://localhost:4200");
                                      builder.SetIsOriginAllowedToAllowWildcardSubdomains();
                                      builder.AllowAnyHeader();
                                      builder.AllowAnyMethod();
                                  });
            });

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme; //for belusia server
                //options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme; // for orchard core openid
            });
            //for orchard core openid
            //  .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            //{
            //    options.Authority = "https://localhost:5001";
            //    options.Audience = "forecastApi";
            //    options.RequireHttpsMetadata = true;
            //    options.SaveToken = true;
            //    options.IncludeErrorDetails = true;
            //});

            // for OpenIdServer
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

            services.AddSingleton<IAuthorizationHandler, RequireScopeHandler>();

            services.AddAuthorization(options =>
            {
                options.AddPolicy("forecastPolicy", policy =>
                 {
                     policy.Requirements.Add(new RequireScope());
                 });
            });

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                // add JWT Authentication
                var securityScheme = new OpenApiSecurityScheme
                {
                    Name = "JWT Authentication",
                    Description = "Enter JWT Bearer token **_only_**",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer", // must be lower case
                    BearerFormat = "JWT",
                    Reference = new OpenApiReference
                    {
                        Id = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme,
                        Type = ReferenceType.SecurityScheme
                    }
                };
                c.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {securityScheme, new string[] { }}
                });
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
