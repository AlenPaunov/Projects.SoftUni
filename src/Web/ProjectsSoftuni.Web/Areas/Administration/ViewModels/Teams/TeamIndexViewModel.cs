namespace ProjectsSoftuni.Web.Areas.Administration.ViewModels.Teams
{
    using ProjectsSoftuni.Data.Models;
    using ProjectsSoftuni.Services.Mapping;

    public class TeamIndexViewModel : IMapFrom<Team>
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string ProjectId { get; set; }

        public string ProjectName { get; set; }
    }
}
