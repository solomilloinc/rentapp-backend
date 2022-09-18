using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rentapp.BL.Dtos.Customer
{
    public class CustomerDto
    {
        public int CustomerId { get; set; }
        public int? DocumentTypeId { get; set; }
        public string DocumentNumber { get; set; }
        public string Name { get; set; }
        public string? LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public DateTime? Birthdate { get; set; }
        public string? PhoneNumber2 { get; set; }
        public List<CustomerAddressDto> CustomerAddresses { get; set; }
    }

    public class CustomerValidator : AbstractValidator<CustomerDto>
    {
        public CustomerValidator()
        {
            RuleFor(p => p.DocumentNumber)
                .NotNull().WithMessage("El número de documento es requerido")
                .NotEmpty().WithMessage("El número de documento es requerido")
                .Length(8, 50).WithMessage("El número de documento debe estar entre 8 y 11");
            RuleFor(p => p.DocumentTypeId)
                .NotNull().WithMessage("El tipo de documento es requerido");
            RuleFor(p => p.Name)
                .NotNull().WithMessage("El nombre es requerido")
                .NotEmpty().WithMessage("El nombre es requerido")
                .Length(3, 50).WithMessage("El nombre debe estar entre 3 y 50 caracteres");
            RuleFor(p => p.LastName)
                .Length(3, 50).WithMessage("El nombre debe estar entre 3 y 50 caracteres");

            RuleForEach(x => x.CustomerAddresses).SetValidator(new CustomerAddressValidator());
        }
    }
}
