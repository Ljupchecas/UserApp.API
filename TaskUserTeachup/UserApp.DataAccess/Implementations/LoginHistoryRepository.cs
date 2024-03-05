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
    public class LoginHistoryRepository : ILoginHistoryRepository
    {
        private readonly UserAppDbContext _context;
        public LoginHistoryRepository(UserAppDbContext context)
        {
            _context = context;
        }

        public List<LoginHistory> GetUserLoginHistory(string username)
        {
            return _context.LoginHistory.Where(history => history.Username == username).ToList();
        }

        public void SaveLoginHistory(LoginHistory loginHistory)
        {
            _context.LoginHistory.Add(loginHistory);
            _context.SaveChanges();
        }
    }
}
