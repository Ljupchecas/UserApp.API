using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserApp.DTOs.User
{
    public class LoginHistoryDto
    {
        public string Username { get; set; }

        public DateTime LoginDate { get; set; }
    }
}
