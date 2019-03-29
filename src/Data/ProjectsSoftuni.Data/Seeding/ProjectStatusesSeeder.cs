namespace ProjectsSoftuni.Data.Seeding
{
    using ProjectsSoftuni.Common;
    using ProjectsSoftuni.Data.Models;
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    public class ProjectStatusesSeeder : ISeeder
    {
        public async Task SeedAsync(ProjectsSoftuniDbContext dbContext, IServiceProvider serviceProvider)
        {
            await SeedProjectStatus(dbContext, GlobalConstants.OpenProjectStatus);
            await SeedProjectStatus(dbContext, GlobalConstants.InProgressProjectStatus);
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
