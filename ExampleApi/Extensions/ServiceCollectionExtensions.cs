using ExampleApi.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using OpenIddict.Validation.AspNetCore;

namespace ExampleApi.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public const string CORS_POLICY = "CorsPolicy";

        public static IServiceCollection AddCorsConfig(this IServiceCollection services)
        {
            return services.AddCors(options =>
                   {
                       options.AddPolicy(name: CORS_POLICY,
                                           builder =>
                                           {
                                               builder.AllowCredentials();
                                               builder.WithOrigins("https://localhost:4200"); // TODO: move to appsettings
                                               builder.SetIsOriginAllowedToAllowWildcardSubdomains();
                                               builder.AllowAnyHeader();
                                               builder.AllowAnyMethod();
                                           });
                   });
        }

        // for OpenIdServer (based in velusia sample)
        public static IServiceCollection AddOpenIdDictAuthenticationSchemaConfig(this IServiceCollection services)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
            });

            services.AddOpenIddict()
                    .AddValidation(options =>
                    {
                        options.SetIssuer("https://localhost:5001"); // TODO: move all the configuration strings to appsettings
                        options.AddAudiences("forecastApi");

                        options.UseIntrospection()
                                .SetClientId("forecastApi")
                                .SetClientSecret("forecastApiSecret"); // TODO: move to a secret vault

                        options.UseSystemNetHttp();
                        options.UseAspNetCore();
                    });

            return services;
        }

        // for orchard core openid
        public static IServiceCollection AddJWTBearerAuthenticationSchemaConfig(this IServiceCollection services)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.Authority = "https://localhost:5001";
                options.Audience = "forecastApi";
                options.RequireHttpsMetadata = true;
                options.SaveToken = true;
                options.IncludeErrorDetails = true;
            });

            return services;
        }

        public static IServiceCollection AddAuthorizationConfig(this IServiceCollection services)
        {
            services.AddSingleton<IAuthorizationHandler, RequireScopeHandler>();

            services.AddAuthorization(options =>
            {
                options.AddPolicy("forecastPolicy", policy =>
                 {
                     policy.Requirements.Add(new RequireScope());
                 });
            });

            return services;
        }

        public static IServiceCollection AddSwaggerConfig(this IServiceCollection services)
        {
            return services.AddSwaggerGen(c =>
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

    }
}
