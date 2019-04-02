using ProjectsSoftuni.Services.Contracts;

namespace ProjectsSoftuni.Services
{
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using ProjectsSoftuni.Data.Common.Repositories;
    using ProjectsSoftuni.Data.Models;

    public class TeamUserStatusService : ITeamUserStatusService
    {
        private readonly IRepository<TeamUserStatus> teamUserStatusRepository;

        public TeamUserStatusService(IRepository<TeamUserStatus> teamUserStatusRepository)
        {
            this.teamUserStatusRepository = teamUserStatusRepository;
        }

        public async Task<int> GetIdByName(string name)
        {
            var teamUserStatus = await this.teamUserStatusRepository.AllAsNoTracking()
                .SingleOrDefaultAsync(s => s.Name == name);

            if (teamUserStatus == null)
            {
                return -1;
            }

            return teamUserStatus.Id;
        }
    }
}
