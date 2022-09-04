using System;
using System.Collections.Generic;

namespace rentapp.BL.Entities
{
    public partial class Period
    {
        public Period()
        {
            Contracts = new HashSet<Contract>();
        }

        public int PeriodId { get; set; }
        public string Name { get; set; } = null!;

        public virtual ICollection<Contract> Contracts { get; set; }
    }
}
