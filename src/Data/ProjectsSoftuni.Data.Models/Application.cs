namespace ProjectsSoftuni.Data.Models
{
    public class Application
    {
        public string ProjectId { get; set; }

        public Project Project { get; set; }

        public string UserId { get; set; }

        public ProjectsSoftuniUser User { get; set; }

        public int ApplicationStatusId { get; set; }

        public ApplicationStatus ApplicationStatus { get; set; }
    }
}
