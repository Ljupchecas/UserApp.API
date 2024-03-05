using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserApp.DataAccess;
using UserApp.DataAccess.Implementations;
using UserApp.DataAccess.Interfaces;
using UserApp.DTOs.User;
using UserApp.DTOs.UserValidators;
using UserApp.Services.Implementation;
using UserApp.Services.Interfaces;

namespace UserApp.Helpers.DIHelper
{
    public static class DependencyInjectionHelper
    {
        public static void InjectDbContext(this IServiceCollection services)
        {
            services.AddDbContext<UserAppDbContext>(x => x.UseSqlServer
            ("Server=.\\SQLExpress;Database = TeachupUsersApp;Trusted_Connection=True; TrustServerCertificate = True;"));
        }

        public static void InjectRepositories(this IServiceCollection services)
        {
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<ILoginHistoryRepository, LoginHistoryRepository>();
        }

        public static void InjectServices(this IServiceCollection services)
        {
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IUserHistoryService, UserHistoryService>();
        }
    }
}
