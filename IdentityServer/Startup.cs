using Duende.IdentityServer.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;

namespace IdentityServer
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            // we created to use identity server as client ClientId and Secret. That is creating AccessToken
            //At First we are building Identity Server before starting api
            services.AddIdentityServer()
                .AddInMemoryClients(new List<Client> {

                    new Client{
                         ClientId="console",
                         ClientSecrets=new List<Secret>
                         {
                             new Secret("secret".Sha256())
                         },
                         AllowedGrantTypes = GrantTypes.ClientCredentials,
                         AllowedScopes=new List<string>
                         {
                             "api"
                         }
                     }
                })
                .AddInMemoryApiScopes(new List<ApiScope>
                {
                    new ApiScope(name:"api") // We are specifing api
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

            app.UseIdentityServer();
        }
    }
}
