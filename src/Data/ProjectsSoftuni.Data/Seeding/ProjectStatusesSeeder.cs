namespace ProjectsSoftuni.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using ProjectsSoftuni.Common;
    using ProjectsSoftuni.Data.Models;

    public class ProjectStatusesSeeder : ISeeder
    {
        public async Task SeedAsync(ProjectsSoftuniDbContext dbContext, IServiceProvider serviceProvider)
        {
            await SeedProjectStatus(dbContext, GlobalConstants.OpenProjectStatus);
            await SeedProjectStatus(dbContext, GlobalConstants.InProgresProjectStatus);
            await SeedProjectStatus(dbContext, GlobalConstants.FinishedProjectStatus);
        }

        private static async Task SeedProjectStatus(ProjectsSoftuniDbContext dbContext, string projectStatusName)
        {
            var projectStatus = dbContext.ProjectStatuses.FirstOrDefault(ps => ps.Name == projectStatusName);

            if (projectStatus == null)
            {
                var result = new ProjectStatus(projectStatusName);
                await dbContext.ProjectStatuses.AddAsync(result);
            }
        }
    }
}
