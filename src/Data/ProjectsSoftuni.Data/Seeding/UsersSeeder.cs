namespace ProjectsSoftuni.Data.Seeding
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.DependencyInjection;
    using ProjectsSoftuni.Common;
    using ProjectsSoftuni.Data.Models;

    public class UsersSeeder : ISeeder
    {
        public async Task SeedAsync(ProjectsSoftuniDbContext dbContext, IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ProjectsSoftuniUser>>();

            await SeedUser(userManager, dbContext, GlobalConstants.DefaultAdminEmail, GlobalConstants.AdministratorRoleName, GlobalConstants.DefaultAdminPassword);
            await SeedUser(userManager, dbContext, GlobalConstants.DefaultUserEmail, GlobalConstants.UserRoleName, GlobalConstants.DefaultUserPassword);
        }

        private static async Task SeedUser(
            UserManager<ProjectsSoftuniUser> userManager,
            ProjectsSoftuniDbContext dbContext,
            string userEmail,
            string roleName,
            string password)
        {
            var user = dbContext.Users.FirstOrDefault(u => u.Email == userEmail);

            if (user == null)
            {
                user = new ProjectsSoftuniUser { UserName = userEmail, Email = userEmail };

                var role = dbContext.Roles.FirstOrDefault(r => r.Name == roleName);
                var userRole = new IdentityUserRole<string>() { RoleId = role.Id };
                user.Roles.Add(userRole);

                var result = await userManager.CreateAsync(user, password);
            }
        }
    }
}
