using ProjectsSoftuni.Data.Models;
using ProjectsSoftuni.Services.Mapping;

namespace ProjectsSoftuni.Services.Models.Projects
{
    public class ProjectIndexViewModel : IMapFrom<Project>
    {
        public string Id { get; set; }

        public string Name { get; set; }
        
        public string Owner { get; set; }

        public string StatusName { get; set; }
    }
}
