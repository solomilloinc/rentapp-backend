using System;
using System.Collections.Generic;

namespace rentapp.BL.Entities
{
    public partial class User
    {
        public User()
        {
            UserUserPermissions = new HashSet<UserUserPermission>();
            UserUserRoles = new HashSet<UserUserRole>();
        }

        public int UserId { get; set; }
        public string UserName { get; set; } = null!;
        public string Password { get; set; } = null!;
        public int? CustomerId { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
        public int CreatedUserId { get; set; }
        public int? UpdatedUserId { get; set; }
        public bool? IsActive { get; set; }
        public int? DirectoryId { get; set; }

        public virtual Directory? Directory { get; set; }
        public virtual Customer UserNavigation { get; set; } = null!;
        public virtual ICollection<UserUserPermission> UserUserPermissions { get; set; }
        public virtual ICollection<UserUserRole> UserUserRoles { get; set; }
    }
}
