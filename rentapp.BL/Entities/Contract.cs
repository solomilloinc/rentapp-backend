using System;
using System.Collections.Generic;

namespace rentapp.BL.Entities
{
    public partial class Contract
    {
        public Contract()
        {
            Chargets = new HashSet<Charget>();
            CustomerPayments = new HashSet<CustomerPayment>();
        }

        public int ContractId { get; set; }
        public int ContractTypeId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int PropertyId { get; set; }
        public string? Observation { get; set; }
        public string? ContractFilePath { get; set; }
        public int CustomerId { get; set; }
        public decimal? WarrantyAmount { get; set; }
        public decimal? IncreasePercentage { get; set; }
        public decimal? FeesAmount { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
        public int CreatedUserId { get; set; }
        public int? UpdatedUserId { get; set; }
        public bool? IsActive { get; set; }
        public int DirectoryId { get; set; }
        public int IsCompleted { get; set; }
        public int PeriodId { get; set; }
        public string? FilePath { get; set; }

        public virtual ContractType ContractType { get; set; } = null!;
        public virtual Customer Customer { get; set; } = null!;
        public virtual Directory Directory { get; set; } = null!;
        public virtual ContractStatus IsCompletedNavigation { get; set; } = null!;
        public virtual Period Period { get; set; } = null!;
        public virtual Property Property { get; set; } = null!;
        public virtual ICollection<Charget> Chargets { get; set; }
        public virtual ICollection<CustomerPayment> CustomerPayments { get; set; }
    }
}
