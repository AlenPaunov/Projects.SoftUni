namespace ProjectsSoftuni.Web
{
    using System.IO;

    using Microsoft.AspNetCore;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;

    public static class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        private static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.SetBasePath(Directory.GetCurrentDirectory());
                    config.AddJsonFile("sendGridSettings.json", optional: false, reloadOnChange: false);
                    config.AddJsonFile("cloudinarySettings.json", optional: false, reloadOnChange: false);
                })
                .UseStartup<Startup>();
    }
}
