using Microsoft.Extensions.Configuration;
using System.IO;

namespace ProjectsSoftuni.Web
{
    using Microsoft.AspNetCore;
    using Microsoft.AspNetCore.Hosting;

    public static class Program
    {
        //public static void Main(string[] args)
        //{
        //    CreateWebHostBuilder(args).Build().Run();
        //}

        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        //private static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
        //    WebHost.CreateDefaultBuilder(args).UseStartup<Startup>();

        private static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.SetBasePath(Directory.GetCurrentDirectory());
                    config.AddJsonFile("sendGridSettings.json", optional: false, reloadOnChange: false);
                })
                .UseStartup<Startup>();

        //private static IWebHost BuildWebHost(string[] args) =>
        //    WebHost.CreateDefaultBuilder(args)
        //        .ConfigureAppConfiguration((webHostBuilderContext, configurationBuilder) =>
        //        {
        //            var env = webHostBuilderContext.HostingEnvironment;
        //            configurationBuilder.SetBasePath(env.ContentRootPath);
        //            configurationBuilder.AddJsonFile("appsettings.json", false, true);
        //            configurationBuilder.AddJsonFile("../../../../sendGridSettings.json", false, true);
        //        })
        //        .UseStartup<Startup>()
        //        .Build();
    }
}
