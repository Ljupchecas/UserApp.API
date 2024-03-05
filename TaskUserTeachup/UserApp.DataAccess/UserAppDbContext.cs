using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserApp.Domain.Models;

namespace UserApp.DataAccess
{
    public class UserAppDbContext : DbContext
    {
        public UserAppDbContext(DbContextOptions options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<LoginHistory> LoginHistory { get; set; }
    }
}
