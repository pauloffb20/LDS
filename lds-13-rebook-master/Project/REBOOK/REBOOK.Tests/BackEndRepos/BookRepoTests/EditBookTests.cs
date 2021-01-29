using System.Data.Entity.Core;
using REBOOK.Data;
using REBOOK.Models;
using Xunit;

namespace REBOOK.Tests.BackEndRepos.BookRepoTests
{
    public class EditBookTests
    {
        private readonly TestDbStarter _testDb; 
        private readonly RebookDbContext _testDbAccess;
        private readonly BookRepo _bookRepo;
        
        public EditBookTests()
        {
            _testDb = new TestDbStarter();
            _testDbAccess = _testDb.GetAccess();
            _bookRepo = new BookRepo(_testDbAccess);
        }

        [Fact]
        public void BookEditedTest()
        {
            _testDbAccess.Users.Add(new User("username", "password"));
            Book book = new Book("Gravity's Rainbow","Thomas Pynchon");
            book.State = Stars.Five;
            book.Description = "Random description";
            book.OwnerId = 1;
            book.Photos = (new Photo("filepath"));
            
            _bookRepo.AddBook(book);
            
            Book bookedit = new Book("Spider Man","Stan Lee");
            bookedit.Id = 1;
            bookedit.State = Stars.Four;
            bookedit.Description = "Another Random description";
            bookedit.Photos = (new Photo("another filepath"));
            bookedit.OwnerId = 1;
            
            _bookRepo.EditBook(bookedit);
            Book bookTest = _testDbAccess.Books.Find(1);
            Assert.Equal(bookedit.Name, bookTest.Name);
            Assert.Equal(bookedit.Id, bookTest.Id);
            Assert.Equal(bookedit.OwnerId, bookTest.OwnerId);
            Assert.Equal(bookedit.Description, bookTest.Description);
            Assert.Equal(bookedit.Photos, bookTest.Photos);
            Assert.Equal(bookedit.Author, bookTest.Author);
            Assert.Equal(bookedit.State, bookTest.State);
            
            _testDb.Erase();
        }
        
        [Fact]
        public void BookDoesNotExistTest()
        {
            Book book = new Book("Spider Man","Stan Lee");
            book.Id = 1;
            book.Description = "Random description";
            book.Name = "Hulk";
            book.Description = "Blablabla";
            Assert.Throws<ObjectNotFoundException>(() =>  _bookRepo.EditBook(book));
            _testDb.Erase();
        }
    }
}