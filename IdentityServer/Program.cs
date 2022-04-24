using Duende.IdentityServer.EntityFramework.DbContexts;
using Duende.IdentityServer.EntityFramework.Mappers;
using Duende.IdentityServer.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using System.Linq;

namespace IdentityServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = CreateHostBuilder(args).Build();


            // we configure in startup and we are writing code what we wrote in startup AddInMemoryClients without Db  before
            // We did in memory configuration we did in startup.cs before
            using (var scope = builder.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                using (var persistedGrantDbContext = scope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>())
                {
                    persistedGrantDbContext.Database.Migrate();
                }

                using (var configurationDbContext = scope.ServiceProvider.GetRequiredService<ConfigurationDbContext>())
                {
                    configurationDbContext.Database.Migrate();

                    if (!configurationDbContext.Clients.Any())
                    {
                        configurationDbContext.Clients.Add(new Client
                        {
                            ClientId = "console",
                            ClientSecrets = new List<Secret> { new("secret".Sha256()) },
                            AllowedGrantTypes = GrantTypes.ClientCredentials,
                            AllowedScopes = new List<string> { "api" }
                        }.ToEntity());

                        //CorsPolicyService should be allowed to origin
                        configurationDbContext.Clients.Add(new Client
                        {
                            ClientId = "swagger",
                            AllowedCorsOrigins = new List<string> {
                              "https://localhost:5003"
                        },
                        }.ToEntity());
                        configurationDbContext.SaveChanges();

                    }

                    if (!configurationDbContext.ApiScopes.Any())
                    {
                        configurationDbContext.ApiScopes.Add(new ApiScope("api").ToEntity());
                        configurationDbContext.SaveChanges();
                    }

                    if (!configurationDbContext.ApiResources.Any())
                    {
                        configurationDbContext.ApiResources.Add(new ApiResource("api")
                        {
                            Scopes = new List<string> { "api" }
                        }.ToEntity());
                        configurationDbContext.SaveChanges();
                    }


                }
            }
            builder.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}

// To make migrate
// dotnet ef migrations add InitialIdentityServerPersistedGrantDbMigration -c PersistedGrantDbContext -o Data/Migrations/IdentityServer/PersistedGrantDb
// dotnet ef migrations add InitialIdentityServerConfigurationDbMigration -c ConfigurationDbContext -o Data/Migrations/IdentityServer/ConfigurationDb
// dotnet ef database update -c PersistedGrantDbContext
// dotnet ef database update -c ConfigurationDbContext

// We are creating all tables like that

// When we build IdentityServer  it is going to create ClientId ClientSecret on program.cs


//We can delete keys folder because we are not keeping in memory anymore. We are keeping in database