namespace ProjectsSoftuni.Services
{
    using System.Linq;

    using Microsoft.EntityFrameworkCore;
    using ProjectsSoftuni.Common;
    using ProjectsSoftuni.Data.Common.Repositories;
    using ProjectsSoftuni.Data.Models;

    public class UserService : IUserService
    {
        private readonly IRepository<ProjectsSoftuniUser> userRepository;

        public UserService(IRepository<ProjectsSoftuniUser> userRepository)
        {
            this.userRepository = userRepository;
        }

        public bool ApplicationEnabled(string userId)
        {
            var user = this.GetUserById(userId);

            if (user.AprovedProjects.Any(p => p.Project.Status.Name == GlobalConstants.InProgresProjectStatus))
            {
                return false;
            }

            if (user.Applications.Any(a => a.ApplicationStatus.Name == GlobalConstants.WaitingApplicationStatus))
            {
                return false;
            }

            return true;
        }

        public bool IsRejectedForTheProject(string userId, string projectId)
        {
            Validator.ThrowIfStringIsNullOrEmpty(projectId, "ProjectId can't be null. UserService");

            var user = this.GetUserById(userId);
            var isRejected = false;

            if (user?.Applications.SingleOrDefault(a => a.ProjectId == projectId)?.ApplicationStatus.Name == GlobalConstants.RejectedApplicationStatus)
            {
                isRejected = true;
            }

            return isRejected;
        }

        private ProjectsSoftuniUser GetUserById(string userId)
        {
            Validator.ThrowIfStringIsNullOrEmpty(userId, "UserId can't be null. UserService");

            var user = this.userRepository
                .All()
                .Include(u => u.Applications)
                    .ThenInclude(a => a.ApplicationStatus)
                .Include(u => u.AprovedProjects)
                    .ThenInclude(ap => ap.Project)
                    .ThenInclude(p => p.Status)
                .SingleOrDefault(u => u.Id == userId);
            return user;
        }
    }
}
