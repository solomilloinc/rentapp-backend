using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using rentapp.BL.Dtos.Customer;
using rentapp.BL.Entities;
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
        [Route("get")]
        public async Task<IActionResult> Get()
        {
            return Ok(await _customerService.GetCustomers());
        }

        [HttpGet]
        [Route("getById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok(await _customerService.GetById(id));
        }

        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> Add([FromBody] CustomerDto dto)
        {
            _customerService.SetCurrentUser((User)HttpContext.Items["User"]);
            await _customerService.SaveCustomer(dto);

            return Ok(dto);
        }

        [HttpPut]
        [Route("update/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CustomerDto dto)
        {
            dto.CustomerId = id;
            _customerService.SetCurrentUser((User)HttpContext.Items["User"]);
            await _customerService.SaveCustomer(dto);

            return Ok(dto);
        }

        [HttpDelete]
        [Route("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            _customerService.SetCurrentUser((User)HttpContext.Items["User"]);
           var dto = await _customerService.Delete(id);

            return Ok(dto);
        }
    }
}
