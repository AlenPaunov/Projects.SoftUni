namespace ProjectsSoftuni.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using ProjectsSoftuni.Data.Common.Models;

    public class Project : BaseModel<string>
    {
        public Project()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Team = new HashSet<ProjectUser>();
            this.Applications = new HashSet<Application>();
        }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        public int StatusId { get; set; }

        public ProjectStatus Status { get; set; }

        [Required]
        public string Owner { get; set; }

        public DateTime? FinishDate { get; set; }

        public string GitHubLink { get; set; }

        public string DeployLink { get; set; }

        public decimal Budget { get; set; }

        public virtual ICollection<ProjectUser> Team { get; set; }

        public virtual ICollection<Application> Applications { get; set; }
    }
}
