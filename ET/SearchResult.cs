using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET
{
    public class SearchResult
    {
        public string ProjectWeekName { get; set; }
        public DateTime WeekStartDate { get; set; }
        public DateTime WeekEndDate { get; set; }
        public int InternshipDirectoryId { get; set; }
        public string CommitHash { get; set; }
        public string CommitMessage { get; set; }
        public DateTime CommitDate { get; set; }
        public string Author { get; set; }
        public string AuthorEmail { get; set; }
        public int ProjectWeekId { get; set; }
        public DateTime Date { get; set; }
        public string Period { get; set; }
        public string PeriodName { get; set; }
        public string PeriodDuration { get; set; }
        public DateTime PeriodStartDate { get; set; }
        public DateTime PeriodEndDate { get; set; }
        public int PeriodID { get; set; }
        public string Attendance { get; set; }
        public string AssignedTasks { get; set; }
        public string ContentResults { get; set; }
        public string SupervisorComments { get; set; }
        public string Notes { get; set; }
        public string AuthorName { get; set; }
        public string ProjectDirectory { get; set; }
        public DateTime InternshipStartDate { get; set; }
        public DateTime InternshipEndDate { get; set; }
        public int Weeks { get; set; }
        public DateTime FirstCommitDate { get; set; }
        public string FirstCommitAuthor { get; set; }
        public string InternshipWeekFolder { get; set; }
        public DateTime DateModified { get; set; }
    }
}
