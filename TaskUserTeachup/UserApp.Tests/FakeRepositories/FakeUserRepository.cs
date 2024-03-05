using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserApp.DataAccess.Interfaces;
using UserApp.Domain.Models;

namespace UserApp.Tests.FakeRepositories
{
    public class FakeUserRepository : IUserRepository
    {
        private List<User> users;
        public FakeUserRepository()
        {
            users = new List<User>()
            {
                new User() 
                { 
                    Id = 1,
                    FirstName = "Ljupche",
                    LastName = "Joldashev",
                    Username = "LJ",
                    Password = "123456",
                    Email = "Ljupche@yahoo.com",
                },
                new User()
                {
                    Id = 2,
                    FirstName = "Angela",
                    LastName = "Gjorgjievska",
                    Username = "AG",
                    Password = "123456",
                    Email = "Angela@yahoo.com",
                }
            };
        }

        public void Delete(User entity)
        {
            users.Remove(entity);
        }

        public User Get(int id)
        {
            return users.FirstOrDefault(x => x.Id == id);
        }

        public User GetUserByUsername(string username)
        {
            return users.FirstOrDefault(x => x.Username == username);
        }

        public User LoginUser(string username, string password)
        {
            // Za ova imam nekoja zamisla deka treba da napravam nekoj helper za heshiranje na password (sekako ke go iskoristam i vo servisot bidejki veke 2 pati mi se povtoruva), ama toa ke vidam so vreme kako ke bidam.
            throw new NotImplementedException();
        }

        public void Register(User user)
        {
            users.Add(user);
        }

        public void SaveLoginHistory(LoginHistory loginHistory)
        {
            throw new NotImplementedException();
        }

        public void Update(User entity)
        {
            users[users.IndexOf(entity)] = entity;
        }
    }
}
