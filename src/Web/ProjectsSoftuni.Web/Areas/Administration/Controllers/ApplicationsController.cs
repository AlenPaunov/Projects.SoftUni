namespace ProjectsSoftuni.Web.Areas.Administration.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using ProjectsSoftuni.Services;

    public class ApplicationsController : AdministrationController
    {
        private readonly IApplicationService applicationService;

        public ApplicationsController(IApplicationService applicationService)
        {
            this.applicationService = applicationService;
        }

        public IActionResult Approve(string projectId, string userId)
        {
            return View();
        }
    }
}