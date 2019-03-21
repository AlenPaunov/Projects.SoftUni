namespace ProjectsSoftuni.Data
{
    using System.Security.Claims;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using ProjectsSoftuni.Data.Models;

    public class ProjectsSoftuniRoleStore : RoleStore<
        ProjectsSoftuniRole,
        ProjectsSoftuniDbContext,
        string,
        IdentityUserRole<string>,
        IdentityRoleClaim<string>>
    {
        public ProjectsSoftuniRoleStore(ProjectsSoftuniDbContext context, IdentityErrorDescriber describer = null)
            : base(context, describer)
        {
        }

        protected override IdentityRoleClaim<string> CreateRoleClaim(ProjectsSoftuniRole role, Claim claim) =>
            new IdentityRoleClaim<string>
            {
                RoleId = role.Id,
                ClaimType = claim.Type,
                ClaimValue = claim.Value,
            };
    }
}
