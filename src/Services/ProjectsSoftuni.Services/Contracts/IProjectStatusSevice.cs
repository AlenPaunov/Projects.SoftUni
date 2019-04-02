namespace ProjectsSoftuni.Services.Contracts
{
    using System.Collections.Generic;

    using ProjectsSoftuni.Data.Models;

    public interface IProjectStatusSevice
    {
        ICollection<ProjectStatus> GetAllProjectStatuses();
    }
}
