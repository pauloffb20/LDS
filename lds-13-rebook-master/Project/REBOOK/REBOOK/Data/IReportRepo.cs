
using System.Collections;
using System.Collections.Generic;
using REBOOK.Models;

namespace REBOOK.Data
{
    public interface IReportRepo
    {
        
        void SubmitReport(Report report);
        IEnumerable<Report> GetReportByUserId(int id);
        IEnumerable<Report> GetAllReports();

    }
}

