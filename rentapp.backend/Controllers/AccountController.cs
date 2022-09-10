using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using rentapp.BL.Dtos;
using rentapp.BL.Dtos.Auth;
using rentapp.Service.Services.Interfaces;

namespace rentapp.backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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
            var response = _userService.Authenticate(auth);
            SetTokenCookie(response.RefreshToken);
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost("refresh-token")]
        public IActionResult RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            var response = _userService.RefreshToken(refreshToken);
            SetTokenCookie(response.RefreshToken);
            return Ok(response);
        }

        [HttpPost]
        [Route("revoke-token")]
        public IActionResult RevokeToken(RevokeTokenRequestDto model)
        {
            // accept refresh token in request body or cookie
            var token = model.Token ?? Request.Cookies["refreshToken"];

            if (string.IsNullOrEmpty(token))
                return BadRequest(new { message = "Token is required" });

            _userService.RevokeToken(token);
            return Ok(new { message = "Token revoked" });
        }

        //Helper Methods
        private void SetTokenCookie(string token)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(7)
            };
            Response.Cookies.Append("refreshToken", token, cookieOptions);
        }
    }
}
