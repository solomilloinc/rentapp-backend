using rentapp.BL.Entities;
using rentapp.Data.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rentapp.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _dataContext;

        public UserRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public List<User> GetAll()
        {
            return _dataContext.Users.ToList();
        }

        public User GetById(int id)
        {
            return _dataContext.Users.SingleOrDefault(p => p.UserId == id);
        }

        public User GetByUserName(string name)
        {
            return _dataContext.Users.SingleOrDefault(p => p.UserName == name);
        }

        public User GetUserByToken(string token)
        {
           return _dataContext.Users.SingleOrDefault(u => u.RefreshTokens.Any(t => t.Token == token));
        }

        public void SaveUser(User user)
        {
            if (user.UserId == 0)
            {
                _dataContext.Users.Add(user);
            }
            else
            {
                _dataContext.Users.Update(user);
            }

            _dataContext.SaveChanges();
        }
    }
}
