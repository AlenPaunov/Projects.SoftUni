namespace ProjectsSoftuni.Web.ViewModels.Teams
{
    using System.ComponentModel.DataAnnotations;

    public class AddMemberViewModel
    {
        [Required]
        [ValidMember]
        public string Member { get; set; }

        [Required]
        public string TeamId { get; set; }

        [Required]
        public string ProjectId { get; set; }
    }
}
