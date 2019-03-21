namespace ProjectsSoftuni.Web.Areas.Administration.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using ProjectsSoftuni.Services.Data;
    using ProjectsSoftuni.Web.Areas.Administration.ViewModels.Dashboard;

    public class DashboardController : AdministrationController
    {
        private readonly ISettingsService settingsService;

        public DashboardController(ISettingsService settingsService)
        {
            this.settingsService = settingsService;
        }

        public IActionResult Index()
        {
            var viewModel = new IndexViewModel { SettingsCount = this.settingsService.GetCount(), };
            return this.View(viewModel);
        }
    }
}
