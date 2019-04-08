using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace ProjectsSoftuni.Services.Models.InputModels
{
    public class UploadSpecificationsInputModel
    {
        [Required]
        public string ProjectId { get; set; }
        
        [Required]
        [DataType(DataType.Upload)]
        public IFormFile SpecificationsFile { get; set; }
    }
}
