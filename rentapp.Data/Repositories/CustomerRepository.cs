using rentapp.BL.Dtos;
using rentapp.Data.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rentapp.Data.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly DataContext _dataContext;

        public CustomerRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public List<CustomerItemDto> GetCustomers()
        {
            return _dataContext.Customers.Select(p => new CustomerItemDto()
            {
                Address = $"{p.Street}, {p.City}, {p.State}, {p.Country}",
                LastName = p.LastName,
                Name = p.Name
            }).Take(10).ToList();
        }
    }
}
