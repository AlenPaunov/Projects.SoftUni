using AutoMapper;

namespace ProjectsSoftuni.Web.ViewModels.Projects
{
    using ProjectsSoftuni.Data.Models;
    using ProjectsSoftuni.Services.Mapping;
    using ProjectsSoftuni.Web.ViewModels.Teams;

    public class MyProjectDetailsViewModel : IHaveCustomMappings
    {
        public string ProjectId { get; set; }

        public string ProjectName { get; set; }

        public string ProjectDescription { get; set; }

        public string ProjectStatusName { get; set; }

        public string ProjectOwner { get; set; }

        public string ProjectDueDate { get; set; }

        public string ProjectDeployLink { get; set; }

        public decimal? ProjectBudget { get; set; }

        public TeamMyProjectDetailsViewModel Team { get; set; }

        public void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<Team, MyProjectDetailsViewModel>()
                .ForMember(dto => dto.Team, opt => opt.MapFrom(t => t))
                .ForPath(dto => dto.Team.Members, src => src.MapFrom(t => t.Members));
        }
    }
}
