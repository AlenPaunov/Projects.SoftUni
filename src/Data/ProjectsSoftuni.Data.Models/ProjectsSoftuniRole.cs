// ReSharper disable VirtualMemberCallInConstructor
namespace ProjectsSoftuni.Data.Models
{
    using System;
    using System.Collections.Generic;

    using Microsoft.AspNetCore.Identity;
    using ProjectsSoftuni.Data.Common.Models;

    public class ProjectsSoftuniRole : IdentityRole, IAuditInfo, IDeletableEntity
    {
        public ProjectsSoftuniRole()
            : this(null)
        {
            this.Users = new HashSet<IdentityUserRole<string>>();
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

        public virtual ICollection<IdentityUserRole<string>> Users { get; set; }
    }
}
