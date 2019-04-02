namespace ProjectsSoftuni.Services.Contracts
{
    public interface IUserService
    {
        bool ApplicationEnabled(string userId);

        bool IsRejectedForTheProject(string userId, string projectId);
    }
}
