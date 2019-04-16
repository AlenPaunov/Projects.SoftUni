namespace ProjectsSoftuni.Web.Areas.Administration.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using ProjectsSoftuni.Services.Contracts;
    using ProjectsSoftuni.Web.Areas.Administration.ViewModels.Teams;
    using ProjectsSoftuni.Web.Models;

    public class TeamsController : AdministrationController
    {
        private const string NameSoftParam = "NameSortParam";
        private const string ProjectNameSortParam = "ProjectNameSortParam";
        private const string StatusSortParam = "StatusSortParam";
        private const string CurrentFilter = "CurrentFilter";
        private const string CurrentSort = "CurrentSort";
        private const string SortOrderName = "name";
        private const string SortOrderDescName = "name_desc";
        private const string SortOrderProjectName = "project";
        private const string SortOrderProjectDescName = "project_desc";

        private readonly ITeamService teamService;
        private readonly IApplicationStatusService applicationStatusService;

        public TeamsController(ITeamService teamService, IApplicationStatusService applicationStatusService)
        {
            this.teamService = teamService;
            this.applicationStatusService = applicationStatusService;
        }

        public async Task<IActionResult> Index(string sortOrder, string searchString, string currentFilter, int? pageIndex)
        {
            this.ViewData[NameSoftParam] = sortOrder == SortOrderName ? SortOrderDescName : SortOrderName;
            this.ViewData[ProjectNameSortParam] = sortOrder == SortOrderProjectName ? SortOrderProjectDescName : SortOrderProjectName;
            this.ViewData[CurrentFilter] = searchString;
            this.ViewData[CurrentSort] = sortOrder;

            if (searchString != null)
            {
                pageIndex = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            this.ViewData["CurrentFilter"] = searchString;

            var teams = await this.teamService.GetAllAsync<TeamIndexViewModel>();

            if (!string.IsNullOrWhiteSpace(searchString))
            {
                teams = teams
                     .Where(p => p.Name.ToLower().Contains(searchString.ToLower())
                         || p.ProjectName.ToLower().Contains(searchString.ToLower()))
                     .ToList();
            }

            switch (sortOrder)
            {
                case SortOrderDescName:
                    teams = teams.OrderByDescending(p => p.Name).ToList();
                    break;
                case SortOrderName:
                    teams = teams.OrderBy(p => p.Name).ToList();
                    break;
                case SortOrderProjectDescName:
                    teams = teams.OrderByDescending(p => p.ProjectName).ToList();
                    break;
                case SortOrderProjectName:
                    teams = teams.OrderBy(p => p.ProjectName).ToList();
                    break;
            }

            //var projectStatuses = this.applicationStatusService.();
            //this.ViewData[ProjectStatusesStr] = projectStatuses.Select(s => new SelectListItem
            //{
            //    Value = s.Id.ToString(),
            //    Text = s.Name,
            //});

            int pageSize = 5;
            return this.View(PaginatedList<TeamIndexViewModel>.Create(teams, pageIndex ?? 1, pageSize));
        }
    }
}