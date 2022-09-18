using rentapp.BL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace rentapp.BL.Dtos.Auth
{
    public class AuthenticateResponseDto
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string JwtToken { get; set; }

        [JsonIgnore] // refresh token is returned in http only cookie
        public string RefreshToken { get; set; }

        public AuthenticateResponseDto(User user, string jwtToken, string refreshToken)
        {
            Id = user.UserId;
            UserName = user.Email;
            JwtToken = jwtToken;
            RefreshToken = refreshToken; 
        }
    }
}
