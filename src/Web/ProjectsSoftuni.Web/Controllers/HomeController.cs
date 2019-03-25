namespace ProjectsSoftuni.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using ProjectsSoftuni.Services;

    public class HomeController : BaseController
    {
        private readonly IProjectService projectService;

        public HomeController(IProjectService projectService)
        {
            this.projectService = projectService;
        }

        public IActionResult Index()
        {
            var projects = this.projectService.GetAllProjects();
            return this.View(projects);
        }

        public IActionResult Privacy()
        {
            return this.View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() => this.View();
    }
}
