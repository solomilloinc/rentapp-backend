using Microsoft.EntityFrameworkCore.Query;
using rentapp.BL.Dtos.Customer;
using rentapp.BL.Entities;

namespace rentapp.Data.Repositories.Interfaces
{
    public interface ICustomerRepository
    {
        Task<List<CustomerItemDto>> GetCustomers();
        Task<CustomerItemDto> GetItemDtoById(int id);
        Task<Customer> GetById(int id, Func<IQueryable<Customer>, IIncludableQueryable<Customer, object>> includes = null);
        Task Save(Customer obj);
        Task Delete(Customer obj);
    }
}
