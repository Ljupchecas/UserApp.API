using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserApp.DTOs.User;

namespace UserApp.Services.Interfaces
{
    public interface IUserHistoryService
    {
        List<LoginHistoryDto> GetHistoryByUser(string username);
        public void AddLoginHistory(string username);
    }
}
