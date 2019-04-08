namespace ProjectsSoftuni.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using ProjectsSoftuni.Data.Common.Models;

    public class Team : BaseModel<string>
    {
        public Team()
        {
            this.Members = new HashSet<TeamUser>();
        }

        [Required]
        public string Name { get; set; }

        public string GitHubLink { get; set; }

        public virtual ICollection<TeamUser> Members { get; set; }

        public string ProjectId { get; set; }

        public virtual Project Project { get; set; }

        public string ApplicationId { get; set; }

        public virtual Application Application { get; set; }
    }
}
