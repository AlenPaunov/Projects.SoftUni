namespace ProjectsSoftuni.Data.Seeding
{
    using System;
    using System.Threading.Tasks;

    public interface ISeeder
    {
        Task SeedAsync(ProjectsSoftuniDbContext dbContext, IServiceProvider serviceProvider);
    }
}
