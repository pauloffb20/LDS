using System.Data.Entity.Core;
using System.Linq;
using REBOOK.Data;
using REBOOK.Models;
using Xunit;

namespace REBOOK.Tests.BackEndRepos.UserRepoTests
{
    public class RemoveBookFromFavoritesTests
    {
        private readonly TestDbStarter _testDb; 
            private readonly RebookDbContext _testDbAccess;
            private readonly UserRepo _userRepo;
        
            public RemoveBookFromFavoritesTests()
            {
                _testDb = new TestDbStarter();
                _testDbAccess = _testDb.GetAccess();
                _userRepo = new UserRepo(_testDbAccess);
            }

            [Fact]
            public void BookAndUserInDbBookInFavoritesTest()
            {
                User user = new User("username", "password");
                User user2 = new User("username2", "password");
                Book book = new Book("Infinite Jest", "David Foster Wallace");
                book.OwnerId = 2;
                _testDbAccess.Add(user);
                _testDbAccess.Add(user2);
                _testDbAccess.Add(book);
                _userRepo.AddBookToFavorites(1,1);
                _userRepo.RemoveBookFromFavorites(1,1);
                Assert.Null(_testDbAccess.BooksLiked.Find(1,1));
                _testDb.Erase();
            }

            [Fact]
            public void BookNotFoundTest()
            {
                User user = new User("username", "password");
                _testDbAccess.Add(user);
                Assert.Throws<ObjectNotFoundException>(() => _userRepo.RemoveBookFromFavorites(1,1));
                _testDb.Erase();
            }

            [Fact]
            public void UserNotFoundTest()
            {
                Book book = new Book("Infinite Jest", "David Foster Wallace");
                _testDbAccess.Add(book);
                Assert.Throws<ObjectNotFoundException>(() => _userRepo.RemoveBookFromFavorites(1,1));
                _testDb.Erase();
            }

            [Fact]
            public void BookNotInFavorites()
            {
                User user = new User("username", "password");
                Book book = new Book("Infinite Jest", "David Foster Wallace");
                _testDbAccess.Add(user);
                _testDbAccess.Add(book);
                Assert.Throws<ObjectNotFoundException>(() => _userRepo.RemoveBookFromFavorites(1,1));
                _testDb.Erase();
            }
    }
}