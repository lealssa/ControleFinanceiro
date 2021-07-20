using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using ControleFinanceiro.Models;
using ControleFinanceiro.Services;
using Microsoft.Extensions.Options;
using System.Reflection;
using System.IO;
using Hellang.Middleware.ProblemDetails;

namespace ControleFinanceiro
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
            services.Configure<ControleFinanceiroDatabaseSettings>(
                Configuration.GetSection(nameof(ControleFinanceiroDatabaseSettings))
            );

            services.AddSingleton<IControleFinanceiroDatabaseSettings>(
                sp => sp.GetRequiredService<IOptions<ControleFinanceiroDatabaseSettings>>().Value
            );

            services.AddSingleton<UsuarioService>();

            services.AddControllers();
            services.AddProblemDetails();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ControleFinanceiro", Version = "v1" });
                
                // Documentação à partir do XML
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => 
                { 
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "ControleFinanceiro v1");
                    c.RoutePrefix = "docs";
                });
            }

            // Retorna as exceptions em ProblemDetail
            // https://andrewlock.net/handling-web-api-exceptions-with-problemdetails-middleware/
            app.UseProblemDetails();
            
            app.UsePathBase("/api/v1");

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
