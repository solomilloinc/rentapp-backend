using System;
using System.Collections.Generic;

namespace rentapp.BL.Entities
{
    public partial class Directory
    {
        public Directory()
        {
            Contracts = new HashSet<Contract>();
            Expenses = new HashSet<Expense>();
            Properties = new HashSet<Property>();
            Users = new HashSet<User>();
        }

        public int DirectoryId { get; set; }
        public string Name { get; set; } = null!;
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
        public DateTime DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
        public int CreatedUserId { get; set; }
        public int? UpdatedUserId { get; set; }
        public bool? IsActive { get; set; }
        public string? LogoFilePath { get; set; }
        public string? Observation { get; set; }
        public double? Lat { get; set; }
        public double? Lng { get; set; }

        public virtual ICollection<Contract> Contracts { get; set; }
        public virtual ICollection<Expense> Expenses { get; set; }
        public virtual ICollection<Property> Properties { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}
