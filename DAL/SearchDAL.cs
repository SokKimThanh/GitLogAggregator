using ET;
using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DAL
{
    public class SearchDAL
    {
        private readonly GitLogAggregatorDataContext db = new GitLogAggregatorDataContext();
        public List<SearchResult> SearchCommits(
    )
        { 
            var query = from pw in db.ProjectWeeks
                        join c in db.Commits on pw.ProjectWeekId equals c.ProjectWeekId
                        join cp in db.CommitPeriods on pw.ProjectWeekId equals cp.PeriodID
                        join cs in db.ChatbotSummaries on cp.PeriodID equals cs.PeriodID into chatbotJoin
                        from cs in chatbotJoin.DefaultIfEmpty()
                        join a in db.Authors on c.Author equals a.AuthorName
                        join cf in db.ConfigFiles on pw.InternshipDirectoryId equals cf.InternshipDirectoryId
                        join id in db.InternshipDirectories on cf.InternshipDirectoryId equals id.ID
                         


                        select new SearchResult
                        {
                            // Mapping các trường dữ liệu
                            ProjectWeekName = pw.ProjectWeekName,
                            WeekStartDate = pw.WeekStartDate.Value,
                            WeekEndDate = pw.WeekEndDate.Value,
                            InternshipDirectoryId = pw.InternshipDirectoryId,
                            CommitHash = c.CommitHash,
                            CommitMessage = c.CommitMessage,
                            CommitDate = c.CommitDate,
                            Author = c.Author,
                            AuthorEmail = c.AuthorEmail,
                            ProjectWeekId = c.ProjectWeekId,
                            Date = c.Date,
                            Period = c.Period,
                            PeriodName = cp.PeriodName,
                            PeriodDuration = cp.PeriodDuration,
                            PeriodStartDate = cp.PeriodStartDate,
                            PeriodEndDate = cp.PeriodEndDate,
                            PeriodID = cp.PeriodID,
                            Attendance = cs != null ? cs.Attendance : null,
                            AssignedTasks = cs != null ? cs.AssignedTasks : null,
                            ContentResults = cs != null ? cs.ContentResults : null,
                            SupervisorComments = cs != null ? cs.SupervisorComments : null,
                            Notes = cs != null ? cs.Notes : null,
                            AuthorName = a.AuthorName,
                            ProjectDirectory = cf.ProjectDirectory,
                            InternshipStartDate = cf.InternshipStartDate,
                            InternshipEndDate = cf.InternshipEndDate,
                            Weeks = cf.Weeks,
                            FirstCommitDate = cf.FirstCommitDate,
                            FirstCommitAuthor = cf.FirstCommitAuthor,
                            InternshipWeekFolder = id.InternshipWeekFolder,
                            DateModified = id.DateModified
                        };

            return query.ToList();
        }

        public DateTime? GetFirstCommitDateByProject(int projectId)
        {
            var firstCommit = db.Commits
                                .Where(c => c.ProjectWeekId == projectId)
                                .OrderBy(c => c.CommitDate)
                                .FirstOrDefault();

            return firstCommit?.CommitDate; // Trả về ngày commit đầu tiên hoặc null nếu không có commit nào
        }
        public DateTime? GetInternshipEndDate(int? projectWeekId)
        {
            if (!projectWeekId.HasValue) return null;

            var config = db.ConfigFiles
                         .FirstOrDefault(cf => db.ProjectWeeks
                                                 .Any(pw => pw.ProjectWeekId == projectWeekId &&
                                                            pw.InternshipDirectoryId == cf.InternshipDirectoryId));
            return config?.InternshipEndDate;
        }
    }
}
