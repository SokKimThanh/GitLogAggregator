using ET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class CommitInfoDAL
    {
        private GitLogAggregatorDataContext db = new GitLogAggregatorDataContext();


        // Thêm commit mới
        public void Create(CommitInfo commitInfo)
        {
            var commit = new Commit
            {
                CommitHash = commitInfo.CommitHash,
                CommitMessage = commitInfo.CommitMessage,
                CommitDate = commitInfo.CommitDate,
                Author = commitInfo.Author,
                ProjectWeekId = commitInfo.ProjectWeekId,
                CreatedAt = commitInfo.CreatedAt,
                UpdatedAt = commitInfo.UpdatedAt
            };
            db.Commits.InsertOnSubmit(commit);
            db.SubmitChanges();
        }

        // Xóa commit theo ID
        public void Delete(int commitId)
        {
            var commit = db.Commits.SingleOrDefault(c => c.CommitId == commitId);
            if (commit != null)
            {
                db.Commits.DeleteOnSubmit(commit);
                db.SubmitChanges();
            }
        }

        // Cập nhật thông tin commit
        public void Update(CommitInfo commitInfo)
        {
            var existingCommit = db.Commits.SingleOrDefault(c => c.CommitId == commitInfo.CommitId);
            if (existingCommit != null)
            {
                existingCommit.CommitHash = commitInfo.CommitHash;
                existingCommit.CommitMessage = commitInfo.CommitMessage;
                existingCommit.CommitDate = commitInfo.CommitDate;
                existingCommit.Author = commitInfo.Author;
                existingCommit.ProjectWeekId = commitInfo.ProjectWeekId;
                existingCommit.CreatedAt = commitInfo.CreatedAt;
                existingCommit.UpdatedAt = commitInfo.UpdatedAt;
                db.SubmitChanges();
            }
        }

        // Lấy tất cả commit
        public List<CommitInfo> GetAll()
        {
            return db.Commits.Select(c => new CommitInfo
            {
                CommitId = c.CommitId,
                CommitHash = c.CommitHash,
                CommitMessage = c.CommitMessage,
                CommitDate = c.CommitDate,
                Author = c.Author,
                ProjectWeekId = c.ProjectWeekId,
                CreatedAt = (DateTime)c.CreatedAt, // Chuyển đổi kiểu dữ liệu
                UpdatedAt = (DateTime)c.UpdatedAt  // Chuyển đổi kiểu dữ liệu
            }).ToList();
        }


        // Lấy commit theo ID
        public CommitInfo GetById(int commitId)
        {
            var commit = db.Commits.SingleOrDefault(c => c.CommitId == commitId);
            if (commit != null)
            {
                return new CommitInfo
                {
                    CommitId = commit.CommitId,
                    CommitHash = commit.CommitHash,
                    CommitMessage = commit.CommitMessage,
                    CommitDate = commit.CommitDate,
                    Author = commit.Author,
                    ProjectWeekId = commit.ProjectWeekId,
                    CreatedAt = (DateTime)commit.CreatedAt, // Chuyển đổi kiểu dữ liệu
                    UpdatedAt = (DateTime)commit.UpdatedAt  // Chuyển đổi kiểu dữ liệu
                };
            }
            return null;
        }
    }
}
