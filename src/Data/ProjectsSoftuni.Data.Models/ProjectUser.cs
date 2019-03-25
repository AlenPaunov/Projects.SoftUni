namespace ProjectsSoftuni.Data.Models
{
    public class ProjectUser
    {
        public string ProjectId { get; set; }

        public Project Project { get; set; }

        public string UserId { get; set; }

        public ProjectsSoftuniUser User { get; set; }
    }
}
