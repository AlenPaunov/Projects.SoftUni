namespace ProjectsSoftuni.Web.Areas.Administration.ViewModels.Projects
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class CreateProjectInputModel
    {
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
    }
}
