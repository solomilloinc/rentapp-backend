using System;
using System.Collections.Generic;

namespace rentapp.BL.Entities
{
    public partial class UserPermission
    {
        public UserPermission()
        {
            UserRoleUserPermissions = new HashSet<UserRoleUserPermission>();
            UserUserPermissions = new HashSet<UserUserPermission>();
        }

        public int UserPermissionId { get; set; }
        public string Name { get; set; } = null!;
        public DateTime DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
        public int CreatedUserId { get; set; }
        public int? UpdatedUserId { get; set; }
        public bool? IsActive { get; set; }

        public virtual ICollection<UserRoleUserPermission> UserRoleUserPermissions { get; set; }
        public virtual ICollection<UserUserPermission> UserUserPermissions { get; set; }
    }
}
