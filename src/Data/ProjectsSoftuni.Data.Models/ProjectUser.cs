namespace ProjectsSoftuni.Data.Models
{
    public class ProjectUser
    {
        public string ProjectId { get; set; }

        public virtual Project Project { get; set; }

        public string UserId { get; set; }

        public virtual ProjectsSoftuniUser User { get; set; }
    }
}
