namespace ProjectsSoftuni.Services.Contracts
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using ProjectsSoftuni.Services.Models.Projects;

    public interface IProjectService
    {
        Task<ICollection<TModel>> GetAllByApplicationStatusNameAsync<TModel>(string applicationStatus);

        Task<string> CreateAsync(
            string name,
            string description,
            string owner,
            DateTime? dueDate,
            string gitHubLink,
            string deployLink,
            decimal? budget);

        Task<ICollection<TModel>> GetAllAsync<TModel>();

        ProjectDetailsViewModel GetProjectDetailsById(string id);

        ProjectEditViewModel GetProjectEditViewModel(string id);

        Task<string> Edit(ProjectEditViewModel model);

        Task<ICollection<TModel>> GetProjectsByUserIdAsync<TModel>(string userId);
    }
}
