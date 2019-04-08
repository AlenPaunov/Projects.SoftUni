namespace ProjectsSoftuni.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using ProjectsSoftuni.Data.Common.Models;

    public class Specification : BaseModel<string>
    {
        [Required]
        public string SpecificationId { get; set; }

        public virtual Project Project { get; set; }
    }
}
