namespace ProjectsSoftuni.Web.ViewModels.Users
{
    using ProjectsSoftuni.Data.Models;
    using ProjectsSoftuni.Services.Mapping;

    public class ProjectListViewModel : IMapFrom<Project>
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Owner { get; set; }

        public string StatusName { get; set; }
    }
}
