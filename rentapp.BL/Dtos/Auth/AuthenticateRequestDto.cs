using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rentapp.BL.Dtos.Auth
{
    public class AuthenticateRequestDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
