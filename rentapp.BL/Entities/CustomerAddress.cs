using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rentapp.BL.Entities
{
    public class CustomerAddress
    {
        public int CustomerAddressId { get; set; }
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
        public DateTime DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
        public int CreatedUserId { get; set; }
        public int? UpdatedUserId { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
