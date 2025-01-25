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
                        join pw in db.Weeks on c.WeekId equals pw.WeekId
                        join a in db.Authors on c.AuthorID equals a.AuthorID
                        join cp in db.CommitPeriods on c.PeriodID equals cp.PeriodID
                        orderby c.CommitDate ascending
                        where (searchAllWeeks || projectWeekId == null || c.WeekId == projectWeekId)
                              && (searchAllAuthors || authorId == null || a.AuthorID == authorId)
                              && (string.IsNullOrEmpty(keyword) || c.CommitMessages.Contains(keyword))
                        select new SearchResult
                        {
                            ProjectWeekName = pw.WeekName,
                            WeekStartDate = pw.WeekStartDate.Value,
                            WeekEndDate = pw.WeekEndDate.Value,
                            Period = cp.PeriodName,
                            PeriodDuration = cp.PeriodDuration,
                            Author = a.AuthorName,
                            AuthorEmail = a.AuthorEmail,
                            CommitMessages = c.CommitMessages
                        };
            return query.ToList();

        }

        public DateTime? GetFirstCommitDateByProject(int weekID)
        {
            var firstCommit = db.Commits
                                .Where(c => c.WeekId == weekID)
                                .OrderBy(c => c.CommitDate)
                                .FirstOrDefault();

            return firstCommit?.CommitDate; // Trả về ngày commit đầu tiên hoặc null nếu không có commit nào
        }
        public DateTime? GetInternshipEndDate(int? id)
        {
            if (!id.HasValue) return null;

            // Nối ConfigFiles và InternshipDirectories để lấy ngày kết thúc thực tập
            var internshipEndDate = (from cf in db.ConfigFiles
                                     join idir in db.InternshipDirectories on cf.InternshipDirectoryId equals idir.InternshipDirectoryId
                                     where cf.ConfigID == id
                                     select idir.InternshipEndDate).FirstOrDefault();

            return internshipEndDate;
        }
    }
}
