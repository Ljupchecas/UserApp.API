using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserApp.Domain.Models;

namespace UserApp.DataAccess.Interfaces
{
    public interface ILoginHistoryRepository
    {
        List<LoginHistory> GetUserLoginHistory(string username);
        void SaveLoginHistory(LoginHistory loginHistory);
    }
}
