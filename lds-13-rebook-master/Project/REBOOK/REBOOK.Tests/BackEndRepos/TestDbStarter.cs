using System.Data.Entity;
using Microsoft.EntityFrameworkCore;
using REBOOK.Data;

namespace REBOOK.Tests.BackEndRepos
{
    public class TestDbStarter
    {
        private readonly RebookDbContext _dbContext;

        public TestDbStarter()
        {
            // var builder = new DbContextOptionsBuilder<RebookDbContext>();
            // builder.UseInMemoryDatabase("TestDatabase");
            // var options = builder.Options;
            _dbContext = new RebookDbContext(true);
            _dbContext.Database.EnsureCreated();
        }
        
        public RebookDbContext GetAccess()
        {
            return _dbContext;
        }

        public void Erase()
        {
            _dbContext.Database.EnsureDeleted();
        }
        
    }
}