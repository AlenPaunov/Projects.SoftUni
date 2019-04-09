namespace ProjectsSoftuni.Web.ViewModels.Teams
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    using ProjectsSoftuni.Common;
    using ProjectsSoftuni.Data.Models;
    using ProjectsSoftuni.Services.Mapping;
    using ProjectsSoftuni.Web.ViewModels.Users;

    public class TeamMyProjectDetailsViewModel
    {
        public string Id { get; set; }

        [Display(Name = ModelConstants.TeamNameDisplay)]
        public string Name { get; set; }

        public ICollection<UserTeamMyProjectDetailsViewModel> Members { get; set; }
    }
}
