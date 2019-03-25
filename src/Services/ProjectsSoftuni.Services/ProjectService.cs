namespace ProjectsSoftuni.Services
{
    using System.Linq;

    using ProjectsSoftuni.Data.Common.Repositories;
    using ProjectsSoftuni.Data.Models;
    using ProjectsSoftuni.Services.Models;

    public class ProjectService : IProjectService
    {
        private readonly IRepository<Project> projectsRepository;

        public ProjectService(IRepository<Project> projectsRepository)
        {
            this.projectsRepository = projectsRepository;
        }

        public ProjectsIndexViewModel GetProjectsWithWaitingApplicationStatus()
        {
            var projects = this.projectsRepository.All().ToList().Select(p => new ProjectIndexViewModel()
            {
                Id = p.Id,
                Name = p.Name,
                Owner = p.Owner,
            }).ToList();

            var projectsViewModel = new ProjectsIndexViewModel() { Projects = projects };

            return projectsViewModel;
        }
    }
}
