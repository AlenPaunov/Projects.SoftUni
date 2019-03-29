namespace ProjectsSoftuni.Services
{
    using System.Threading.Tasks;

    public interface ITeamService
    {
        Task<string> CreteTeam(string teamName, string projectId, string userId);
    }
}
