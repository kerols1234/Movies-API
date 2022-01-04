using Movies_API.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Movies_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Movies_API
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
            var server = Configuration["DBServer"] ?? "localhost";
            var port = Configuration["DBPort"] ?? "1443";
            var user = Configuration["DBUser"] ?? "SA";
            var password = Configuration["DBPassord"] ?? "yourStrong(!)Password";
            var database = Configuration["Database"] ?? "Movies";
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    );
            });
            services.AddDbContext<AppDbContext>
                (options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            //app.UseHttpsRedirection();

            app.UseRouting();
            UpdateDatabase(app);

            app.UseCors(options => options.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private void UpdateDatabase(IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
            using var context = serviceScope.ServiceProvider.GetService<AppDbContext>();
            try
            {
                context.Database.Migrate();
                var moviesList = new List<Movies>()
                {
                    new Movies() { Name = "iron-man", Release = "2008", Duration = 2.6 },
                    new Movies() { Name = "spider-man", Release = "2002", Duration = 2.1 },
                    new Movies() { Name = "super-man", Release = "1978", Duration = 2.23 },
                    new Movies() { Name = "thor", Release = "2011", Duration = 1.55 },
                    new Movies() { Name = "hulk", Release = "2003", Duration = 2.18 },
                    new Movies() { Name = "fantastic four", Release = "2005", Duration = 1.46 }
                };
                foreach (Movies movies in moviesList)
                {
                    if (!context.Movies.Any(obj => obj.Name == movies.Name))
                    {
                        context.Movies.Add(movies);
                    }
                }
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                _ = ex.Message;
            }
        }
    }
}
