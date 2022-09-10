using rentapp.BL.Entities;
using rentapp.Service.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rentapp.Service.Services
{
    public abstract class BaseService : IBaseService
    {
        protected User user;

        public virtual void SetCurrentUser(User user)
        {
            this.user = user;
        }
    }
}
