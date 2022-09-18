using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace rentapp.BL.Dtos.Customer
{
    public class CustomerAddressDto
    {
        public int CustomerAddressId { get; set; }
        public string Street { get; set; }
        public string Number { get; set; }
        public string? Floor { get; set; }
        public string? Unit { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string? AdjacentStreet1 { get; set; }
        public string? AdjacentStreet2 { get; set; }
        public double? Lat { get; set; }
        public double? Lng { get; set; }
    }

    public class CustomerAddressValidator : AbstractValidator<CustomerAddressDto>
    {
        public CustomerAddressValidator()
        {
            RuleFor(p => p.Street)
                .NotNull().WithMessage("La calle de la dirección es requerido")
                .NotEmpty().WithMessage("La calle de la dirección es requerido")
                .MaximumLength(100).WithMessage("La calle de la dirección no puede superar los 100 caracteres");
            RuleFor(p => p.Number)
                .NotNull().WithMessage("El número de la calle de la dirección es requerido")
                .NotEmpty().WithMessage("El número de la calle de la dirección es requerido")
                .MaximumLength(50).WithMessage("El número no puede superar los 50 caracteres");
            RuleFor(p => p.ZipCode)
                .NotNull().WithMessage("El cod postal es requerido")
                .NotEmpty().WithMessage("El cod postal es requerido")
                .MaximumLength(50).WithMessage("El cod postal no puede superar los 50 caracteres");
            RuleFor(p => p.City)
                .NotNull().WithMessage("La ciudad es requerido")
                .NotEmpty().WithMessage("La ciudad es requerido")
                .MaximumLength(50).WithMessage("La ciudad no puede superar los 50 caracteres");
            RuleFor(p => p.State)
                .NotNull().WithMessage("La provincia es requerido")
                .NotEmpty().WithMessage("La provincia es requerido")
                .MaximumLength(50).WithMessage("La provincia no puede superar los 50 caracteres");
            RuleFor(p => p.Country)
                .NotNull().WithMessage("El país es requerido")
                .NotEmpty().WithMessage("El país es requerido")
                .MaximumLength(50).WithMessage("El país no puede superar los 50 caracteres");
        }
    }
}
