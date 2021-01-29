using System.Data.Entity.Core;
using REBOOK.Data;
using REBOOK.Models;
using Xunit;

namespace REBOOK.Tests.BackEndRepos.UserRepoTests
{
    public class GetAllFavoritesTests
    {
        private readonly TestDbStarter _testDb; 
        private readonly RebookDbContext _testDbAccess;
        private readonly UserRepo _userRepo;
        
        public GetAllFavoritesTests()
        {
            _testDb = new TestDbStarter();
            _testDbAccess = _testDb.GetAccess();
            _userRepo = new UserRepo(_testDbAccess);
        }

        [Fact]
        public void UserExistsInDbTest()
        {
            User user1 = new User("username","password");
            User user2 = new User("username","password");
            Book book1 = new Book("Cadernos de Pickwick", "Charles Dickens");
            Book book2 = new Book("Ficções", "Borges");
            book1.OwnerId = 2;
            book2.OwnerId = 2;
            _testDbAccess.Users.Add(user1);
            _testDbAccess.Users.Add(user2);
            _testDbAccess.Books.Add(book1);
            _testDbAccess.Books.Add(book2);
            _userRepo.AddBookToFavorites(user1.Id,book1.Id);
            _userRepo.AddBookToFavorites(user1.Id,book2.Id);

            Assert.Equal(2,_userRepo.getAllFavorites(user1.Id).Count);
            _testDb.Erase();
            
        }

        [Fact]
        public void UserDoesNotExistInDbTest()
        {
            Assert.Throws<ObjectNotFoundException>(() => _userRepo.getAllFavorites(3));
            _testDb.Erase();
        }
    }
}