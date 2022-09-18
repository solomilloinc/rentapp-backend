using rentapp.BL.Dtos.Customer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rentapp.Service.Services.Interfaces
{
    public interface ICustomerService: IBaseService
    {
        Task<List<CustomerItemDto>> GetCustomers();
        Task<CustomerItemDto> GetById(int id);
        Task SaveCustomer(CustomerDto dto);
        Task<CustomerDto> Delete(int id);
    }
}
