using System.Collections.Generic;
using REBOOK.Data;
using REBOOK.Exceptions;
using REBOOK.Models;
using Xunit;

namespace REBOOK.Tests.BackEndRepos.BookRepoTests
{
    public class GetBooksTests
    {
        
        private readonly TestDbStarter _testDb; 
        private readonly RebookDbContext _testDbAccess;
        private readonly BookRepo _bookRepo;
        
        public GetBooksTests()
        {
            _testDb = new TestDbStarter();
            _testDbAccess = _testDb.GetAccess();
            _bookRepo = new BookRepo(_testDbAccess);
        }
        
        
        [Fact]
        public void BooksReturnedTests()
        {

            _testDbAccess.Users.Add(new User("username", "password"));
            
            Book book = new Book("Memorial do Convento", "José Saramago");
            book.State = Stars.Five;
            book.Photos = (new Photo("Filepath"));
            book.OwnerId = 1;
            
            Book book2 = new Book("Marriage of Heaven and Hell", "William Blake");
            book2.State = Stars.Five;
            book2.Photos = (new Photo("Filepath"));
            book2.OwnerId = 1;
            
            
            _bookRepo.AddBook(book);
            _bookRepo.AddBook(book2);
            
            Assert.Equal(2, _testDbAccess.Users.Find(1).BookList.Count);
            _testDb.Erase();
            
        }
    }
}
