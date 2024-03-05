using AutoMapper;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserApp.DataAccess.Implementations;
using UserApp.DataAccess.Interfaces;
using UserApp.Domain.Models;
using UserApp.DTOs.User;
using UserApp.DTOs.UserValidators;
using UserApp.Services.Interfaces;

namespace UserApp.Services.Implementation
{
    public class UserHistoryService : IUserHistoryService
    {
        private readonly ILoginHistoryRepository _loginHistoryRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        private readonly LoginHistoryDtoValidator _loginHistoryValidator;
        public UserHistoryService(ILoginHistoryRepository loginHistoryRepository, 
                                  IMapper mapper, 
                                  IUserRepository userRepository)
        {
            _loginHistoryRepository = loginHistoryRepository;
            _mapper = mapper;
            _userRepository = userRepository;

            _loginHistoryValidator = new LoginHistoryDtoValidator();

        }

        #region Add Login History
        public void AddLoginHistory(string username)
        {
            // Creating an object of the LoginHistory class and setting values
            var loginHistory = new LoginHistory
                {
                    Username = username,
                    LoginDate = DateTime.Now,
                };

            // Saving login history using the repository
            _loginHistoryRepository.SaveLoginHistory(loginHistory);
        }
        #endregion

        #region Get History By User
        public List<LoginHistoryDto> GetHistoryByUser(string username)
        {
            // Creating a LoginHistoryDto object with the provided username
            var loginHistoryDto = new LoginHistoryDto { Username = username };

            // Validating the login history DTO
            _loginHistoryValidator.Validate(loginHistoryDto);

            // Retrieving the user by username
            var user = _userRepository.GetUserByUsername(username);

            // Checking if the user exists
            _ = user ?? throw new Exception("User was not found");

            // Retrieving login histories for the user
            var loginHistories = _loginHistoryRepository.GetUserLoginHistory(username);

            // Mapping login histories to DTOs
            var loginHistoryDtos = _mapper.Map<List<LoginHistoryDto>>(loginHistories);

            return loginHistoryDtos;
        }
        #endregion

    }
}
