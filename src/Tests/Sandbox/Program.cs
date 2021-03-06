﻿namespace Sandbox
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;

    using CommandLine;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.UI.Services;
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
    using ProjectsSoftuni.Services.Data;
    using ProjectsSoftuni.Services.Mapping;
    using ProjectsSoftuni.Services.Messaging;

    public static class Program
    {
        public static int Main(string[] args)
        {
            Console.WriteLine($"{typeof(Program).Namespace} ({string.Join(" ", args)}) starts working...");
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider(true);

            // Seed data on application startup
            using (var serviceScope = serviceProvider.CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetRequiredService<ProjectsSoftuniDbContext>();
                dbContext.Database.Migrate();
                new ProjectsSoftuniDbContextSeeder().SeedAsync(dbContext, serviceScope.ServiceProvider).GetAwaiter().GetResult();
            }

            using (var serviceScope = serviceProvider.CreateScope())
            {
                serviceProvider = serviceScope.ServiceProvider;

                return Parser.Default.ParseArguments<SandboxOptions>(args).MapResult(
                    opts => SandboxCode(opts, serviceProvider),
                    _ => 255);
            }
        }


        class UserDto
        {
            public string Name { get; set; }

            public string Description { get; set; }

            public int StatusId { get; set; }

            public string StatusName { get; set; }
        }

        private static int SandboxCode(SandboxOptions options, IServiceProvider serviceProvider)
        {
            var sw = Stopwatch.StartNew();
            var settingsService = serviceProvider.GetService<ISettingsService>();
            var dbContext = serviceProvider.GetService<ProjectsSoftuniDbContext>();

            var projects = dbContext
                .Projects;

            var projectDtos = projects.To<UserDto>().ToList();
            //.To<UserDto>()
            //.ToList();

            var roles = dbContext.Roles.ToList();
            var rolesUsers = dbContext.UserRoles.ToList();

            Console.WriteLine($"Count of settings: {settingsService.GetCount()}");
            Console.WriteLine(sw.Elapsed);
            return 0;
        }

        private static void ConfigureServices(ServiceCollection services)
        {
            AutoMapperConfig.RegisterMappings(typeof(UserDto).Assembly);

            var configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false, true)
                .AddEnvironmentVariables()
                .Build();

            services.AddSingleton<IConfiguration>(configuration);
            services.AddDbContext<ProjectsSoftuniDbContext>(
                options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"))
                    .UseLoggerFactory(new LoggerFactory()));

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
                .AddDefaultTokenProviders();

            services.AddScoped(typeof(IDeletableEntityRepository<>), typeof(EfDeletableEntityRepository<>));
            services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
            services.AddScoped<IDbQueryRunner, DbQueryRunner>();

            // Application services
            services.AddTransient<IEmailSender, NullMessageSender>();
            services.AddTransient<ISmsSender, NullMessageSender>();
            services.AddTransient<ISettingsService, SettingsService>();
        }
    }
}
