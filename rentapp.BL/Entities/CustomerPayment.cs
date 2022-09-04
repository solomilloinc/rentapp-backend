using System;
using System.Collections.Generic;

namespace rentapp.BL.Entities
{
    public partial class CustomerPayment
    {
        public CustomerPayment()
        {
            Chargets = new HashSet<Charget>();
            CustomerPaymentCards = new HashSet<CustomerPaymentCard>();
            CustomerPaymentCashes = new HashSet<CustomerPaymentCash>();
            CustomerPaymentFlows = new HashSet<CustomerPaymentFlow>();
        }

        public int CustomerPaymentId { get; set; }
        public decimal Total { get; set; }
        public int CustomerId { get; set; }
        public DateTime PaymentDate { get; set; }
        public string? Observation { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
        public int CreatedUserId { get; set; }
        public int? UpdatedUserId { get; set; }
        public bool? IsActive { get; set; }
        public decimal? TotalPaid { get; set; }
        public int ContractId { get; set; }
        public int DirectoryId { get; set; }

        public virtual Contract Contract { get; set; } = null!;
        public virtual Customer Customer { get; set; } = null!;
        public virtual ICollection<Charget> Chargets { get; set; }
        public virtual ICollection<CustomerPaymentCard> CustomerPaymentCards { get; set; }
        public virtual ICollection<CustomerPaymentCash> CustomerPaymentCashes { get; set; }
        public virtual ICollection<CustomerPaymentFlow> CustomerPaymentFlows { get; set; }
    }
}
