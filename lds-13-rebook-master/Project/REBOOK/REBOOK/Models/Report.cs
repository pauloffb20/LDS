using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace REBOOK.Models
{
    public class Report
    {
        public Report(int reporterId, int reportedId, string text)
         {
            ReporterId = reporterId;
            ReportedId = reportedId;
            Text = text;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [ForeignKey("User")]
        public int ReporterId { get; set; }
        [ForeignKey("User")]
        public int ReportedId { get; set; }
        public String Text { get; set; }
    }
}