using System;
using System.ComponentModel.DataAnnotations;

namespace ProjectsSoftuni.Services.Models.Projects
{
    public class ProjectEditViewModel
    {
        public string Id { get; set; }

        [Required]
        [MinLength(5)]
        public string Name { get; set; }

        [Required]
        [MinLength(10)]
        public string Description { get; set; }

        [Required]
        [MinLength(5)]
        public string Owner { get; set; }
        
        public DateTime? DueDate { get; set; }

        public string DeployLink { get; set; }

        public decimal? Budget { get; set; }

        [Required]
        [Display(Name = "Status")]
        public int StatusId { get; set; }
    }
}
