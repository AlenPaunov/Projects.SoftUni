namespace ProjectsSoftuni.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using ProjectsSoftuni.Data.Common.Models;

    public class ApplicationStatus : BaseModel<int>
    {
        public ApplicationStatus(string name)
        {
            this.Name = name;
        }

        [Required]
        public string Name { get; set; }

        // Aproved
        // Waiting
        // Rejected
    }
}
