using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserApp.Domain.Models;

namespace UserApp.DataAccess.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        void Register(User user);
        User LoginUser(string username, string password);
        User GetUserByUsername(string username);
    }
}
