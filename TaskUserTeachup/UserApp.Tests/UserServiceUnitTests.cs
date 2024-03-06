using AutoMapper;
using FluentValidation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using UserApp.DataAccess.Interfaces;
using UserApp.Domain.Models;
using UserApp.DTOs.User;
using UserApp.Services.Implementation;
using UserApp.Services.Interfaces;

namespace UserApp.Tests
{
    [TestClass]
    public class UserServiceUnitTests
    {
        private readonly Mock<IRepository<User>> _repository;
        private readonly Mock<IUserRepository> _userRepository;
        private readonly Mock<ILoginHistoryRepository> _iLoginHistoryRepository;

        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly IUserHistoryService _userHistoryService;

        public UserServiceUnitTests()
        {
            _repository = new Mock<IRepository<User>>();
            _userRepository = new Mock<IUserRepository>();

            _iLoginHistoryRepository = new Mock<ILoginHistoryRepository>();

            _mapper = new Mock<IMapper>().Object;

            //_userHistoryService = new UserHistoryService(_iLoginHistoryRepository.Object, _mapper);

            _userService = new UserService(_userRepository.Object, _mapper, _userHistoryService);
        }


        [TestMethod]
        [ExpectedException(typeof(Exception), "User was not found")]
        public void GetUser_WhenUserDoesNotExist_ShouldThrowException()
        {
            // Arrange
            _userRepository.Setup(x => x.Get(It.IsAny<int>())).Returns((User)null);

            // Act
            _userService.GetUser(1);
        }

        [TestMethod]
        public void RegisterUser_ShouldCallRegisterMethodInUserRepository()
        {
            // Arrange
            RegisterUserDto registerUserDto = new RegisterUserDto()
            {
                FirstName = "Ljupche",
                LastName = "Joldashev",
                Username = "LJ",
                Password = "123456",
                ConfirmPassword = "123456",
                Email = "Ljupchejoldashev@yahoo.com"
            };

            // Act
            _userService.Register(registerUserDto);


            // Assert
            _userRepository.Verify(x => x.Register(It.IsAny<User>()), Times.Once, "Register method not called once.");
        }

        [TestMethod]
        public void LoginUser_ShouldReturnTokenWhenCredentialsAreCorrect()
        {
            // Arrange
            LoginUserDto loginUserDto = new LoginUserDto
            {
                Username = "LJ",
                Password = "123456"
            };

            User user = new User
            {
                Id = 1,
                Username = loginUserDto.Username,
                Password = "hashedPassword"
            };

            _userRepository.Setup(x => x.LoginUser(loginUserDto.Username, It.IsAny<string>())).Returns(user);

            // Act
            string resultToken = _userService.LoginUser(loginUserDto);

            // Assert
            Assert.IsNotNull(resultToken);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "User not found!")]
        public void LoginUser_ShouldThrowExceptionWhenUserNotFound()
        {
            // Arrange
            LoginUserDto loginUserDto = new LoginUserDto
            {
                Username = "LJ",
                Password = "123456"
            };

            _userRepository.Setup(x => x.LoginUser(loginUserDto.Username, It.IsAny<string>())).Returns((User)null);

            // Act & Assert
            _userService.LoginUser(loginUserDto);
        }

        [TestMethod]
        public void DeleteUser_ShouldDeleteUserWhenUserExists()
        {
            // Arrange
            int userIdToDelete = 1;

            User userToDelete = new User
            {
                Id = userIdToDelete,
                FirstName = "Ljupche",
            };

            _userRepository.Setup(x => x.Get(userIdToDelete)).Returns(userToDelete);

            // Act
            _userService.DeleteUser(userIdToDelete);

            // Assert
            _userRepository.Verify(x => x.Delete(userToDelete), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "User was not found.")]
        public void DeleteUser_ShouldThrowExceptionWhenUserNotFound()
        {
            // Arrange
            int userIdToDelete = 1;

            _userRepository.Setup(x => x.Get(userIdToDelete)).Returns((User)null);

            // Act & Assert
            _userService.DeleteUser(userIdToDelete);
        }

        [TestMethod]
        public void UpdateUser_ShouldUpdateUserWhenUserExists()
        {
            // Arrange
            UpdateUserDto updateUserDto = new UpdateUserDto
            {
                Id = 1,
                FirstName = "LjupcheUpdated",
                LastName = "JoldashevUpdated",
                Username = "LJUpdated",
                Email = "Ljupche@updated.com",
                Password = "123456updated"
            };

            User existingUser = new User
            {
                Id = updateUserDto.Id,
                FirstName = "Ljupche",
                LastName = "Joldashev",
                Username = "LJ",
                Email = "Ljupche@original.com",
                Password = "123456old"
            };

            _userRepository.Setup(x => x.Get(updateUserDto.Id)).Returns(existingUser);

            // Act
            _userService.UpdateUser(updateUserDto);

            // Assert
            _userRepository.Verify(x => x.Update(It.IsAny<User>()), Times.Once);
            Assert.AreEqual(updateUserDto.FirstName, existingUser.FirstName);
            Assert.AreEqual(updateUserDto.LastName, existingUser.LastName);
            Assert.AreEqual(updateUserDto.Username, existingUser.Username);
            Assert.AreEqual(updateUserDto.Email, existingUser.Email);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "User was not found.")]
        public void UpdateUser_ShouldThrowExceptionWhenUserNotFound()
        {
            // Arrange
            UpdateUserDto updateUserDto = new UpdateUserDto
            {
                Id = 1,
                FirstName = "Ljupche"
            };

            _userRepository.Setup(x => x.Get(updateUserDto.Id)).Returns((User)null);

            // Act & Assert
            _userService.UpdateUser(updateUserDto);
        }
    }
}

