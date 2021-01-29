using System.Data.Entity.Core;
using System.Linq;
using REBOOK.Data;
using REBOOK.Models;
using Xunit;

namespace REBOOK.Tests.BackEndRepos.ReportRepoTests
{
    public class GetReportByUserIdTests
    {
        private readonly TestDbStarter _testDb;
        private readonly RebookDbContext _testDbAccess;
        private readonly ReportRepo _reportRepo;

        public GetReportByUserIdTests()
        {
            _testDb = new TestDbStarter();
            _testDbAccess = _testDb.GetAccess();
            _reportRepo = new ReportRepo(_testDbAccess);
        }

        [Fact]
        public void ReportAndUserInDbTest()
        {
            _testDbAccess.Add(new User("user", "password"));
            _testDbAccess.Add(new User("nUser", "password"));

            Report report = new Report(2, 1, "Conduta imprópria");
            _reportRepo.SubmitReport(report);
            Assert.Equal(report, _reportRepo.GetReportByUserId(1).First());
            _testDb.Erase();
        }
        
        [Fact]
        public void ReportedNotFoundTest()
        {
            Assert.Throws<ObjectNotFoundException>(() => _reportRepo.GetReportByUserId(1) );
            _testDb.Erase();
        }
    }
}