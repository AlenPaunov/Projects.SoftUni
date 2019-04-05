namespace ProjectsSoftuni.Services.Contracts
{
    using System.Threading.Tasks;

    public interface ITeamService
    {
        Task<string> CreteTeam(string teamName, string projectId, string userId);

        Task<TModel> GetByProjectIdAsync<TModel>(string projectId, string userId)
            where TModel : class;

        Task<bool> SendApprovalMailAsync(string memberStr, string teamId, string invitationUserId);
    }
}
