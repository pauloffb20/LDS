using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Linq;
using REBOOK.Exceptions;
using REBOOK.Models;

namespace REBOOK.Data
{
    public class ReportRepo : IReportRepo
    {
        private readonly RebookDbContext _db = new RebookDbContext();

        public ReportRepo(RebookDbContext dbContext)
        {
            _db = dbContext;
        }

        public ReportRepo(){}        
        
        public void SubmitReport(Report report)
        {
            if (report.ReporterId == null)
            {
                throw new ObjectNotFoundException("ReporterID was not found");
            }

            if (report.ReportedId == null)
            {
                throw new ObjectNotFoundException("ReportedID was not found");
            }

            if (report.Text == null)
            {
                throw new ObjectNotFoundException("Text Invalid!");
            }
            
            var user = _db.Users.Find(report.ReportedId);
            var user2 = _db.Users.Find(report.ReporterId);

            if (user == null || user2 == null)
            {
                throw new ObjectNotFoundException("User Not Found");
            }

            _db.Reports.Add(report);
            _db.SaveChanges();
        }

        public IEnumerable<Report> GetReportByUserId(int id)
        {
            var user = _db.Users.Find(id);
            
            if (user == null)
            {
                throw new ObjectNotFoundException("User Reported not Found");
            }

            return _db.Reports.Where(r => r.ReportedId == id).ToList();
        }

        public IEnumerable<Report> GetAllReports()
        {
            var reports = _db.Reports.ToList();
            if (reports.Count == 0)
            {
                throw new DatabaseIsEmptyException("No reports in database");
            }

            return _db.Reports.ToList();
        }
    }
}