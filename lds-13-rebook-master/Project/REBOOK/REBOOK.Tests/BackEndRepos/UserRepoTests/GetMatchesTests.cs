using System.Data.Entity.Core;
using System.Linq;
using REBOOK.Data;
using REBOOK.Models;
using Xunit;

namespace REBOOK.Tests.BackEndRepos.UserRepoTests
{
    public class MatchTests
    {
        private readonly TestDbStarter _testDb; 
        private readonly RebookDbContext _testDbAccess;
        private readonly UserRepo _userRepo;
        private readonly BookRepo _bookRepo;
        
        public MatchTests()
        {
            _testDb = new TestDbStarter();
            _testDbAccess = _testDb.GetAccess();
            _userRepo = new UserRepo(_testDbAccess);
            _bookRepo = new BookRepo(_testDbAccess);
        }
        
        [Fact]
        public void ShouldBeMatchTest()
        {
            User user1 = new User("username", "password");
            User user2 = new User("username2", "password");
            Book book1 = new Book("Senhor dos aneis", "Tolkien");
            Book book2 = new Book("Cyclonopedia", "Reza Nagarouchi");
            _userRepo.AddAccount(user1);
            _userRepo.AddAccount(user2);

            
            book1.OwnerId = 1;
            book2.OwnerId = 2;
            book1.Photos = (new Photo("filepath"));
            book2.Photos = (new Photo("filepath"));
            book1.State = Stars.Five;
            book2.State = Stars.Five;
            _bookRepo.AddBook(book1);
            _bookRepo.AddBook(book2);
            
            _userRepo.AddBookToFavorites(user1.Id, book2.Id);
            _userRepo.AddBookToFavorites(user2.Id, book1.Id);
            
            Assert.True(_userRepo.GetMatches(2).Contains(user1));
            Assert.NotNull(_testDbAccess.Matches.Find(1,2));
            _testDb.Erase();
        }

        [Fact]
        public void UserDoesNotExists()
        {
            Assert.Throws<ObjectNotFoundException>(() => _userRepo.GetMatches(1));
            _testDb.Erase();
        }
    }
}