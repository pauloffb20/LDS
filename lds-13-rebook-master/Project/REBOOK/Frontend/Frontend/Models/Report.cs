using System;

namespace Frontend.Models
{
    public class Report
    {
        public Report()
        {
            
        }
        
        public int Id { get; set; }
        public int ReporterId { get; set; }
        public int ReportedId { get; set; }
        public String Text { get; set; }
    }
}