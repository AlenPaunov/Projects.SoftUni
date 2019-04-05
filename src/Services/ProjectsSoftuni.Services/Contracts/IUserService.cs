namespace ProjectsSoftuni.Services.Contracts
{
    using System.Threading.Tasks;

    public interface IUserService
    {
        bool ApplicationEnabled(string userId);

        bool IsRejectedForTheProject(string userId, string projectId);

        Task<bool> IsMemberValidAsync(string member);

        Task<TModel> GetByIdAsync<TModel>(string id)
            where TModel : class;

        Task<TModel> GetByUsernameOrEmailAsync<TModel>(string member)
            where TModel : class;
    }
}
