using System;
using System.Collections.Generic;

namespace rentapp.BL.Entities
{
    public partial class Customer
    {
        public Customer()
        {
            Contracts = new HashSet<Contract>();
            CustomerPaymentFlows = new HashSet<CustomerPaymentFlow>();
            CustomerPayments = new HashSet<CustomerPayment>();
            Properties = new HashSet<Property>();
            CustomerAddresses = new HashSet<CustomerAddress>();
        }

        public int CustomerId { get; set; }        
        public int DocumentTypeId { get; set; }
        public string DocumentNumber { get; set; }
        public string Name { get; set; } = null!;
        public string? LastName { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
        public int CreatedUserId { get; set; }
        public int? UpdatedUserId { get; set; }
        public bool? IsActive { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public DateTime? Birthdate { get; set; }
        public string? PhoneNumber2 { get; set; }

        public virtual DocumentType? DocumentType { get; set; }
        public virtual User User { get; set; } = null!;
        public virtual ICollection<Contract> Contracts { get; set; }
        public virtual ICollection<CustomerPaymentFlow> CustomerPaymentFlows { get; set; }
        public virtual ICollection<CustomerPayment> CustomerPayments { get; set; }
        public virtual ICollection<Property> Properties { get; set; }
        public virtual ICollection<CustomerAddress> CustomerAddresses { get; set; }
    }
}
