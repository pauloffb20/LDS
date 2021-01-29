using System;
using System.Collections.Generic;
using System.Linq;
using REBOOK.Data;
using REBOOK.Exceptions;
using REBOOK.Models;
using Xunit;

namespace REBOOK.Tests.BackEndRepos.UserRepoTests
{
    public class GetAllUsersTests
    {
        private readonly TestDbStarter _testDb; 
        private readonly RebookDbContext _testDbAccess;
        private readonly UserRepo _userRepo;
        
        public GetAllUsersTests()
        {
            _testDb = new TestDbStarter();
            _testDbAccess = _testDb.GetAccess();
            _userRepo = new UserRepo(_testDbAccess);
        }

        [Fact]
        public void UsersReturnedTest()
        {
            List<User> list = new List<User>();
            list.Add(new User("user1", "password"));
            list.Add(new User("user2", "password"));
            list.Add(new User("user3", "password"));
            list.Add(new User("user4", "password"));
            list.Add(new User("user5", "password"));

            foreach (User user in list)
            {
                _userRepo.AddAccount(user);
            }

            Assert.Equal(5, _userRepo.GetAllUsers().Count());
            _testDb.Erase();
        }
    }
}