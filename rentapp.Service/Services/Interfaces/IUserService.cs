using rentapp.BL.Dtos.Auth;
using rentapp.BL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rentapp.Service.Services.Interfaces
{
    public interface IUserService
    {
        AuthenticateResponseDto Authenticate(AuthenticateRequestDto model);
        AuthenticateResponseDto RefreshToken(string token);
        void RevokeToken(string token, string ipAddress);
        IEnumerable<User> GetAll();
        User GetById(int id);
    }
}
