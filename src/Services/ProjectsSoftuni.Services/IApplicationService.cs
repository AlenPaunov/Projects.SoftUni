namespace ProjectsSoftuni.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IApplicationService
    {
        Task<ICollection<TModel>> GetAllByProjectId<TModel>(string projectId);
    }
}
