namespace ProjectsSoftuni.Services.Contracts
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using ProjectsSoftuni.Services.Models.Projects;

    public interface IProjectService
    {
        ProjectsIndexViewModel GetProjectsWithWaitingApplicationStatus();

        Task<string> CreateAsync(
            string name,
            string description,
            string owner,
            DateTime? dueDate,
            string gitHubLink,
            string deployLink,
            decimal? budget);

        ProjectsIndexViewModel GetAllProjects();

        ProjectDetailsViewModel GetProjectDetailsById(string id);

        ProjectEditViewModel GetProjectEditViewModel(string id);

        Task<string> Edit(ProjectEditViewModel model);

        Task<ICollection<TModel>> GetProjectsByUserIdAsync<TModel>(string userId);
    }
}
