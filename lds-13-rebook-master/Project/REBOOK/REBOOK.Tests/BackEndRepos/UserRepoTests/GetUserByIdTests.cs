using System.Data.Entity.Core;
using System.Linq;
using REBOOK.Data;
using REBOOK.Models;
using Xunit;


namespace REBOOK.Tests.BackEndRepos.UserRepoTests
{
    public class GetUserByIdTests
    {
        private readonly TestDbStarter _testDb; 
        private readonly RebookDbContext _testDbAccess;
        private readonly UserRepo _userRepo;
        
        public GetUserByIdTests()
        {
            _testDb = new TestDbStarter();
            _testDbAccess = _testDb.GetAccess();
            _userRepo = new UserRepo(_testDbAccess);
        }

        [Fact]
        public void UserExistsTest()
        {
            //Database is empty so ID is 1 
            User user = new User("username", "password");
            _testDbAccess.Users.Add(user);
            Assert.Equal(_userRepo.GetUserById(1), user);
            _testDb.Erase();
        }

        [Fact]
        public void UserDoesNotExistTest()
        {
            Assert.Throws<ObjectNotFoundException>(() => _userRepo.GetUserById(1));
            _testDb.Erase();
        }
    }
}