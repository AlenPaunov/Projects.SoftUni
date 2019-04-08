using ProjectsSoftuni.Data.Models;
using ProjectsSoftuni.Services.Mapping;

namespace ProjectsSoftuni.Services.Models.Projects
{
    public class SpecificationProjectViewModel : IMapFrom<Project>
    {
        public string Name { get; set; }
    }
}
