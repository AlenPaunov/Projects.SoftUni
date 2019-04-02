namespace ProjectsSoftuni.Services.Contracts
{
    using System.Threading.Tasks;

    public interface ITeamUserStatusService
    {
        Task<int> GetIdByName(string name);
    }
}
