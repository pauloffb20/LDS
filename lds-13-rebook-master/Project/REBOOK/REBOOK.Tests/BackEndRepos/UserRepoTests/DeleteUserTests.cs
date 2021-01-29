using System.Data.Entity.Core;
using System.Linq;
using REBOOK.Data;
using REBOOK.Models;
using Xunit;

namespace REBOOK.Tests.BackEndRepos.UserRepoTests
{
    public class DeleteUserTests
    {
        private readonly TestDbStarter _testDb; 
        private readonly RebookDbContext _testDbAccess;
        private readonly UserRepo _userRepo;
        
        public DeleteUserTests()
        {
            _testDb = new TestDbStarter();
            _testDbAccess = _testDb.GetAccess();
            _userRepo = new UserRepo(_testDbAccess);
        }

        [Fact]
        public void UserIsDeletedTest()
        {
            _userRepo.AddAccount(new User("username","password"));
            _userRepo.DeleteUser(1);
            Assert.Equal(0, _testDbAccess.Users.Count());
            _testDb.Erase();
        }
        
        [Fact]
        public void UserDoesNotExistTest()
        {
            Assert.Throws<ObjectNotFoundException>(() => _userRepo.DeleteUser(1));
            _testDb.Erase();
        }
    }
}