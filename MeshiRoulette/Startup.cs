using System.Security.Claims;
using MeshiRoulette.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
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

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddAuthentication()
                .AddTwitter(options =>
                {
                    // 環境変数から指定
                    options.ConsumerKey = this.Configuration["Authentication:Twitter:ConsumerKey"];
                    options.ConsumerSecret = this.Configuration["Authentication:Twitter:ConsumerSecret"];

                    options.SaveTokens = true;

                    // 認証後にプロフィール画像を取得する
                    options.RetrieveUserDetails = true;
                    options.ClaimActions.MapJsonKey("urn:twitter:profileimage", "profile_image_url_https", ClaimTypes.Uri);
                });

            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }

            app.UseStaticFiles();
            app.UseAuthentication();

            app.UseMvcWithDefaultRoute();
        }
    }
}
