using System;
using System.Text.Json.Serialization;
using BusinessLibrary.Interfaces;
using BusinessLibrary.Repositories;
using DataAccessLibrary.Database;
using DataAccessLibrary.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace IdentityWithReact
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ReactDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<IPersonRepository, PersonRepository>();

            services.AddIdentity<AppUser, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 8; // Changes Minimum-length from default value of 6 to 8.

                options.Lockout.AllowedForNewUsers = true; // Default
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5); // Default
                options.Lockout.MaxFailedAccessAttempts = 5; // Default

                options.User.AllowedUserNameCharacters += "Â‰ˆ≈ƒ÷"; // Adds scandinavian letters
                options.User.RequireUniqueEmail = true; // Declares only unique emails are allowed in application
            })
                .AddEntityFrameworkStores<ReactDbContext>()
                .AddDefaultTokenProviders();

            services.AddIdentityServer()
                .AddApiAuthorization<AppUser, ReactDbContext>();

            services.AddAuthentication()
                .AddIdentityServerJwt();

            // Makes it possible for frameworks to add this into their dependencies.
            services.AddDistributedMemoryCache();
            // Creates a session on the application that lasts for 20 minutes.
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(20);
            });

            // Cookie configuration.
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            // Further cookie configurations.
            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
                options.Cookie.SameSite = SameSiteMode.None;

                options.ExpireTimeSpan = TimeSpan.FromMinutes(5);

                options.LoginPath = "/Account/Login";               // Unsure if accurate
                options.AccessDeniedPath = "/Account/AccessDenied"; // Unsure if accurate
                options.SlidingExpiration = true;
            });

            // Hashes the password 200_000 times for security.
            services.Configure<PasswordHasherOptions>(options =>
            {
                options.IterationCount = 200_000;
            });

            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                { // Only for developer mode.
                    builder.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .Build();

                    builder.WithOrigins("http://localhost:3000")
                    .AllowCredentials()
                    .Build();
                });
            });
            // This adds the possibility to convert enum values to their respective string values.
            services.AddMvc().AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                    options.JsonSerializerOptions.IgnoreNullValues = true;
                }).SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");

                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseStaticFiles();
            //app.UseSpaStaticFiles();

            app.UseRouting();

            app.UseCors();

            app.UseAuthentication();
            app.UseIdentityServer();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{Action=Index}/{id?}");

                endpoints.MapControllerRoute(
                    name: "PersonApi",
                    pattern: "api/{controller=PersonApi}");

                //endpoints.MapGet("/", async context =>
                //{
                //    await context.Response.WriteAsync("Hello World!");
                //});
            });
        }
    }
}