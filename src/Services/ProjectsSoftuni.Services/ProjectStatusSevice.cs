namespace ProjectsSoftuni.Services
{
    using System.Collections.Generic;
    using System.Linq;

    using ProjectsSoftuni.Data.Common.Repositories;
    using ProjectsSoftuni.Data.Models;

    public class ProjectStatusSevice : IProjectStatusSevice
    {
        private readonly IRepository<ProjectStatus> projectStatusRepository;

        public ProjectStatusSevice(IRepository<ProjectStatus> projectStatusRepository)
        {
            this.projectStatusRepository = projectStatusRepository;
        }

        public ICollection<ProjectStatus> GetAllProjectStatuses()
        {
            var projectStatuses = this.projectStatusRepository.All().ToArray();

            return projectStatuses;
        }
    }
}
