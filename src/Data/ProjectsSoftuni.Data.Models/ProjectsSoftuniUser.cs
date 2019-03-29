// ReSharper disable VirtualMemberCallInConstructor
namespace ProjectsSoftuni.Data.Models
{
    using System;
    using System.Collections.Generic;

    using Microsoft.AspNetCore.Identity;
    using ProjectsSoftuni.Data.Common.Models;

    public class ProjectsSoftuniUser : IdentityUser, IAuditInfo, IDeletableEntity
    {
        public ProjectsSoftuniUser()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Roles = new HashSet<IdentityUserRole<string>>();
            this.Claims = new HashSet<IdentityUserClaim<string>>();
            this.Logins = new HashSet<IdentityUserLogin<string>>();

            this.Teams = new HashSet<TeamUser>();
        }

        // Audit info
        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        // Deletable entity
        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        public virtual ICollection<IdentityUserRole<string>> Roles { get; set; }

        public virtual ICollection<IdentityUserClaim<string>> Claims { get; set; }

        public virtual ICollection<IdentityUserLogin<string>> Logins { get; set; }

        // Application properties and methods
        public virtual ICollection<TeamUser> Teams { get; set; }
    }
}
