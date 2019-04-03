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
            this.Teams = new HashSet<Team>();
            this.Applications = new HashSet<Application>();
        }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        public int StatusId { get; set; }

        public virtual ProjectStatus Status { get; set; }

        [Required]
        public string Owner { get; set; }

        public DateTime? DueDate { get; set; }

        public string GitHubLink { get; set; }

        public string DeployLink { get; set; }

        public decimal? Budget { get; set; }

        public virtual ICollection<Team> Teams { get; set; }

        public virtual ICollection<Application> Applications { get; set; }
        
        //public virtual ICollection<ProjectUser> Team { get; set; }

    }
}
