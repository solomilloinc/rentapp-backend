using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using rentapp.BL.Dtos.Customer;
using rentapp.BL.Entities;
using rentapp.BL.Helpers;
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

        public async Task Delete(Customer obj)
        {
            _dataContext.Customers.Remove(obj);
            await _dataContext.SaveChangesAsync();
        }

        public async Task<Customer> GetById(int id, Func<IQueryable<Customer>, IIncludableQueryable<Customer, object>> includes = null)
        {
            IQueryable<Customer> query = _dataContext.Customers;

            if (includes != null)
            {
                query = includes(query);
            }

            return await query.SingleOrDefaultAsync(p => p.CustomerId == id);
        }

        public async Task<CustomerItemDto> GetItemDtoById(int id)
        {
            return await _dataContext.Customers.Where(p => p.CustomerId == id).Include(p => p.CustomerAddresses.Where(p => p.IsActive)).Select(p => new CustomerItemDto()
            {
                Addresses = p.CustomerAddresses.Select(p =>
                 AddressBuilder.GetCustomerAddress(p)).ToList(),
                LastName = p.LastName,
                Name = p.Name,
                DocumentNumber = p.DocumentNumber,
                DocumentTypeId = p.DocumentTypeId
            }).SingleOrDefaultAsync();
        }

        public async Task<List<CustomerItemDto>> GetCustomers()
        {
            return await _dataContext.Customers.Include(p => p.CustomerAddresses.Where(p => p.IsActive)).Select(p => new CustomerItemDto()
            {
                Addresses = p.CustomerAddresses.Select(p =>
                 AddressBuilder.GetCustomerAddress(p)).ToList(),
                LastName = p.LastName,
                Name = p.Name,
                DocumentNumber = p.DocumentNumber,
                DocumentTypeId = p.DocumentTypeId
            }).Take(10).ToListAsync();
        }

        public async Task Save(Customer obj)
        {
            if (obj.CustomerId == 0)
            {
                _dataContext.Customers.Add(obj);
            }
            else
            {
                _dataContext.Customers.Update(obj);
            }

            await _dataContext.SaveChangesAsync();
        }
    }
}
