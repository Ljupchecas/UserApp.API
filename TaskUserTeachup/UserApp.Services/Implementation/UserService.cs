using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using UserApp.DataAccess.Interfaces;
using UserApp.Domain.Models;
using UserApp.DTOs.User;
using UserApp.DTOs.UserValidators;
using UserApp.Services.Interfaces;
using XSystem.Security.Cryptography;

namespace UserApp.Services.Implementation
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        private readonly IUserHistoryService _userHistoryService;

        private readonly RegisterUserDtoValidator _registerUserValidator;
        private readonly UpdateUserDtoValidator _updateUserValidator;
        private readonly LoginUserDtoValidator _loginUserValidator;

        #region Constructor
        public UserService
            (
            IUserRepository userRepository, 
            IMapper mapper,
            IUserHistoryService userHistoryService
            )
        {
            _userRepository = userRepository;
            _userHistoryService = userHistoryService;
            _mapper = mapper;
            _registerUserValidator = new RegisterUserDtoValidator();
            _updateUserValidator = new UpdateUserDtoValidator();
            _loginUserValidator = new LoginUserDtoValidator();
        }
        #endregion


        #region Delete User
        public void DeleteUser(int id) =>
        _userRepository.Delete(_userRepository.Get(id) ?? throw new Exception("User was not found"));
        #endregion

        #region Get User
        public UserDto GetUser(int id) =>
        _mapper.Map<UserDto>(_userRepository.Get(id) ?? throw new Exception("User was not found"));
        #endregion

        #region Update User
        public void UpdateUser(UpdateUserDto updateUserDto)
        {
            // Validate the updateUserDto using the updateUserValidator
            _updateUserValidator.Validate(updateUserDto);

            // Retrieve the user from the repository based on the provided id
            User userDb = _userRepository.Get(updateUserDto.Id) ?? throw new Exception("User was not found.");

            // Map the properties from updateUserDto to userDb
            _mapper.Map(updateUserDto, userDb);

            // Hash the password if it's changed
            if (!string.IsNullOrEmpty(updateUserDto.Password))
                userDb.Password = HashPassword(updateUserDto.Password);

            // Update the user in the repository
            _userRepository.Update(userDb);
        }
        #endregion

        #region Register
        public void Register(RegisterUserDto registerUserDto)
        {
            _registerUserValidator.Validate(registerUserDto);

            // Map the properties from RegisterUserDto to User using AutoMapper
            var user = _mapper.Map<User>(registerUserDto);

            // Hash the password from registerUserDto
            user.Password = HashPassword(registerUserDto.Password);

            // Register the user in the repository
            _userRepository.Register(user);
        }
        #endregion

        #region Login User
        public string LoginUser(LoginUserDto loginUserDto)
        {
            // Validate the loginUserDto using the loginUserValidator
            _loginUserValidator.Validate(loginUserDto);

            // Hash the password from loginUserDto
            var hashedPassword = HashPassword(loginUserDto.Password);

            // Try to login user with the provided username and hashed password
            User userDb = _userRepository.LoginUser(loginUserDto.Username, hashedPassword);

            // If user not found, throw an exception
            if (userDb == null)
                throw new Exception("User not found!");

            // Generate symmetric security key for token signing
            var secret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("My secret code must be very long"));

            // Create signing credentials using the secret and HmacSha256 algorithm
            var credentials = new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);

            // Define claims for the user
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, userDb.Id.ToString()),
                new Claim(ClaimTypes.Name, userDb.Username),
            };

            // Create JWT token with specified claims, expiry time, and signing credentials
            var token = new JwtSecurityToken("http://localhost:5043",
                                             "http://localhost:5043",
                                             claims: claims,
                                             expires: DateTime.Now.AddHours(3),
                                             signingCredentials: credentials);

            // Add login history for the user
            _userHistoryService.AddLoginHistory(loginUserDto.Username);

            // Return the JWT token as a string
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        #endregion

        #region Hash Password
        private string HashPassword(string password)
        {
            var data = ASCIIEncoding.ASCII.GetBytes(password);
            var shaProvider = new SHA1CryptoServiceProvider();
            return ASCIIEncoding.ASCII.GetString(shaProvider.ComputeHash(data));
        }
        #endregion


    }
}
