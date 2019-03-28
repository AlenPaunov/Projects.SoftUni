namespace ProjectsSoftuni.Data.Models
{
    public class Application
    {
        public string ProjectId { get; set; }

        public virtual Project Project { get; set; }

        public string UserId { get; set; }

        public virtual ProjectsSoftuniUser User { get; set; }

        public int ApplicationStatusId { get; set; }

        public virtual ApplicationStatus ApplicationStatus { get; set; }
    }
}
