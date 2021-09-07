using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenIddict.Abstractions;
using OpenIdServer.Data;
using System;
using System.Threading;
using System.Threading.Tasks;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace OpenIdServer
{
    public class Worker : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;

        public Worker(IServiceProvider serviceProvider)
            => _serviceProvider = serviceProvider;

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = _serviceProvider.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            await context.Database.EnsureCreatedAsync(cancellationToken);

            await RegisterApplicationsAsync(scope.ServiceProvider);
            await RegisterScopesAsync(scope.ServiceProvider);

            static async Task RegisterApplicationsAsync(IServiceProvider serviceProvider)
            {
                var manager = serviceProvider.GetRequiredService<IOpenIddictApplicationManager>();

                // Angular client
                if (await manager.FindByClientIdAsync("angularClient") is null)
                {
                    await manager.CreateAsync(new OpenIddictApplicationDescriptor
                    {
                        ClientId = "angularClient",
                        ConsentType = ConsentTypes.Explicit,
                        DisplayName = "AngularClient",
                        PostLogoutRedirectUris =
                        {
                            new Uri("https://localhost:4200"),
                            new Uri("https://localhost:4200/index.html"),
                            new Uri("https://localhost:4200/silent-refresh.html")
                        },
                        RedirectUris =
                        {
                            new Uri("https://localhost:4200"),
                            new Uri("https://localhost:4200/index.html"),
                            new Uri("https://localhost:4200/silent-refresh.html")
                        },
                        Permissions =
                        {
                            Permissions.Endpoints.Authorization,
                            Permissions.Endpoints.Logout,
                            Permissions.Endpoints.Token,
                            Permissions.Endpoints.Revocation,
                            Permissions.GrantTypes.AuthorizationCode,
                            Permissions.GrantTypes.RefreshToken,
                            Permissions.ResponseTypes.Code,
                            Permissions.Scopes.Email,
                            Permissions.Scopes.Profile,
                            Permissions.Scopes.Roles,
                            Permissions.Prefixes.Scope + "forecast"
                        },
                        Requirements =
                        {
                            Requirements.Features.ProofKeyForCodeExchange
                        }
                    }); ;
                }

                // API
                if (await manager.FindByClientIdAsync("forecastApi") == null)
                {
                    await manager.CreateAsync(new OpenIddictApplicationDescriptor
                    {
                        ClientId = "forecastApi",
                        ClientSecret = "forecastApiSecret",
                        Permissions =
                        {
                            Permissions.Endpoints.Introspection
                        }
                    });

                }
            }

            static async Task RegisterScopesAsync(IServiceProvider serviceProvider)
            {
                var manager = serviceProvider.GetRequiredService<IOpenIddictScopeManager>();

                if (await manager.FindByNameAsync("forecast") is null)
                {
                    await manager.CreateAsync(new OpenIddictScopeDescriptor
                    {
                        DisplayName = "forecast API access",
                        Name = "forecast",
                        Resources =
                        {
                            "forecastApi"
                        }
                    });
                }
            }
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}