using Microsoft.EntityFrameworkCore;
using ProjectsSoftuni.Data.Common.Repositories;
using ProjectsSoftuni.Data.Models;
using ProjectsSoftuni.Services.Contracts;
using System.Threading.Tasks;

namespace ProjectsSoftuni.Services
{
    public class ApplicationStatusService : IApplicationStatusService
    {
        private readonly IRepository<ApplicationStatus> applicationStatusRepository;

        public ApplicationStatusService(IRepository<ApplicationStatus> applicationStatusesRepository)
        {
            this.applicationStatusRepository = applicationStatusesRepository;
        }

        public async Task<int> GetIdByNameAsync(string name)
        {
            var applicationStatusId = -1;

            if (string.IsNullOrWhiteSpace(name))
            {
                return applicationStatusId;
            }

            var applicationStatus = await this.applicationStatusRepository
                .AllAsNoTracking()
                .SingleOrDefaultAsync(s => s.Name == name);

            if (applicationStatus == null)
            {
                return applicationStatusId;
            }

            applicationStatusId = applicationStatus.Id;
            return applicationStatusId;
        }
    }
}
