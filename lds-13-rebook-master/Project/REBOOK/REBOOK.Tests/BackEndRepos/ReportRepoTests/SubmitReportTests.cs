using System.Data.Entity.Core;
using System.Linq;
using REBOOK.Data;
using REBOOK.Models;
using Xunit;

namespace REBOOK.Tests.BackEndRepos.UserRepoTests
{
    public class SubmitReportTests
    {
        private readonly TestDbStarter _testDb; 
        private readonly RebookDbContext _testDbAccess;
        private readonly ReportRepo _reportRepo;

        public SubmitReportTests()
        {
            _testDb = new TestDbStarter();
            _testDbAccess = _testDb.GetAccess();
            _reportRepo = new ReportRepo(_testDbAccess);
        }

        [Fact]
        public void ReportAndUserInDbTest()
        {
            User user = new User("teste1", "password");
            User user2 = new User("teste2", "password");
            Report report = new Report(2, 1, "Conduta Imprópria");
            _testDbAccess.Add(user);
            _testDbAccess.Add(user2);
            _testDbAccess.Add(report);
            _reportRepo.SubmitReport(report);
            Assert.Equal(report, _testDbAccess.Reports.Find(1));
            _testDb.Erase();
        }

        [Fact]
        public void TextNotFoundTest()
        {
            User user = new User("teste2", "password");
            Report report = new Report(2, 1, null);
            _testDbAccess.Add(user);
            Assert.Throws<ObjectNotFoundException>(() => _reportRepo.SubmitReport(report));
            _testDb.Erase();
        }

        [Fact]
        public void ReporterNotFoundTeste()
        {
            User reporter = new User("reporter", "something");
            Report report = new Report(2, 1, "Report Something");
            _testDbAccess.Add(reporter);
            _testDbAccess.Add(report);
            Assert.Throws<ObjectNotFoundException>(() => _reportRepo.SubmitReport(report));
            _testDb.Erase();
        }

        [Fact]
        public void ReportedNotFoundTeste()
        {
            User reported = new User("reported", "something");
            Report report = new Report(1, 2, "Report Something");
            _testDbAccess.Add(reported);
            _testDbAccess.Add(report);
            Assert.Throws<ObjectNotFoundException>(() => _reportRepo.SubmitReport(report));
            _testDb.Erase();
        }
    }
}