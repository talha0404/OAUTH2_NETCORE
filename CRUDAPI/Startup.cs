using CRUDAPI.EFCORE;
using CRUDAPI.Services.Services.CustomerServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace CRUDAPI
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

            services.AddControllers();

            services.AddDbContext<CrudApiDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("ConnectionStrings")));

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "CRUDAPI", Version = "v1" });
            });

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                   .AddJwtBearer(options =>
                   {
                       options.Authority = Configuration["Authentication:Authority"];
                       options.Audience = "api";
                       //it should be match in IdentityServer AddInMemoryApiResources when add IdentityServer. We are registering api to IdentityServer


                       //options.TokenValidationParameters = new TokenValidationParameters
                       //{
                       //    ValidateAudience = false  // without registering api to access data
                       //};
                   }
             );

            //it is supposed to match with IdentityServer startup
            //we are configuring Clients in IdentityServer and build start before starting Api to access Api functions 
            services.AddAuthorization(options =>
            {
                //We can add more than one policy
                options.AddPolicy("ApiScope", builder =>
                 {
                     builder.RequireAuthenticatedUser();
                     builder.RequireClaim("scope", "api");
                 });
            });

            #region DependencyInjection
            services.AddTransient<ICustomerService, CustomerService>();
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "CRUDAPI v1"));
            }

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
