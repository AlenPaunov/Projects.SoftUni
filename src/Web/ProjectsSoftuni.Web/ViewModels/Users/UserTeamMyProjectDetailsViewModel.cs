namespace ProjectsSoftuni.Web.ViewModels.Users
{
    using AutoMapper;
    using ProjectsSoftuni.Data.Models;
    using ProjectsSoftuni.Services.Mapping;

    public class UserTeamMyProjectDetailsViewModel : IHaveCustomMappings
    {
        public string Id { get; set; }

        public string Username { get; set; }

        public string TeamRole { get; set; }

        public void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<TeamUser, UserTeamMyProjectDetailsViewModel>()
                .ForMember(dto => dto.Id, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dto => dto.Username, opt => opt.MapFrom(src => src.User.UserName))
                .ForMember(dto => dto.TeamRole, opt => opt.MapFrom(src => src.TeamUserStatus.Name));
        }
    }
}
