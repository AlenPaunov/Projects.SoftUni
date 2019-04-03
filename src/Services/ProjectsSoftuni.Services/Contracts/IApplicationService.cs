namespace ProjectsSoftuni.Services.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IApplicationService
    {
        Task<ICollection<TModel>> GetAllByProjectId<TModel>(string projectId);

        Task<bool> ApplyTeamForProjectAsync(string teamName, string projectId, string userId);

        Task<bool> ApproveApplicationAsync(string projectId, string teamId);

        Task<bool> RejectApplicationAsync(string projectId, string teamId);
    }
}
