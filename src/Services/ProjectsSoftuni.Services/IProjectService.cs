namespace ProjectsSoftuni.Services
{
    using ProjectsSoftuni.Services.Models;

    public interface IProjectService
    {
        ProjectsIndexViewModel GetProjectsWithWaitingApplicationStatus();
    }
}
