using System;
using System.Collections.Generic;

namespace rentapp.BL.Entities
{
    public partial class SubPropertyType
    {
        public SubPropertyType()
        {
            Properties = new HashSet<Property>();
        }

        public int SubPropertyTypeId { get; set; }
        public string Name { get; set; } = null!;
        public DateTime DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
        public int CreatedUserId { get; set; }
        public int? UpdatedUserId { get; set; }
        public bool? IsActive { get; set; }

        public virtual ICollection<Property> Properties { get; set; }
    }
}
