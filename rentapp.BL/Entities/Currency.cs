using System;
using System.Collections.Generic;

namespace rentapp.BL.Entities
{
    public partial class Currency
    {
        public Currency()
        {
            Properties = new HashSet<Property>();
        }

        public int CurrencyId { get; set; }
        public string Name { get; set; } = null!;
        public decimal? Rate { get; set; }
        public bool IsMain { get; set; }
        public string Symbol { get; set; } = null!;
        public DateTime DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
        public int CreatedUserId { get; set; }
        public int? UpdatedUserId { get; set; }
        public bool? IsActive { get; set; }

        public virtual ICollection<Property> Properties { get; set; }
    }
}
