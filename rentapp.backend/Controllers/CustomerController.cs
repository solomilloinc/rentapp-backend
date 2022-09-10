using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using rentapp.BL.Dtos;
using rentapp.Service.Services.Interfaces;

namespace rentapp.backend.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        
        [HttpGet]
        [Route("getCustomers")]
        public IActionResult GetCustomers()
        {
            return Ok(_customerService.GetCustomers());
        }
    }
}
