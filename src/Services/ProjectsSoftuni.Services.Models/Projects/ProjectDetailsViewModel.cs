using ProjectsSoftuni.Data.Models;
using ProjectsSoftuni.Services.Mapping;

namespace ProjectsSoftuni.Services.Models.Projects
{
    public class ProjectDetailsViewModel : IMapFrom<Project>
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
        
        public string StatusName { get; set; }

        public string Owner { get; set; }

        public string DueDate { get; set; }

        public string GitHubLink { get; set; }

        public string DeployLink { get; set; }

        public decimal? Budget { get; set; }

    }
}
