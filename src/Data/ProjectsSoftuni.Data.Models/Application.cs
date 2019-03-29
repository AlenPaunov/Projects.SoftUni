namespace ProjectsSoftuni.Data.Models
{
    using ProjectsSoftuni.Data.Common.Models;

    public class Application
    {
        public string ProjectId { get; set; }

        public virtual Project Project { get; set; }

        public string TeamId { get; set; }

        public virtual Team Team { get; set; }

        public int ApplicationStatusId { get; set; }

        public virtual ApplicationStatus ApplicationStatus { get; set; }
    }
}
