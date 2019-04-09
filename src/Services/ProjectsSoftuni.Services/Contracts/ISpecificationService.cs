namespace ProjectsSoftuni.Services.Contracts
{
    using System.Threading.Tasks;

    using ProjectsSoftuni.Services.Models.InputModels;

    public interface ISpecificationService
    {
        Task<bool> UploadSpecificationAsync(UploadSpecificationsInputModel model);

        Task<string> GetSpecificationUrlByProjectIdAsync(string projectId);
    }
}
