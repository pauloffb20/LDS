using System.Data.Entity.Core;
using REBOOK.Data;
using REBOOK.Exceptions;
using REBOOK.Models;
using Xunit;

namespace REBOOK.Tests.BackEndRepos.UserRepoTests
{
    public class RateUserTests
    {
     
            private readonly TestDbStarter _testDb; 
            private readonly RebookDbContext _testDbAccess;
            private readonly UserRepo _userRepo;
        
            public RateUserTests()
            {
                _testDb = new TestDbStarter();
                _testDbAccess = _testDb.GetAccess();
                _userRepo = new UserRepo(_testDbAccess);
            }

            [Fact]
            public void ShouldWorkTest()
            {
                User rated = new User("user1", "password");
                User rater = new User("user2", "password");
                _testDbAccess.Add(rated);
                _testDbAccess.Add(rater);

                _userRepo.RateUser(rater.Id,rated.Id,5);
                Assert.Equal(5,rated.Rating);
            }
            
            [Fact]
            public void RaterDoesNotExistTest()
            {
                User rated = new User("user1", "password");
                _testDbAccess.Add(rated);
                
                Assert.Throws<ObjectNotFoundException>(() => _userRepo.RateUser(7,rated.Id,5));
            }            
            
            [Fact]
            public void RatedDoesNotExistTest()
            {
                User rater = new User("user1", "password");
                _testDbAccess.Add(rater);
                
                Assert.Throws<ObjectNotFoundException>(() => _userRepo.RateUser(rater.Id,7,5));
            }
            
            [Fact]
            public void UserAlreadyRatedTest()
            {
                User rated = new User("user1", "password");
                User rater = new User("user2", "password");
                _testDbAccess.Add(rated);
                _testDbAccess.Add(rater);

                _userRepo.RateUser(rater.Id,rated.Id,5);
                Assert.Throws<InvalidObjectException>(() => _userRepo.RateUser(rater.Id,rated.Id,5));
            }
            
            [Fact]
            public void RatingBiggerthan5Test()
            {
                User rated = new User("user1", "password");
                User rater = new User("user2", "password");
                _testDbAccess.Add(rated);
                _testDbAccess.Add(rater);

                Assert.Throws<InvalidObjectException>(() => _userRepo.RateUser(rater.Id,rated.Id,6));
            }
            
            [Fact]
            public void RatingLesserThan0Test()
            {
                User rated = new User("user1", "password");
                User rater = new User("user2", "password");
                _testDbAccess.Add(rated);
                _testDbAccess.Add(rater);

                Assert.Throws<InvalidObjectException>(() => _userRepo.RateUser(rater.Id,rated.Id,-1));
            }
    }
}