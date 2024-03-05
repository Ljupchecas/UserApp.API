using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserApp.DataAccess.Interfaces;
using UserApp.Domain.Models;
using UserApp.Services.Implementation;
using UserApp.Services.Interfaces;
using UserApp.Tests.FakeRepositories;

namespace UserApp.Tests
{
    [TestClass]
    public class UserServiceIntegrationTests
    {

        [TestMethod]
        public void Get_ShouldReturn_User()
        {
            // Arrange
            FakeUserRepository userRepository = new FakeUserRepository();
            int userIdToRetrieve = 1;

            // Act
            User retrievedUser = userRepository.Get(userIdToRetrieve);

            // Assert
            Assert.IsNotNull(retrievedUser);
            Assert.AreEqual(userIdToRetrieve, retrievedUser.Id);
        }

        [TestMethod]
        public void Get_ShouldReturnNullWhenUserDoesNotExist()
        {
            // Arrange
            FakeUserRepository userRepository = new FakeUserRepository();
            int nonExistingUserId = 999;

            // Act
            User retrievedUser = userRepository.Get(nonExistingUserId);

            // Assert
            Assert.IsNull(retrievedUser);
        }

        [TestMethod]
        public void GetUserByUsername_ShouldReturnUserWhenExists()
        {
            // Arrange
            FakeUserRepository userRepository = new FakeUserRepository();
            string existingUsername = "LJ";

            // Act
            User retrievedUser = userRepository.GetUserByUsername(existingUsername);

            // Assert
            Assert.IsNotNull(retrievedUser);
            Assert.AreEqual(existingUsername, retrievedUser.Username);
        }

        [TestMethod]
        public void GetUserByUsername_ShouldReturnNullWhenUserDoesNotExist()
        {
            // Arrange
            FakeUserRepository userRepository = new FakeUserRepository();
            string nonExistingUsername = "Kiko";

            // Act
            User retrievedUser = userRepository.GetUserByUsername(nonExistingUsername);

            // Assert
            Assert.IsNull(retrievedUser);
        }

        [TestMethod]
        public void Register_ShouldAddUserToRepository()
        {
            // Arrange
            FakeUserRepository userRepository = new FakeUserRepository();
            User newUser = new User
            {
                Id = 3,
                FirstName = "Kire",
                LastName = "Joldashev",
                Username = "KJ",
                Password = "123456",
                Email = "Kire@yahoo.com",
            };

            // Act
            userRepository.Register(newUser);
            User retrievedUser = userRepository.Get(newUser.Id);

            // Assert
            Assert.IsNotNull(retrievedUser);
            Assert.AreEqual(newUser, retrievedUser);
        }

        [TestMethod]
        public void Delete_ShouldRemoveUserFromRepository()
        {
            // Arrange
            FakeUserRepository userRepository = new FakeUserRepository();
            int userIdToDelete = 1;
            User userToDelete = userRepository.Get(userIdToDelete);

            // Act
            userRepository.Delete(userToDelete);
            User retrievedUser = userRepository.Get(userIdToDelete);

            // Assert
            Assert.IsNull(retrievedUser);
        }

        [TestMethod]
        public void Update_ShouldUpdateUserInRepository()
        {
            // Arrange
            FakeUserRepository userRepository = new FakeUserRepository();
            int userIdToUpdate = new Random().Next(10000, 99999);

            User userToUpdate = new User
            {
                Id = userIdToUpdate,
                FirstName = "Tale",
                LastName = "Nestorov",
                Username = "TN",
                Password = "123456",
                Email = "Tale@yahoo.com"
            };

            userRepository.Register(userToUpdate);

            // Act
            userToUpdate.FirstName = "Tale123";
            userToUpdate.LastName = "Nestorov123";
            userToUpdate.Username = "NK123";
            userToUpdate.Password = "1234567890";
            userToUpdate.Email = "Tale123@yahoo.com";

            userRepository.Update(userToUpdate);

            User updatedUser = userRepository.Get(userIdToUpdate);

            // Assert
            Assert.IsNotNull(userToUpdate);
            Assert.AreEqual(userToUpdate.FirstName, updatedUser.FirstName);
            Assert.AreEqual(userToUpdate.LastName, updatedUser.LastName);
            Assert.AreEqual(userToUpdate.Username, updatedUser.Username);
            Assert.AreEqual(userToUpdate.Password, updatedUser.Password);
            Assert.AreEqual(userToUpdate.Email, updatedUser.Email);
        }
    }
}

