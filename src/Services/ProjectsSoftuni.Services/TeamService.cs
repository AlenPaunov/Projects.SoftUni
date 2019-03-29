using ProjectsSoftuni.Common;
using ProjectsSoftuni.Data.Common.Repositories;
using ProjectsSoftuni.Data.Models;
using System.Threading.Tasks;

namespace ProjectsSoftuni.Services
{
    public class TeamService : ITeamService
    {
        private readonly IRepository<Team> teamRepository;
        private readonly ITeamUserStatusService teamUserStatusService;

        public TeamService(IRepository<Team> teamRepository, ITeamUserStatusService teamUserStatusService)
        {
            this.teamRepository = teamRepository;
            this.teamUserStatusService = teamUserStatusService;
        }

        public async Task<string> CreteTeam(string teamName, string projectId, string userId)
        {
            var teamUserStatusId = this.teamUserStatusService.GetIdByName(GlobalConstants.TeamUserStatusTeamLead).Result;

            var teamUser = new TeamUser() { UserId = userId, TeamUserStatusId = teamUserStatusId };

            var team = new Team()
            {
                Name = teamName,
                ProjectId = projectId,
            };

            team.Members.Add(teamUser);

            await this.teamRepository.AddAsync(team);
            await this.teamRepository.SaveChangesAsync();

            return team.Id;
        }
    }
}
