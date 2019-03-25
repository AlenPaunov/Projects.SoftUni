namespace ProjectsSoftuni.Data.Seeding
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using ProjectsSoftuni.Common;
    using ProjectsSoftuni.Data.Models;

    public class ApplicationStatusesSeeder : ISeeder
    {
        public async Task SeedAsync(ProjectsSoftuniDbContext dbContext, IServiceProvider serviceProvider)
        {
            await SeedApplicationStatus(dbContext, GlobalConstants.AprovedApplicationStatus);
            await SeedApplicationStatus(dbContext, GlobalConstants.RejectedApplicationStatus);
            await SeedApplicationStatus(dbContext, GlobalConstants.WaitingApplicationStatus);
        }

        private static async Task SeedApplicationStatus(ProjectsSoftuniDbContext dbContext, string applicationStatusName)
        {
            var applicationStatus = dbContext.ApplicationStatuses.FirstOrDefault(s => s.Name == applicationStatusName);

            if (applicationStatus == null)
            {
                var result = new ApplicationStatus(applicationStatusName);
                await dbContext.ApplicationStatuses.AddAsync(result);
            }
        }
    }
}
