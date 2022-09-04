using System;
using System.Collections.Generic;

namespace rentapp.BL.Entities
{
    public partial class UserRole
    {
        public UserRole()
        {
            UserRoleUserPermissions = new HashSet<UserRoleUserPermission>();
            UserUserRoles = new HashSet<UserUserRole>();
        }

        public int UserRoleId { get; set; }
        public string Name { get; set; } = null!;
        public DateTime DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
        public int CreatedUserId { get; set; }
        public int? UpdatedUserId { get; set; }
        public bool? IsActive { get; set; }

        public virtual ICollection<UserRoleUserPermission> UserRoleUserPermissions { get; set; }
        public virtual ICollection<UserUserRole> UserUserRoles { get; set; }
    }
}
