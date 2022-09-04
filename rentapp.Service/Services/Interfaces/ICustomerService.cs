using rentapp.BL.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rentapp.Service.Services.Interfaces
{
    public interface ICustomerService
    {
        List<CustomerItemDto> GetCustomers();
    }
}
