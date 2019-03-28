namespace ProjectsSoftuni.Services
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using ProjectsSoftuni.Data.Models;
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

        Task<bool> ApplyForProjectAsync(string projectId, string userId);

        ProjectEditViewModel GetProjectEditViewModel(string id);

        Task<string> Edit(ProjectEditViewModel model);
    }
}
