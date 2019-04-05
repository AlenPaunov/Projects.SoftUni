namespace ProjectsSoftuni.Web.ViewModels.Teams
{
    using System.Collections.Generic;

    using ProjectsSoftuni.Data.Models;
    using ProjectsSoftuni.Services.Mapping;
    using ProjectsSoftuni.Web.ViewModels.Users;

    public class TeamMyProjectDetailsViewModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public ICollection<UserTeamMyProjectDetailsViewModel> Members { get; set; }
    }
}
