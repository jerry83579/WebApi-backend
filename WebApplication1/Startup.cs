using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using Microsoft.OpenApi.Models;
using System.IO;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;
using Microsoft.Extensions.Options;

namespace WebApi
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

            //enble cors
            services.AddCors(
               options => options.AddPolicy("AllowCors",
               builder =>
               {
                   builder
                   .AllowAnyOrigin() 
                   //.WithMethods("GET", "PUT") //AllowSpecificMethods;
                   .AllowAnyMethod() //AllowAllMethods;
                   //.WithHeaders("Accept", "Content-type", "Origin", "X-Custom-Header");  //AllowSpecificHeaders;
                   .AllowAnyHeader(); //AllowAllHeaders;
               })
  
             );

            services.AddDbContext<RestaurantContext>(opt =>
            opt.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"), x => x.UseNetTopologySuite())
            );

            services.AddSwaggerGen();

            services.AddControllers().AddJsonOptions(options => {
                options.JsonSerializerOptions.Converters.Add(new NetTopologySuite.IO.Converters.GeoJsonConverterFactory());

            });

        
            services.AddSwaggerGen(option =>
            {
                option.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Web Api",
                    Description = "Web API",
                    TermsOfService = new Uri("https://example.com/terms"),
                    Contact = new OpenApiContact
                    {
                        Name = "Shayne Boyer",
                        Email = string.Empty,
                        Url = new Uri("https://twitter.com/spboyer"),
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Use under LICX",
                        Url = new Uri("https://example.com/license"),
                    }
                });

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                option.IncludeXmlComments(xmlPath);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {   
            if (env.IsDevelopment() || env.IsProduction())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/api/error");
            }

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.)
            app.UseSwaggerUI(option =>
            {
                if (env.IsDevelopment())
                {
                    string swaggerJsonBasePath = string.IsNullOrWhiteSpace(option.RoutePrefix) ? "." : "..";
                    option.SwaggerEndpoint($"{swaggerJsonBasePath}/swagger/v1/swagger.json", "My API");
                }
                else
                {
                    // To deploy on IIS
                    string swaggerJsonBasePath = string.IsNullOrWhiteSpace(option.RoutePrefix) ? "." : "..";
                    option.SwaggerEndpoint($"{swaggerJsonBasePath}/swagger/v1/swagger.json", "My API");
                }
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseCors("AllowCors");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
