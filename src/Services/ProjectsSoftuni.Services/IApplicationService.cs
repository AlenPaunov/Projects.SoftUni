using System.Collections.Generic;

namespace ProjectsSoftuni.Services
{
    using System.Threading.Tasks;

    public interface IApplicationService
    {
        Task<ICollection<TModel>> GetAllByProjectId<TModel>(string projectId);

        Task<bool> ApplyTeamForProjectAsync(string teamName, string projectId, string userId);
    }
}
