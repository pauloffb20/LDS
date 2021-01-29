using System.Collections.Generic;
using System.Linq;
using REBOOK.Data;
using REBOOK.Exceptions;
using REBOOK.Models;
using Xunit;

namespace REBOOK.Tests.BackEndRepos.ReportRepoTests
{
    public class GetAllReportsTests
    {
        private readonly TestDbStarter _testDb;
        private readonly RebookDbContext _testDbAccess;
        private readonly ReportRepo _reportRepo;

        public GetAllReportsTests()
        {
            _testDb = new TestDbStarter();
            _testDbAccess = _testDb.GetAccess();
            _reportRepo = new ReportRepo(_testDbAccess);
        }

        [Fact]
        public void ShouldReturnReportsTest()
        {
            _testDbAccess.Add(new User("user", "password"));
            _testDbAccess.Add(new User("nUser", "password"));
            List<Report> list = new List<Report>();
            list.Add(new Report(1, 2, "something"));
            list.Add(new Report(1, 2, "something"));
            list.Add(new Report(1, 2, "something"));
            list.Add(new Report(1, 2, "something"));

            foreach (Report report in list)
            {
                _reportRepo.SubmitReport(report);
            }
            
            Assert.Equal(4, _reportRepo.GetAllReports().Count());
            _testDb.Erase();
        }
    }
}