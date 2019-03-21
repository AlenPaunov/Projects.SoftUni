namespace ProjectsSoftuni.Services.Data
{

    using ProjectsSoftuni.Data.Models;

    public interface IRoleService
    {
        ProjectsSoftuniRole GetRoleByName(string name);
    }
}
