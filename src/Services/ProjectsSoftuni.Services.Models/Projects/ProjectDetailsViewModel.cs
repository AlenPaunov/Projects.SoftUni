namespace ProjectsSoftuni.Services.Models.Projects
{
    public class ProjectDetailsViewModel
    {
        public string Name { get; set; }

        public string Description { get; set; }
        
        public string Status { get; set; }

        public string Owner { get; set; }

        public string DueDate { get; set; }

        public string GitHubLink { get; set; }

        public string DeployLink { get; set; }

        public decimal? Budget { get; set; }

    }
}
