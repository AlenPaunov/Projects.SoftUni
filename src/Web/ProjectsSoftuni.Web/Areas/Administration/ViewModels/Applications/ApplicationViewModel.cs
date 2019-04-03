namespace ProjectsSoftuni.Web.Areas.Administration.ViewModels.Applications
{
    using ProjectsSoftuni.Data.Models;
    using ProjectsSoftuni.Services.Mapping;

    public class ApplicationViewModel : IMapFrom<Application>
    {
        public string TeamId { get; set; }

        public string TeamName { get; set; }

        public string ProjectId { get; set; }

        public int ApplicationStatusId { get; set; }

        public string ApplicationStatusName { get; set; }
    }
}
