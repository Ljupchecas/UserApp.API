using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserApp.DataAccess.Interfaces;
using UserApp.Domain.Models;

namespace UserApp.DataAccess.Implementations
{
    public class UserRepository : IUserRepository
    {
        private readonly UserAppDbContext _context;
        public UserRepository(UserAppDbContext context)
        {
            _context = context;
        }

        public void Delete(User entity)
        {
            _context.Users.Remove(entity);
            _context.SaveChanges();
        }

        public User Get(int id)
        {
            return _context.Users.FirstOrDefault(x => x.Id == id);
        }

        public List<LoginHistory> GetAll()
        {
            return _context.LoginHistory.ToList();
        }

        public User GetUserByUsername(string username)
        {
            var user = _context.Users.FirstOrDefault(x => x.Username.ToLower() == username.ToLower());

            return user;
        }

        public User LoginUser(string username, string password)
        {
            var user = _context.Users.FirstOrDefault(
                x => x.Username.ToLower() == username.ToLower() && 
                x.Password == password);

            return user;
        }

        public void Register(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public void Update(User entity)
        {
            _context.Users.Update(entity);
            _context.SaveChanges();
        }
    }
}
