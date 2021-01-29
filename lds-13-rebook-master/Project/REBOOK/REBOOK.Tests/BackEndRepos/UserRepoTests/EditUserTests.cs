using System.Data.Entity.Core;
using System.Linq;
using REBOOK.Data;
using REBOOK.Models;
using Xunit;

namespace REBOOK.Tests.BackEndRepos.UserRepoTests
{
    public class EditUserTests 
    {
        private readonly TestDbStarter _testDb; 
        private readonly RebookDbContext _testDbAccess;
        private readonly UserRepo _userRepo;
        
        public EditUserTests()
        {
            _testDb = new TestDbStarter();
            _testDbAccess = _testDb.GetAccess();
            _userRepo = new UserRepo(_testDbAccess);
        }

        [Fact]
        public void UserEditedTest()
        {
            _userRepo.AddAccount(new User("username","password"));
            User user = new User("username", "password");
            user.Id = 1;
            user.Description = "dummy description";
            user.Password = "new Password";
            user.Range = 15;
            _userRepo.EditUser(user);
            Assert.True(_userRepo.GetUserById(1).Description.Equals(user.Description));
            Assert.True(_userRepo.GetUserById(1).Range.Equals(user.Range));
            Assert.True(_userRepo.GetUserById(1).Password.Equals(user.Password));
            Assert.Equal(1,_userRepo.GetAllUsers().Count());
            _testDb.Erase();
        }
        
        [Fact]
        public void UserDoesNotExist()
        {
            User user = new User("username", "password");
            user.Id = 1;
            user.Description = "dummy description";
            user.Password = "new Password";
            user.Range = 15;

            Assert.Throws<ObjectNotFoundException>(() =>  _userRepo.EditUser(user));
            _testDb.Erase();
        }
    }
}