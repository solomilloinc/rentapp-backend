using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using rentapp.BL.Dtos;
using rentapp.BL.Dtos.Auth;
using rentapp.Service.Services.Interfaces;

namespace rentapp.backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IUserService _userService;

        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        [Route("login")]
        public IActionResult login(AuthenticateRequestDto auth)
        {
            return Ok(_userService.Authenticate(auth));
        }
    }
}
