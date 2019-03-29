namespace ProjectsSoftuni.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using ProjectsSoftuni.Data.Common.Models;

    public class TeamUserStatus : BaseModel<int>
    {
        [Required]
        public string Name { get; set; }
    }
}