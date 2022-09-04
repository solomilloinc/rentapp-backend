using System;
using System.Collections.Generic;

namespace rentapp.BL.Entities
{
    public partial class CustomerPaymentFlow
    {
        public int CustomerPaymentFlowId { get; set; }
        public int? CustomerPaymentId { get; set; }
        public int CustomerId { get; set; }
        public decimal Income { get; set; }
        public decimal Outcome { get; set; }
        public string? Description { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
        public int CreatedUserId { get; set; }
        public int? UpdatedUserId { get; set; }
        public bool? IsActive { get; set; }
        public int DirectoryId { get; set; }

        public virtual Customer Customer { get; set; } = null!;
        public virtual CustomerPayment? CustomerPayment { get; set; }
    }
}
