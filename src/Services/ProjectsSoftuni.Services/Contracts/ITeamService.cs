namespace ProjectsSoftuni.Services.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ITeamService
    {
        Task<string> CreteTeamAsync(string teamName, string projectId, string userId);

        Task<TModel> GetByProjectIdAsync<TModel>(string projectId, string userId)
            where TModel : class;

        Task<ICollection<TModel>> GetAllAsync<TModel>();

        //Task<bool> SendApprovalMailAsync(string memberStr, string teamId, string invitationUserId);
    }
}
