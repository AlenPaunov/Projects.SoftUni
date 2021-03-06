﻿using CloudinaryDotNet;

namespace ProjectsSoftuni.Web
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.UI;
    using Microsoft.AspNetCore.Identity.UI.Services;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using ProjectsSoftuni.Data;
    using ProjectsSoftuni.Data.Common;
    using ProjectsSoftuni.Data.Common.Repositories;
    using ProjectsSoftuni.Data.Models;
    using ProjectsSoftuni.Data.Repositories;
    using ProjectsSoftuni.Data.Seeding;
    using ProjectsSoftuni.Services;
    using ProjectsSoftuni.Services.Contracts;
    using ProjectsSoftuni.Services.Data;
    using ProjectsSoftuni.Services.Mapping;
    using ProjectsSoftuni.Services.Messaging;
    using ProjectsSoftuni.Services.Models.Projects;
    using ProjectsSoftuni.Web.ViewModels;
    using System.Reflection;

    public class Startup
    {
        private readonly IConfiguration configuration;

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Framework services
            // TODO: Add pooling when this bug is fixed: https://github.com/aspnet/EntityFrameworkCore/issues/9741
            services.AddDbContext<ProjectsSoftuniDbContext>(
                options => options
                .UseLazyLoadingProxies()
                .UseSqlServer(this.configuration.GetConnectionString("DefaultConnection")));

            services
                .AddIdentity<ProjectsSoftuniUser, ProjectsSoftuniRole>(options =>
                {
                    options.Password.RequireDigit = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequiredLength = 6;
                })
                .AddEntityFrameworkStores<ProjectsSoftuniDbContext>()
                .AddUserStore<ProjectsSoftuniUserStore>()
                .AddRoleStore<ProjectsSoftuniRoleStore>()
                .AddDefaultTokenProviders()
                .AddDefaultUI(UIFramework.Bootstrap4);

            services
                .AddMvc(options => { options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute()); })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddRazorPagesOptions(options =>
                {
                    options.AllowAreas = true;
                    options.Conventions.AuthorizeAreaFolder("Identity", "/Account/Manage");
                    options.Conventions.AuthorizeAreaPage("Identity", "/Account/Logout");
                });

            services
                .ConfigureApplicationCookie(options =>
                {
                    options.LoginPath = "/Identity/Account/Login";
                    options.LogoutPath = "/Identity/Account/Logout";
                    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
                });

            services
                .Configure<CookiePolicyOptions>(options =>
                {
                    // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                    options.CheckConsentNeeded = context => true;
                    options.MinimumSameSitePolicy = SameSiteMode.Lax;
                    options.ConsentCookie.Name = ".AspNetCore.ConsentCookie";
                });

            services.AddSingleton(this.configuration);

            // Identity stores
            services.AddTransient<IUserStore<ProjectsSoftuniUser>, ProjectsSoftuniUserStore>();
            services.AddTransient<IRoleStore<ProjectsSoftuniRole>, ProjectsSoftuniRoleStore>();

            // Data repositories
            services.AddScoped(typeof(IDeletableEntityRepository<>), typeof(EfDeletableEntityRepository<>));
            services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
            services.AddScoped<IDbQueryRunner, DbQueryRunner>();

            // Application services
            var apiKey = this.configuration["SendGridKey"];
            var email = this.configuration["FromAddress"];
            var name = this.configuration["FromName"];

            services.AddTransient<IEmailSender, SendGridEmailSender>(provider =>
                new SendGridEmailSender(
                    new LoggerFactory(),
                    apiKey,
                    email,
                    name));

            services.AddTransient<IEmailSender, NullMessageSender>();
            services.AddTransient<ISmsSender, NullMessageSender>();
            services.AddTransient<ISettingsService, SettingsService>();

            // Application services
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IProjectService, ProjectService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IProjectStatusSevice, ProjectStatusSevice>();
            services.AddScoped<IApplicationService, ApplicationService>();
            services.AddScoped<ITeamService, TeamService>();
            services.AddScoped<ITeamUserStatusService, TeamUserStatusService>();
            services.AddScoped<IApplicationStatusService, ApplicationStatusService>();
            services.AddScoped<ISpecificationService, SpecificationService>();

            var cloudinaryAccount = new Account(this.configuration["CloudName"], this.configuration["ApiKey"], this.configuration["ApiSecret"]);
            var cloudinary = new Cloudinary(cloudinaryAccount);
            services.AddSingleton(cloudinary);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            AutoMapperConfig.RegisterMappings(
                typeof(ErrorViewModel).GetTypeInfo().Assembly,
                typeof(ProjectDetailsViewModel).GetTypeInfo().Assembly,
                typeof(ProjectDetailsViewModel).GetTypeInfo().Assembly);

            // Seed data on application startup
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetRequiredService<ProjectsSoftuniDbContext>();

                if (env.IsDevelopment())
                {
                    dbContext.Database.Migrate();
                }

                new ProjectsSoftuniDbContextSeeder().SeedAsync(dbContext, serviceScope.ServiceProvider).GetAwaiter().GetResult();
            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute("areaRoute", "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                routes.MapRoute("default", "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
