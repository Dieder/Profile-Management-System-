using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Steeltoe.Management.Endpoint;
using Steeltoe.Management.Endpoint.Health;
using Steeltoe.Management.Endpoint.Info;
using Steeltoe.Management.Tracing;

namespace ProfileService.Api
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
            services.AddSwaggerGen(c =>
                   c.SwaggerDoc("v1", new OpenApiInfo
                   {
                       Version = "v1",
                       Title = "Profile Managment Gateway API",
                       Description = "Contains profile info of persons"
                   })
            );
            services.AddHealthActuator(Configuration);
            services.AddInfoActuator(Configuration);
            services.AddDistributedTracing(Configuration);
      //      services.AddOcelot();
        //    services.AddSwaggerForOcelot(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();
            

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

       //     app.UseOcelot();
        //    app.UseSwaggerForOcelotUI(Configuration);
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                const string apiDescriptiveName = "Translation service";
                string swaggerJsonBasePath = string.IsNullOrWhiteSpace(c.RoutePrefix) ? "." : " ..";
                c.SwaggerEndpoint($"/swagger/v1/swagger.json", apiDescriptiveName);
            });
        }
    }
}
