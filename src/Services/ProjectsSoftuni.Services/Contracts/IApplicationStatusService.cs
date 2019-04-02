namespace ProjectsSoftuni.Services.Contracts
{
    using System.Threading.Tasks;

    public interface IApplicationStatusService
    {
        Task<int> GetIdByNameAsync(string name);
    }
}
