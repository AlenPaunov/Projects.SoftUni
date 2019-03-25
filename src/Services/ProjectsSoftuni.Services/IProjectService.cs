namespace ProjectsSoftuni.Services
{
    using System.Threading.Tasks;

    using ProjectsSoftuni.Services.Models.Projects;

    public interface IProjectService
    {
        ProjectsIndexViewModel GetProjectsWithWaitingApplicationStatus();

        Task<string> CreateAsync(
            string name,
            string description,
            string owner,
            string dueDate,
            string gitHubLink,
            string deployLink,
            decimal? budget);

        ProjectsIndexViewModel GetAllProjects();

        ProjectDetailsViewModel GetProjectDetailsById(string id);
    }
}
