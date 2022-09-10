using rentapp.BL.Dtos;
using rentapp.Data.Repositories.Interfaces;
using rentapp.Service.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rentapp.Service.Services
{
    public class CustomerService : BaseService, ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerService(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public List<CustomerItemDto> GetCustomers()
        {
            return _customerRepository.GetCustomers();
        }
    }
}
