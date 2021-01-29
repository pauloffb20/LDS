using System.Data.Entity.Core;
using REBOOK.Data;
using REBOOK.Models;
using Xunit;

namespace REBOOK.Tests.BackEndRepos.BookRepoTests
{
    public class GetBookByIdTests
    {
        private readonly TestDbStarter _testDb;
        private readonly RebookDbContext _testDbAccess;
        private readonly BookRepo _bookRepo;

        public GetBookByIdTests()
        {
            _testDb = new TestDbStarter();
            _testDbAccess = _testDb.GetAccess();
            _bookRepo = new BookRepo(_testDbAccess);
        }

        [Fact]
        public void BookExistTest()
        {
            Book book = new Book("Harry Potter", "JKR");
            _testDbAccess.Books.Add(book);
            Assert.Equal(_bookRepo.GetBookById(1), book);
            _testDb.Erase();
        }

        [Fact]
        public void BookDoesNotExistTest()
        {
            Assert.Throws<ObjectNotFoundException>(() => _bookRepo.GetBookById(1));
            _testDb.Erase();
        }
    }
}
