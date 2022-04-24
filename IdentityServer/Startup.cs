using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;

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
            void ConfigureDbContext(DbContextOptionsBuilder builder)
            {
                builder.UseSqlServer(Configuration.GetConnectionString("ConnectionStrings"),
                    optionsBuilder =>
                    {
                        optionsBuilder.MigrationsAssembly(typeof(Startup).GetTypeInfo().Assembly.GetName().Name);
                    });
            }

            services.AddIdentityServer()
                .AddConfigurationStore(options => { options.ConfigureDbContext = ConfigureDbContext; })
                .AddOperationalStore(options => { options.ConfigureDbContext = ConfigureDbContext; });

            //.AddInMemoryClients(new List<Client> {

            //    new Client{
            //         ClientId="console",
            //         ClientSecrets=new List<Secret>
            //         {
            //             new Secret("secret".Sha256())
            //         },
            //         AllowedGrantTypes = GrantTypes.ClientCredentials,
            //         AllowedScopes=new List<string>
            //         {
            //             "api"
            //         }
            //     }
            //})
            //.AddInMemoryApiScopes(new List<ApiScope>
            //{
            //    new ApiScope(name:"api") // We are specifing api
            //})
            //.AddInMemoryApiResources(new List<ApiResource>
            //{
            //    //Here We can specify api resources and change the name
            //    //Definetelly it should be matches with audience name in Api

            //    new ApiResource(name:"api"){
            //        Scopes=new List<string>
            //        {
            //            "api"
            //        }
            //    }
            //});
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

            app.UseIdentityServer();
        }
    }
}
