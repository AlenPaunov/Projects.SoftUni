namespace ProjectsSoftuni.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using ProjectsSoftuni.Common;
    using ProjectsSoftuni.Data.Common.Repositories;
    using ProjectsSoftuni.Data.Models;
    using ProjectsSoftuni.Services.Contracts;
    using ProjectsSoftuni.Services.Mapping;
    using ProjectsSoftuni.Services.Models.Projects;

    public class ProjectService : IProjectService
    {
        private const int InvalidApplicationId = -1;

        private readonly IRepository<Project> projectsRepository;
        private readonly IRepository<ProjectStatus> projectStatusRepository;
        private readonly IUserService userService;
        private readonly IApplicationStatusService applicationStatusService;

        public ProjectService(
            IRepository<Project> projectsRepository,
            IRepository<ProjectStatus> projectStatusRepository,
            IUserService userService,
            IApplicationStatusService applicationStatusService)
        {
            this.projectsRepository = projectsRepository;
            this.projectStatusRepository = projectStatusRepository;
            this.userService = userService;
            this.applicationStatusService = applicationStatusService;
        }

        public async Task<ICollection<TModel>> GetAllByApplicationStatusNameAsync<TModel>(string applicationStatus)
        {
            var applicationId = await this.applicationStatusService.GetIdByNameAsync(applicationStatus);

            if (applicationId == InvalidApplicationId)
            {
                return null;
            }

            // TODO: Test sorting
            var projects = await this.projectsRepository
                .AllAsNoTracking()
                .Where(p => p.Applications.Any(u => u.ApplicationStatusId == applicationId))
                .OrderBy(p => p.CreatedOn)
                .To<TModel>()
                .ToListAsync();

            return projects;
        }

        public async Task<string> CreateAsync(string name, string description, string owner, DateTime? dueDate, string deployLink, decimal? budget)
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
                DeployLink = deployLink,
                Budget = budget,
                StatusId = openProjectStatus.Id,
            };

            await this.projectsRepository.AddAsync(project);
            await this.projectsRepository.SaveChangesAsync();

            return project.Id;
        }

        public async Task<ICollection<TModel>> GetAllAsync<TModel>()
        {
            // TODO: Test sorting
            var projects = await this.projectsRepository
                .AllAsNoTracking()
                .OrderBy(p => p.CreatedOn)
                .To<TModel>()
                .ToListAsync();

            return projects;
        }

        public async Task<TModel> GetProjectDetailsByIdAsync<TModel>(string id)
            where TModel : class
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return null;
            }

            var project = await this.projectsRepository
                .AllAsNoTracking()
                .Where(p => p.Id == id)
                .To<TModel>()
                .FirstOrDefaultAsync();

            return project;
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
            projectFromDb.DeployLink = model.DeployLink;
            projectFromDb.Budget = model.Budget;
            projectFromDb.StatusId = model.StatusId;

            this.projectsRepository.Update(projectFromDb);
            await this.projectsRepository.SaveChangesAsync();

            return projectFromDb.Id;
        }

        public async Task<ICollection<TModel>> GetProjectsByUserIdAsync<TModel>(string userId)
        {
            var projects = await this.projectsRepository
                .AllAsNoTracking()
                .Where(u => u.Teams.Any(t => t.Members.Any(m => m.UserId == userId)))
                .To<TModel>()
                .ToListAsync();

            return projects;
        }

        public async Task<ICollection<TModel>> GetProjectsWithApprovedApplicationByUserIdAsync<TModel>(string userId)
        {
            var projects = await this.projectsRepository
                .AllAsNoTracking()
                .Where(u => u.Teams.Any(t => t.Members.Any(m => m.UserId == userId)
                                             && t.Application.ApplicationStatus.Name == GlobalConstants.ApprovedApplicationStatus))
                .To<TModel>()
                .ToListAsync();

            return projects;
        }

        public async Task<TModel> GetProjectByIdAsync<TModel>(string id)
        where TModel : class
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return null;
            }

            var project = await this.projectsRepository
                .AllAsNoTracking()
                .Where(p => p.Id == id)
                .To<TModel>()
                .FirstOrDefaultAsync();

            return project;
        }

        public async Task<string> UpdateSpecificationAsync(Specification specification, string projectId)
        {
            var project = this.GetProjectById(projectId);

            project.Specification = specification;

            this.projectsRepository.Update(project);
            await this.projectsRepository.SaveChangesAsync();

            return project.SpecificationId;
        }

        private Project GetProjectById(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return null;
            }

            var project = this.projectsRepository.All().SingleOrDefault(p => p.Id == id);

            return project;
        }
    }
}
