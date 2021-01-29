using System.Data.Entity.Core;
using System.Linq;
using REBOOK.Data;
using REBOOK.Exceptions;
using REBOOK.Models;
using Xunit;

namespace REBOOK.Tests.BackEndRepos.BookRepoTests
{
    public class AddBookTests
    {
        
        private readonly TestDbStarter _testDb; 
        private readonly RebookDbContext _testDbAccess;
        private readonly BookRepo _bookRepo;
        
        public AddBookTests()
        {
            _testDb = new TestDbStarter();
            _testDbAccess = _testDb.GetAccess();
            _bookRepo = new BookRepo(_testDbAccess);
        }
        
        [Fact]
        public void BookValidAndUserExistsTest()
        {
            User user = new User("username", "password");
            Book book = new Book("Spider Man","Stan Lee");
            book.State = Stars.Five;
            book.Photos =new Photo("filepath");
            book.OwnerId = 1;
            
            _testDbAccess.Users.Add(user);
            _bookRepo.AddBook(book);
            Assert.Equal(_testDbAccess.Books.Find(1), book);
            Assert.Equal(book, user.BookList.First());
            _testDb.Erase();
            
        }

        [Fact]
        public void BookHasNoNameTest()
        {
            User user = new User("username", "password");
            Book book = new Book(null,"Stan Lee");
            book.State = Stars.Five;
            book.Photos = new Photo("filepath");
            book.OwnerId = 1;
            _testDbAccess.Users.Add(user);

            Assert.Throws<InvalidObjectException>(() => _bookRepo.AddBook(book));
            _testDb.Erase();
        }
        
        [Fact]
        public void BookHasNoAuthorTest()
        {
            User user = new User("username", "password");
            Book book = new Book("Spider Man", null);
            book.State = Stars.Five;
            book.Photos = (new Photo("filepath"));
            book.OwnerId = 1;
            _testDbAccess.Users.Add(user);
            Assert.Throws<InvalidObjectException>(() => _bookRepo.AddBook(book));
            _testDb.Erase();
        }
        
        
        [Fact]
        public void BookHasNoStateTest()
        {
            User user = new User("username", "password");
            Book book = new Book("Uma Faca Nos Dentes", "António José Forte");
            book.Photos = (new Photo("filepath"));
            book.OwnerId = 1;
            _testDbAccess.Users.Add(user);
            Assert.Throws<InvalidObjectException>(() => _bookRepo.AddBook(book));
            _testDb.Erase();
        }
        
        
        [Fact]
        public void BookHasNoPhotosTest()
        {
            User user = new User("username", "password");
            Book book = new Book("Uma Faca Nos Dentes", "António José Forte");
            book.State = Stars.Five;
            book.OwnerId = 1;
            _testDbAccess.Users.Add(user);
            Assert.Throws<InvalidObjectException>(() => _bookRepo.AddBook(book));
            _testDb.Erase();
        }
        
        [Fact]
        public void UserDoesNotExistTest()
        {
            Book book = new Book("Uma Faca Nos Dentes", "António José Forte");
            book.Photos = (new Photo("filepath"));
            book.State = Stars.Five;
            book.OwnerId = 1;
            Assert.Throws<ObjectNotFoundException>(() => _bookRepo.AddBook(book));
            _testDb.Erase();
        }
    }
}
