using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MeshiRoulette.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MeshiRoulette
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                var connectionString = new SqliteConnectionStringBuilder()
                {
                    DataSource = this.Configuration["SqliteDataSource"]
                };

                options.UseSqlite(connectionString.ConnectionString);
            });

            services.AddIdentity<ApplicationUser, IdentityRole<long>>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddAuthentication()
                .AddTwitter(options =>
                {
                    // 環境変数から指定
                    options.ConsumerKey = this.Configuration["Authentication:Twitter:ConsumerKey"];
                    options.ConsumerSecret = this.Configuration["Authentication:Twitter:ConsumerSecret"];
                });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Hello World!");
            });
        }
    }
}
