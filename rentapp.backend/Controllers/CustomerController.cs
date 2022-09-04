using Microsoft.AspNetCore.Mvc;
using rentapp.BL.Dtos;
using rentapp.Service.Services.Interfaces;

namespace rentapp.backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet]
        [Route("get")]
        public List<CustomerItemDto> Get()
        {
            return _customerService.GetCustomers();
        }
    }
}
