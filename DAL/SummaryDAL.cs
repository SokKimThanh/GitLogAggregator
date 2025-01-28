using ET;
using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class SummaryDAL
    {
        private readonly GitLogAggregatorDataContext db = new GitLogAggregatorDataContext();

        // Lưu Summary
        public void Add(SummaryET summaryET)
        {
            if (summaryET == null) throw new ArgumentNullException(nameof(summaryET));

            // Kiểm tra trùng lặp
            var summary = db.Summaries.FirstOrDefault(s => s.SummaryID == summaryET.SummaryID) ?? new Summary();

            // Map properties from SummaryET to Summary
            summary.SummaryID = summaryET.SummaryID;
            summary.Attendance = summaryET.Attendance;
            summary.AssignedTasks = summaryET.AssignedTasks;
            summary.ContentResults = summaryET.ContentResults;
            summary.SupervisorComments = summaryET.SupervisorComments;
            summary.Notes = summaryET.Notes;
            summary.CreatedAt = DateTime.Now;
            summary.UpdatedAt = DateTime.Now;
            db.Summaries.InsertOnSubmit(summary);
            db.SubmitChanges();
        }


        public SummaryET GetLastInserted()
        {
            var summary = db.Summaries.OrderByDescending(s => s.SummaryID).FirstOrDefault();
            if (summary == null) return null;

            return new SummaryET
            {
                SummaryID = summary.SummaryID,
                Attendance = summary.Attendance,
                AssignedTasks = summary.AssignedTasks,
                ContentResults = summary.ContentResults,
                SupervisorComments = summary.SupervisorComments,
                Notes = summary.Notes,
                CreatedAt = summary.CreatedAt ?? DateTime.MinValue,
                UpdatedAt = summary.UpdatedAt ?? DateTime.MinValue,
            };
        }
        // READ BY ID
        public SummaryET GetById(int id)
        {
            var summary = db.Summaries.FirstOrDefault(s => s.SummaryID == id);

            if (summary == null) return null;

            return new SummaryET
            {
                SummaryID = summary.SummaryID,
                Attendance = summary.Attendance,
                AssignedTasks = summary.AssignedTasks,
                ContentResults = summary.ContentResults,
                SupervisorComments = summary.SupervisorComments,
                Notes = summary.Notes,
                CreatedAt = summary.CreatedAt ?? DateTime.MinValue,
                UpdatedAt = summary.UpdatedAt ?? DateTime.MinValue,
            };
        }
        // READ ALL
        public List<SummaryET> GetAll()
        {
            return db.Summaries.Select(s => new SummaryET
            {
                SummaryID = s.SummaryID,
                Attendance = s.Attendance,
                AssignedTasks = s.AssignedTasks,
                ContentResults = s.ContentResults,
                SupervisorComments = s.SupervisorComments,
                Notes = s.Notes,
                CreatedAt = s.CreatedAt ?? DateTime.MinValue,
                UpdatedAt = s.UpdatedAt ?? DateTime.MinValue,
            }).ToList();
        }

        // UPDATE
        public void Update(SummaryET updatedSummary)
        {
            var existingSummary = db.Summaries
                                  .FirstOrDefault(s => s.SummaryID == updatedSummary.SummaryID);

            if (existingSummary != null)
            {
                existingSummary.Attendance = updatedSummary.Attendance;
                existingSummary.AssignedTasks = updatedSummary.AssignedTasks;
                existingSummary.ContentResults = updatedSummary.ContentResults;
                existingSummary.SupervisorComments = updatedSummary.SupervisorComments;
                existingSummary.Notes = updatedSummary.Notes;
                existingSummary.UpdatedAt = DateTime.Now;

                db.SubmitChanges();
            }
        }

        // DELETE
        public void Delete(int id)
        {
            var summaryToDelete = db.Summaries
                                  .FirstOrDefault(s => s.SummaryID == id);

            if (summaryToDelete != null)
            {
                db.Summaries.DeleteOnSubmit(summaryToDelete);
                db.SubmitChanges();
            }
        }

        public SummaryET GetByDateAndPeriod(DateTime date, TimeSpan periodStartTime, TimeSpan periodEndTime)
        {
            var summary = (from cs in db.CommitSummaries
                           join c in db.Commits on cs.CommitID equals c.CommitID
                           join p in db.CommitPeriods on c.PeriodID equals p.PeriodID
                           join s in db.Summaries on cs.SummaryID equals s.SummaryID
                           join w in db.Weeks on c.WeekId equals w.WeekId
                           where w.WeekStartDate <= date.Date && w.WeekEndDate >= date.Date
                                 && p.PeriodStartTime == periodStartTime
                                 && p.PeriodEndTime == periodEndTime
                           select s).FirstOrDefault();

            if (summary == null)
            {
                return null;
            }

            return new SummaryET
            {
                SummaryID = summary.SummaryID,
                Attendance = summary.Attendance,
                AssignedTasks = summary.AssignedTasks,
                ContentResults = summary.ContentResults,
                SupervisorComments = summary.SupervisorComments,
                Notes = summary.Notes,
                CreatedAt = summary.CreatedAt ?? DateTime.MinValue,
                UpdatedAt = summary.UpdatedAt ?? DateTime.MinValue,
            };
        }
    }
}
