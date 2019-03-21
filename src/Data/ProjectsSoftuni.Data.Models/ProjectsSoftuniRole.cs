// ReSharper disable VirtualMemberCallInConstructor
namespace ProjectsSoftuni.Data.Models
{
    using System;

    using Microsoft.AspNetCore.Identity;
    using ProjectsSoftuni.Data.Common.Models;

    public class ProjectsSoftuniRole : IdentityRole, IAuditInfo, IDeletableEntity
    {
        public ProjectsSoftuniRole()
            : this(null)
        {
        }

        public ProjectsSoftuniRole(string name)
            : base(name)
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}
