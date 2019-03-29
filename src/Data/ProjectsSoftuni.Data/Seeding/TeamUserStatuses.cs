namespace ProjectsSoftuni.Data.Seeding
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using ProjectsSoftuni.Common;
    using ProjectsSoftuni.Data.Models;

    public class TeamUserStatuses : ISeeder
    {
        public async Task SeedAsync(ProjectsSoftuniDbContext dbContext, IServiceProvider serviceProvider)
        {
            await SeedProjectUserStatus(dbContext, GlobalConstants.TeamUserStatusTeamLead);
            await SeedProjectUserStatus(dbContext, GlobalConstants.TeamUserStatusMember);
        }

        private static async Task SeedProjectUserStatus(ProjectsSoftuniDbContext dbContext, string projectUserStatusName)
        {
            var projectUserStatus = dbContext.TeamUserStatuses.FirstOrDefault(ps => ps.Name == projectUserStatusName);

            if (projectUserStatus == null)
            {
                var result = new TeamUserStatus() { Name = projectUserStatusName };
                await dbContext.TeamUserStatuses.AddAsync(result);
            }
        }
    }
}
