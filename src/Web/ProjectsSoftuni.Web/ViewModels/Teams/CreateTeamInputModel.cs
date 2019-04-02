namespace ProjectsSoftuni.Web.ViewModels.Teams
{
    using System.ComponentModel.DataAnnotations;

    public class CreateTeamInputModel
    {
        public string ProjectId { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
