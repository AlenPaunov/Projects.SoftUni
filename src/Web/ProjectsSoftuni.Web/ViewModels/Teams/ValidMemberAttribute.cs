namespace ProjectsSoftuni.Web.ViewModels.Teams
{
    using System.ComponentModel.DataAnnotations;
    using System.Threading.Tasks;

    using ProjectsSoftuni.Common;
    using ProjectsSoftuni.Services.Contracts;

    public class ValidMemberAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var service = (IUserService)validationContext
                .GetService(typeof(IUserService));

            if (this.IsValidMember((string)value, service).Result)
            {
                return ValidationResult.Success;
            }

            return new ValidationResult(ExceptionMessages.InvalidMember);
        }

        private async Task<bool> IsValidMember(string member, IUserService service)
        {
            var isValid = await service.IsMemberValidAsync(member);

            return isValid;
        }
    }
}
