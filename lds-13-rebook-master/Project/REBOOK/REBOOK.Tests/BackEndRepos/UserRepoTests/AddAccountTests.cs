using System.Linq;
using REBOOK.Data;
using REBOOK.Exceptions;
using REBOOK.Models;
using Xunit;

namespace REBOOK.Tests.BackEndRepos.UserRepoTests
{

    public class AddAccountTests 
    {
        private readonly TestDbStarter _testDb; 
        private readonly RebookDbContext _testDbAccess;
        private readonly UserRepo _userRepo;
        
        public AddAccountTests()
        {
            _testDb = new TestDbStarter();
            _testDbAccess = _testDb.GetAccess();
            _userRepo = new UserRepo(_testDbAccess);
        }
        
        //Test where account is added to an empty database
        [Fact]
        public void UserShoulBeAddedTest()
        {
            User user = new User("username", "password");
            _userRepo.AddAccount(user);
            Assert.Equal(_testDbAccess.Users.Find(1), user);
            _testDb.Erase();
        }

        [Fact]
        public void UsernameAlreadyExistsTest()
        {
            User user = new User("username", "password");
            User user2 = new User("username", "password2");
            _userRepo.AddAccount(user);
            Assert.Throws<UserAlreadyExistsException>(() => _userRepo.AddAccount(user2));
            Assert.Equal(_testDbAccess.Users.First(), user);
            _testDb.Erase();
        }
    }
}