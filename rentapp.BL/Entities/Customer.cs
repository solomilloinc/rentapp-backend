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
        }

        public int CustomerId { get; set; }
        public string Street { get; set; } = null!;
        public string PlainStreet { get; set; } = null!;
        public string Number { get; set; } = null!;
        public string? Floor { get; set; }
        public string? Unit { get; set; }
        public string ZipCode { get; set; } = null!;
        public string PlainZipCode { get; set; } = null!;
        public string City { get; set; } = null!;
        public string PlainCity { get; set; } = null!;
        public string State { get; set; } = null!;
        public string PlainState { get; set; } = null!;
        public string Country { get; set; } = null!;
        public string PlainCountry { get; set; } = null!;
        public string? AdjacentStreet1 { get; set; }
        public string? PlainAdjacentStreet1 { get; set; }
        public string? AdjacentStreet2 { get; set; }
        public string? PlainAdjacentStreet2 { get; set; }
        public double? Lat { get; set; }
        public double? Lng { get; set; }
        public int? DocumentTypeId { get; set; }
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
        public int DirectoryId { get; set; }

        public virtual DocumentType? DocumentType { get; set; }
        public virtual User User { get; set; } = null!;
        public virtual ICollection<Contract> Contracts { get; set; }
        public virtual ICollection<CustomerPaymentFlow> CustomerPaymentFlows { get; set; }
        public virtual ICollection<CustomerPayment> CustomerPayments { get; set; }
        public virtual ICollection<Property> Properties { get; set; }
    }
}
