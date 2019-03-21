namespace ProjectsSoftuni.Data.Seeding
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using ProjectsSoftuni.Data.Models;

    internal class SettingsSeeder : ISeeder
    {
        public async Task SeedAsync(ProjectsSoftuniDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.Settings.Any())
            {
                return;
            }

            await dbContext.Settings.AddAsync(new Setting { Name = "Setting1", Value = "value1" });
        }
    }
}
