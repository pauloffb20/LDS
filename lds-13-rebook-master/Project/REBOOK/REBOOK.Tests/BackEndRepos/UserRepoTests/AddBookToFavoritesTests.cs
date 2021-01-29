using System;
using System.Data.Entity.Core;
using System.Linq;
using REBOOK.Data;
using REBOOK.Models;
using Xunit;

namespace REBOOK.Tests.BackEndRepos.UserRepoTests
{
    public class AddBookToFavoritesTests
    {
        private readonly TestDbStarter _testDb; 
        private readonly RebookDbContext _testDbAccess;
        private readonly UserRepo _userRepo;
        
        public AddBookToFavoritesTests()
        {
            _testDb = new TestDbStarter();
            _testDbAccess = _testDb.GetAccess();
            _userRepo = new UserRepo(_testDbAccess);
        }

         [Fact]
         public void BookAndUserInDbTest()
         {
             User user = new User("username", "password");
             User user2 = new User("username2", "password");
             Book book = new Book("Infinite Jest", "David Foster Wallace");
             book.OwnerId = 2;
            _testDbAccess.Add(user);
            _testDbAccess.Add(user2);
            _testDbAccess.Add(book);
            _userRepo.AddBookToFavorites(1,1);
            Assert.NotNull(_testDbAccess.BooksLiked.Find(1, 1));
        }

        [Fact]
        public void BookNotFoundTest()
        {
            User user = new User("username", "password");
            _testDbAccess.Add(user);
            Assert.Throws<ObjectNotFoundException>(() => _userRepo.AddBookToFavorites(1,1));
        }

        [Fact]
        public void UserNotFoundTest()
        {
            Book book = new Book("Infinite Jest", "David Foster Wallace");
            _testDbAccess.Add(book);
            Assert.Throws<ObjectNotFoundException>(() => _userRepo.AddBookToFavorites(1,1));
        }
    }
}