using System;
using System.Collections.Generic;

namespace rentapp.BL.Entities
{
    public partial class Property
    {
        public Property()
        {
            Contracts = new HashSet<Contract>();
            PictureProperties = new HashSet<PictureProperty>();
        }

        public int PropertyId { get; set; }
        public int PropertyTypeId { get; set; }
        public int ConditionId { get; set; }
        public string? Description { get; set; }
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
        public int? BathroomQuantity { get; set; }
        public int? RoomQuantity { get; set; }
        public int? BedroomQuantity { get; set; }
        public int? GaragesQuantity { get; set; }
        public int? Antiquity { get; set; }
        public bool IsCommercial { get; set; }
        public int? SubPropertyTypeId { get; set; }
        public decimal? Price { get; set; }
        public decimal? ExpensesPrice { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
        public int CustomerId { get; set; }
        public int CreatedUserId { get; set; }
        public int? UpdatedUserId { get; set; }
        public bool? IsActive { get; set; }
        public int DirectoryId { get; set; }
        public int PropertyStatusId { get; set; }
        public double? Lat { get; set; }
        public double? Lng { get; set; }
        public int CurrencyId { get; set; }

        public virtual Condition Condition { get; set; } = null!;
        public virtual Currency Currency { get; set; } = null!;
        public virtual Customer Customer { get; set; } = null!;
        public virtual Directory Directory { get; set; } = null!;
        public virtual PropertyStatus PropertyStatus { get; set; } = null!;
        public virtual SubPropertyType? SubPropertyType { get; set; }
        public virtual ICollection<Contract> Contracts { get; set; }
        public virtual ICollection<PictureProperty> PictureProperties { get; set; }
    }
}
