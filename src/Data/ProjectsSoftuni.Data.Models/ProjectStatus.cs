namespace ProjectsSoftuni.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using ProjectsSoftuni.Data.Common.Models;

    public class ProjectStatus : BaseModel<int>
    {
        public ProjectStatus()
        {
        }

        public ProjectStatus(string projectStatusName)
        {
            this.Name = projectStatusName;
        }

        [Required]
        public string Name { get; set; }

        // Open
        // In Progress
        // Finished
    }
}
