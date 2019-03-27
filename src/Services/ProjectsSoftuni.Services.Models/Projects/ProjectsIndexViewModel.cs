namespace ProjectsSoftuni.Services.Models.Projects
{
    using System.Collections.Generic;
    using System.Linq;

    public class ProjectsIndexViewModel
    {
        public IQueryable<ProjectIndexViewModel> Projects { get; set; }
    }
}
