namespace ProjectsSoftuni.Services.Contracts
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using ProjectsSoftuni.Data.Models;
    using ProjectsSoftuni.Services.Models.Projects;

    public interface IProjectService
    {
        Task<ICollection<TModel>> GetAllByApplicationStatusNameAsync<TModel>(string applicationStatus);

        Task<string> CreateAsync(
            string name,
            string description,
            string owner,
            DateTime? dueDate,
            string deployLink,
            decimal? budget);

        Task<ICollection<TModel>> GetAllAsync<TModel>();

        Task<TModel> GetProjectDetailsByIdAsync<TModel>(string id)
            where TModel : class;

        ProjectEditViewModel GetProjectEditViewModel(string id);

        Task<string> Edit(ProjectEditViewModel model);

        Task<ICollection<TModel>> GetProjectsByUserIdAsync<TModel>(string userId);

        Task<ICollection<TModel>> GetProjectsWithApprovedApplicationByUserIdAsync<TModel>(string userId);

        Task<TModel> GetProjectByIdAsync<TModel>(string id)
            where TModel : class;

        Task<string> UpdateSpecificationAsync(Specification specification, string projectId);
    }
}
