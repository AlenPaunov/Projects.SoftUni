namespace ProjectsSoftuni.Services
{
    using System.Threading.Tasks;

    public interface ITeamUserStatusService
    {
        Task<int> GetIdByName(string name);
    }
}
