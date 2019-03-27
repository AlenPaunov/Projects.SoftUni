namespace ProjectsSoftuni.Services
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using ProjectsSoftuni.Common;
    using ProjectsSoftuni.Data.Common.Repositories;
    using ProjectsSoftuni.Data.Models;
    using ProjectsSoftuni.Services.Models.Projects;

    public class ProjectService : IProjectService
    {
        private readonly IRepository<Project> projectsRepository;
        private readonly IRepository<ProjectStatus> projectStatusRepository;
        private readonly IRepository<ProjectsSoftuniUser> userRepository;
        private readonly IRepository<Application> applicationsRepository;
        private readonly IRepository<ApplicationStatus> applicationStatusesRepository;
        private readonly IUserService userService;

        public ProjectService(
            IRepository<Project> projectsRepository,
            IRepository<ProjectStatus> projectStatusRepository,
            IRepository<ProjectsSoftuniUser> userRepository,
            IRepository<Application> applicationsRepository,
            IRepository<ApplicationStatus> applicationStatusesRepository,
            IUserService userService)
        {
            this.projectsRepository = projectsRepository;
            this.projectStatusRepository = projectStatusRepository;
            this.userRepository = userRepository;
            this.applicationsRepository = applicationsRepository;
            this.applicationStatusesRepository = applicationStatusesRepository;
            this.userService = userService;
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

        public async Task<string> CreateAsync(string name, string description, string owner, DateTime? dueDate, string gitHubLink, string deployLink, decimal? budget)
        {
            var openProjectStatus = this.projectStatusRepository
                .All()
                .SingleOrDefault(s => s.Name == GlobalConstants.OpenProjectStatus);

            var project = new Project()
            {
                Name = name,
                Description = description,
                Owner = owner,
                DueDate = dueDate,
                GitHubLink = gitHubLink,
                DeployLink = deployLink,
                Budget = budget,
                StatusId = openProjectStatus.Id,
            };

            await this.projectsRepository.AddAsync(project);
            await this.projectsRepository.SaveChangesAsync();

            return project.Id;
        }

        public async Task<ProjectsIndexViewModel> GetAllProjects()
        {
            // TODO: Test sorting
            var projects = await this.projectsRepository
                .AllAsNoTracking()
                .OrderBy(p => p.CreatedOn)
                .Select(p => new ProjectIndexViewModel()
                {
                    Id = p.Id,
                    Name = p.Name,
                    Owner = p.Owner,
                    Status = p.Status.Name,
                })
                .ToListAsync();

            var projectsViewModel = new ProjectsIndexViewModel() { Projects = projects };

            return projectsViewModel;
        }

        public ProjectDetailsViewModel GetProjectDetailsById(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return null;
            }

            var project = this.projectsRepository.All().Include(p => p.Status).FirstOrDefault(p => p.Id == id);

            ProjectDetailsViewModel projectViewModel = null;

            if (project != null)
            {
                projectViewModel = new ProjectDetailsViewModel()
                {
                    Id = project.Id,
                    Budget = project.Budget,
                    DeployLink = project.DeployLink,
                    Description = project.Description,
                    DueDate = project.DueDate.GetValueOrDefault().ToString("yyyy-MM-dd"),
                    GitHubLink = project.GitHubLink,
                    Name = project.Name,
                    Owner = project.Owner,
                    Status = project.Status.Name,
                };
            }

            return projectViewModel;
        }

        public async Task<bool> ApplyForProjectAsync(string projectId, string userId)
        {
            if (string.IsNullOrWhiteSpace(projectId) && string.IsNullOrWhiteSpace(userId))
            {
                return false;
            }

            var project = this.projectsRepository.All().Include(p => p.Status).SingleOrDefault(p => p.Id == projectId);
            var user = this.userRepository.All().SingleOrDefault(p => p.Id == userId);

            if (project.Status.Name != GlobalConstants.OpenProjectStatus)
            {
                return false;
            }

            if (!this.userService.ApplicationEnabled(userId))
            {
                return false;
            }

            var applicationStatus = this.applicationStatusesRepository
                .All()
                .FirstOrDefault(s => s.Name == GlobalConstants.WaitingApplicationStatus);

            var application = new Application()
            {
                Project = project,
                User = user,
                ApplicationStatus = applicationStatus,
            };

            await this.applicationsRepository.AddAsync(application);
            await this.applicationsRepository.SaveChangesAsync();

            return true;
        }

        public ProjectEditViewModel GetProjectEditViewModel(string id)
        {
            var project = this.GetProjectById(id);

            if (project == null)
            {
                return null;
            }

            var projectViewModel = new ProjectEditViewModel()
            {
                Id = project.Id,
                Budget = project.Budget,
                DeployLink = project.DeployLink,
                Description = project.Description,
                DueDate = project.DueDate,
                GitHubLink = project.GitHubLink,
                Name = project.Name,
                Owner = project.Owner,
                StatusId = project.StatusId,
            };

            return projectViewModel;
        }

        public async Task<string> Edit(ProjectEditViewModel model)
        {
            var projectFromDb = this.GetProjectById(model.Id);

            if (projectFromDb == null)
            {
                return null;
            }

            projectFromDb.Name = model.Name;
            projectFromDb.Description = model.Description;
            projectFromDb.Owner = model.Owner;
            projectFromDb.DueDate = model.DueDate;
            projectFromDb.GitHubLink = model.GitHubLink;
            projectFromDb.DeployLink = model.DeployLink;
            projectFromDb.Budget = model.Budget;
            projectFromDb.StatusId = model.StatusId;

            this.projectsRepository.Update(projectFromDb);
            await this.projectsRepository.SaveChangesAsync();

            return projectFromDb.Id;
        }

        private Project GetProjectById(string id)
        {
            Validator.ThrowIfStringIsNullOrEmpty(id, ExceptionMessages.ProjectIdNull);

            var project = this.projectsRepository.All().SingleOrDefault(p => p.Id == id);

            return project;
        }
    }
}
