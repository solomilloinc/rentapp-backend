using rentapp.BL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rentapp.Data.Repositories.Interfaces
{
    public interface IUserRepository
    {
        User GetByUserName(string userName);
        List<User> GetAll();
        User GetById(int id);
        void SaveUser(User user);
        User GetUserByToken(string token);
    }
}
