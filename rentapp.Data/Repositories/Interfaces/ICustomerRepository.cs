using rentapp.BL.Dtos;

namespace rentapp.Data.Repositories.Interfaces
{
    public interface ICustomerRepository
    {
        List<CustomerItemDto> GetCustomers();
    }
}
