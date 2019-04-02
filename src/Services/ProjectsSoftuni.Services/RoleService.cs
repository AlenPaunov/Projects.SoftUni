using ProjectsSoftuni.Services.Contracts;

namespace ProjectsSoftuni.Services
{
    using System.Linq;

    using ProjectsSoftuni.Common;
    using ProjectsSoftuni.Data.Common.Repositories;
    using ProjectsSoftuni.Data.Models;

    public class RoleService : IRoleService
    {
        private readonly IRepository<ProjectsSoftuniRole> rolesRepository;

        public RoleService(IRepository<ProjectsSoftuniRole> rolesRepository)
        {
            this.rolesRepository = rolesRepository;
        }

        public string GetRoleIdByName(string name)
        {
            Validator.ThrowIfStringIsNullOrEmpty(name, ExceptionMessages.RoleNameNull);

            var role = this.rolesRepository
                            .All()
                            .SingleOrDefault(r => r.Name == GlobalConstants.UserRoleName);

            return role.Id;
        }
    }
}
