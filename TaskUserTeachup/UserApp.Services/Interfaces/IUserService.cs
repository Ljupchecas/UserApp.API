using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserApp.Domain.Models;
using UserApp.DTOs.User;

namespace UserApp.Services.Interfaces
{
    public interface IUserService
    {
        void Register(RegisterUserDto registerUserDto);
        string LoginUser(LoginUserDto loginUserDto);
        UserDto GetUser(int id);
        void UpdateUser(UpdateUserDto updateUserDto);
        void DeleteUser(int id);
    }
}
