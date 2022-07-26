using API.Data.Repository;
using Customer.Providers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Customer
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
            services.AddDbContext<AdventureWorks2019DbContext>(x => x.UseSqlServer(Configuration.GetConnectionString("AdventureWorks")));
            services.AddScoped<ICustomerProvider, CustomerProvider>();
            services.AddControllers();
            services.AddSwaggerGen();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {


            //if (env.IsDevelopment())
            //{
            app.UseDeveloperExceptionPage();
            //}
            app.UseHttpsRedirection();
            app.UseCors(corsOptions =>
            {
                corsOptions.AllowAnyOrigin();
                corsOptions.AllowAnyMethod();
            });
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Adventure Works API");
                //c.RoutePrefix = string.Empty;
            });
        }
    }
}
