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
        public List<SearchResult> SearchCommits(bool searchAllWeeks, int? projectWeekId, bool searchAllAuthors, int? authorId, string keyword)
        {

            var query = from c in db.Commits
                        join pw in db.ProjectWeeks on c.ProjectWeekId equals pw.ProjectWeekId
                        join a in db.Authors on c.Author equals a.AuthorName
                        join cgm in db.CommitGroupMembers on c.CommitId equals cgm.CommitId
                        join cp in db.CommitPeriods on cgm.PeriodID equals cp.PeriodID
                        where (searchAllWeeks || projectWeekId == null || c.ProjectWeekId == projectWeekId)
                              && (searchAllAuthors || authorId == null || a.AuthorID == authorId)
                              && (string.IsNullOrEmpty(keyword) || c.CommitMessage.Contains(keyword))
                        select new SearchResult
                        {
                            ProjectWeekName = pw.ProjectWeekName,
                            WeekStartDate = pw.WeekStartDate.Value,
                            WeekEndDate = pw.WeekEndDate.Value,
                            Period = cp.PeriodName,
                            PeriodDuration = cp.PeriodDuration,
                            Author = a.AuthorName,
                            AuthorEmail = a.AuthorEmail,
                            CommitMessage = c.CommitMessage
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
