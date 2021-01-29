using System.Data.Entity.Core;
using System.Linq;
using REBOOK.Data;
using REBOOK.Models;
using Xunit;

namespace REBOOK.Tests.BackEndRepos.BookRepoTests
{
    public class DeleteBookTests
    {
        
        private readonly TestDbStarter _testDb; 
        private readonly RebookDbContext _testDbAccess;
        private readonly BookRepo _bookRepo;
        
        public DeleteBookTests()
        {
            _testDb = new TestDbStarter();
            _testDbAccess = _testDb.GetAccess();
            _bookRepo = new BookRepo(_testDbAccess);
        }

        [Fact]
        public void BookExistsUserExistsAndHasBookInListTest()
        {
            User user = new User("username","password");
            Book book = new Book("Life at sea", "Mishima");
            book.State = Stars.Five;
            book.OwnerId = 1;
            book.Photos = (new Photo("filepath"));
            
            
            _testDbAccess.Users.Add(user);
            _bookRepo.AddBook(book);
            _bookRepo.DeleteBook(1);
            Assert.Empty( _testDbAccess.Books);
            Assert.Empty( _testDbAccess.Users.Find(1).BookList);
            
            _testDb.Erase();
        }
        
        [Fact]
        public void BookDoesNotExistTest() {
            
            Assert.Throws<ObjectNotFoundException>(() => _bookRepo.DeleteBook(1));
            _testDb.Erase();
        }

        [Fact]
        public void UserDoesNotHaveInListxist()
        {
            Book book = new Book("Life at sea", "Mishima");
            book.State = Stars.Five;
            book.OwnerId = 1;
            book.Photos = (new Photo("filepath"));
            _testDbAccess.Books.Add(book);
            Assert.Throws<ObjectNotFoundException>(() => _bookRepo.DeleteBook(1));
            _testDb.Erase();
            
        }
    }
}