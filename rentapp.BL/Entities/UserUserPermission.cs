using System;
using System.Collections.Generic;

namespace rentapp.BL.Entities
{
    public partial class UserUserPermission
    {
        public int UserUserPermissionId { get; set; }
        public int UserId { get; set; }
        public int UserPermissionId { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
        public int CreatedUserId { get; set; }
        public int? UpdatedUserId { get; set; }
        public bool? IsActive { get; set; }

        public virtual User User { get; set; } = null!;
        public virtual UserPermission UserPermission { get; set; } = null!;
    }
}
