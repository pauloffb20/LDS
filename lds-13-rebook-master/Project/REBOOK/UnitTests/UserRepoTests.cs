using System.Data.Entity;
using Xunit;
namespace UnitTest
{
    public class UserRepoTests
    {
        [Fact]
        public void SavesToDb()
        {
            using (var ctx = new AppDbContext())
            {
                ctx.
            }
        }
    }

    public class AppDbContext : DbContext  {
        public AppDbContext()
        {
            
        }
        
    }
    
    
}