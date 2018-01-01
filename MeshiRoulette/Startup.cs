using System;
using System.IO;
using System.Security.Claims;
using MeshiRoulette.Data;
using MeshiRoulette.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;
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

            if (this.Configuration["DataProtectionRepository"] != null)
            {
                services.AddDataProtection()
                    .PersistKeysToFileSystem(new DirectoryInfo(this.Configuration["DataProtectionRepository"]));
            }

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddAuthentication()
                .AddTwitter(options =>
                {
                    // 環境変数から指定
                    options.ConsumerKey = this.Configuration["Authentication:Twitter:ConsumerKey"]
                        ?? throw new Exception("Authentication:Twitter:ConsumerKey が設定されていません。");
                    options.ConsumerSecret = this.Configuration["Authentication:Twitter:ConsumerSecret"]
                        ?? throw new Exception("Authentication:Twitter:ConsumerSecret が設定されていません。");

                    options.SaveTokens = true;

                    // 認証後にプロフィール画像を取得する
                    options.RetrieveUserDetails = true;
                    options.ClaimActions.MapJsonKey("urn:twitter:profileimage", "profile_image_url_https", ClaimTypes.Uri);
                });

            services.Configure<RouteOptions>(options => options.LowercaseUrls = true);

            services.AddMvc();

            services.AddScoped(typeof(IPlaceCollectionAuthorization), typeof(PlaceCollectionAuthorization));
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ApplicationDbContext dbContext)
        {
            if (this.Configuration["AutoMigration"] == "true")
            {
                dbContext.Database.Migrate();
            }

            app.UseForwardedHeaders();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }

            app.UseStaticFiles();
            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                // PlaceCollection は内部用の名前なので roulettes に書き換える
                routes.MapRoute("PlaceCollections", "roulettes/{action=Index}/{id?}", new { controller = "PlaceCollections" });

                routes.MapRoute("default", "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
