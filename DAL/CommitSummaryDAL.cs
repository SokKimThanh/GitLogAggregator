using DocumentFormat.OpenXml.Office2019.Word.Cid;
using ET;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class CommitSummaryDAL
    {
        private readonly GitLogAggregatorDataContext db = new GitLogAggregatorDataContext();

        // CREATE
        public void Add(CommitSummaryET c)
        {
            try
            {
                CommitSummary commitSummary = new CommitSummary
                {
                    CommitID = c.CommitID,
                    SummaryID = c.SummaryID,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };

                db.CommitSummaries.InsertOnSubmit(commitSummary);
                db.SubmitChanges();
            }
            catch (SqlException ex)
            {
                // Log lỗi SQL specific
                Console.WriteLine("[DAL] SQL Error creating CommitSummary", ex);
                throw new Exception("Database operation failed", ex);
            }
            catch (Exception ex)
            {
                // Log lỗi tổng quát
                Console.WriteLine("[DAL] Error creating CommitSummary", ex);
                throw;
            }
        }

        public bool Exists(int commitId, int summaryId)
        {
            try
            {
                // Sử dụng LINQ query syntax
                IEnumerable<bool> query =
                    from cs in db.CommitSummaries
                    where cs.CommitID == commitId && cs.SummaryID == summaryId
                    select true;

                // Thực thi truy vấn và kiểm tra kết quả
                return query.Take(1).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Console.WriteLine("[DAL] Error checking existence: " + ex.Message);
                throw;
            }
        }
        public CommitSummaryET GetBySummaryId(int commitSummaryId)
        {
            try
            {
                var commitSummary = db.CommitSummaries
                       .FirstOrDefault(cs => cs.CommitSummaryID == commitSummaryId);

                if (commitSummary == null) return null;

                return new CommitSummaryET
                {
                    CommitSummaryID = commitSummary.CommitSummaryID,
                    CommitID = commitSummary.CommitID,
                    SummaryID = commitSummary.SummaryID,
                    CreatedAt = commitSummary.CreatedAt ?? DateTime.MinValue,
                    UpdatedAt = commitSummary.UpdatedAt ?? DateTime.MinValue
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"[DAL] Error getting CommitSummary by ID: {commitSummaryId}", ex);

            }
        }

        public List<CommitSummaryET> GetByCommitId(int commitId)
        {
            try
            {
                return db.CommitSummaries
                   .Where(cs => cs.CommitID == commitId)
                   .Select(cs => new CommitSummaryET
                   {
                       CommitSummaryID = cs.CommitSummaryID,
                       CommitID = cs.CommitID,
                       SummaryID = cs.SummaryID,
                       CreatedAt = cs.CreatedAt ?? DateTime.MinValue,
                       UpdatedAt = cs.UpdatedAt ?? DateTime.MinValue
                   })
                   .ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"[DAL] Error getting GetByCommitId: {commitId}", ex);

            }
        }
        // UPDATE
        public void Update(CommitSummaryET updatedCommitSummaryET)
        {
            try
            {
                var existing = db.CommitSummaries
                               .FirstOrDefault(cs => cs.CommitSummaryID == updatedCommitSummaryET.CommitSummaryID);

                if (existing != null)
                {
                    existing.CommitID = updatedCommitSummaryET.CommitID;
                    existing.SummaryID = updatedCommitSummaryET.SummaryID;
                    existing.UpdatedAt = DateTime.Now; // Cập nhật thời gian

                    db.SubmitChanges();
                }
            }
            catch (SqlException ex)
            {
                // Log lỗi SQL specific
                Console.WriteLine("[DAL] SQL Error Updating CommitSummary", ex);
                throw new Exception("Database operation failed", ex);
            }
            catch (Exception ex)
            {
                // Log lỗi tổng quát
                Console.WriteLine("[DAL] Error Updating CommitSummary", ex);
                throw;
            }
        }

        // DELETE
        public void Delete(int commitSummaryId)
        {
            try
            {
                var toDelete = db.CommitSummaries
                           .FirstOrDefault(cs => cs.CommitSummaryID == commitSummaryId);

                if (toDelete != null)
                {
                    db.CommitSummaries.DeleteOnSubmit(toDelete);
                    db.SubmitChanges();
                }
            }
            catch (SqlException ex)
            {
                // Log lỗi SQL specific
                Console.WriteLine("[DAL] SQL Error Deleting CommitSummary", ex);
                throw new Exception("Database operation failed", ex);
            }
            catch (Exception ex)
            {
                // Log lỗi tổng quát
                Console.WriteLine("[DAL] Error Deleting CommitSummary", ex);
                throw;
            }
        }
    }
}
