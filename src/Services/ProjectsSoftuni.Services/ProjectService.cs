namespace ProjectsSoftuni.Services
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;

    using ProjectsSoftuni.Common;
    using ProjectsSoftuni.Data.Common.Repositories;
    using ProjectsSoftuni.Data.Models;
    using ProjectsSoftuni.Services.Models.Projects;

    public class ProjectService : IProjectService
    {
        private readonly IRepository<Project> projectsRepository;
        private readonly IRepository<ProjectStatus> projectStatusRepository;

        public ProjectService(IRepository<Project> projectsRepository, IRepository<ProjectStatus> projectStatusRepository)
        {
            this.projectsRepository = projectsRepository;
            this.projectStatusRepository = projectStatusRepository;
        }

        public ProjectsIndexViewModel GetProjectsWithWaitingApplicationStatus()
        {
            // TODO: Test sorting
            var projects = this.projectsRepository
                .All()
                .Where(p => p.Applications.Any(u => u.ApplicationStatus.Name == GlobalConstants.WaitingApplicationStatus))
                .OrderBy(p => p.CreatedOn)
                .Select(p => new ProjectIndexViewModel()
                {
                    Id = p.Id,
                    Name = p.Name,
                    Owner = p.Owner,
                })
            .ToList();

            var projectsViewModel = new ProjectsIndexViewModel() { Projects = projects };

            return projectsViewModel;
        }

        public async Task<string> CreateAsync(string name, string description, string owner, string dueDate, string gitHubLink, string deployLink, decimal? budget)
        {
            DateTime? dt = null;

            if (!string.IsNullOrEmpty(dueDate))
            {
                dt = DateTime.ParseExact(dueDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            }

            var openProjectStatus = this.projectStatusRepository.All().SingleOrDefault(s => s.Name == GlobalConstants.OpenProjectStatus);

            var project = new Project()
            {
                Name = name,
                Description = description,
                Owner = owner,
                DueDate = dt,
                GitHubLink = gitHubLink,
                DeployLink = deployLink,
                Budget = budget,
                StatusId = openProjectStatus.Id,
            };

            await this.projectsRepository.AddAsync(project);
            await this.projectsRepository.SaveChangesAsync();

            return project.Id;
        }

        public ProjectsIndexViewModel GetAllProjects()
        {
            // TODO: Test sorting
            var projects = this.projectsRepository
                .All()
                .OrderBy(p => p.CreatedOn)
                .Select(p => new ProjectIndexViewModel()
                {
                    Id = p.Id,
                    Name = p.Name,
                    Owner = p.Owner,
                })
            .ToList();

            var projectsViewModel = new ProjectsIndexViewModel() { Projects = projects };

            return projectsViewModel;
        }
    }
}
