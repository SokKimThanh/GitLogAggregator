using ET;
using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;

namespace DAL
{
    public class CommitDAL
    {
        private GitLogAggregatorDataContext db = new GitLogAggregatorDataContext();

        // Thêm commit mới
        public void Create(CommitET c)
        {
            // Kiểm tra xem CommitHash đã tồn tại chưa
            var isDuplicate = db.Commits.Any(x => x.CommitHash == c.CommitHash);
            if (isDuplicate)
            {
                throw new Exception("Trùng dữ liệu.");
            }

            // Kiểm tra xem ProjectWeekId có tồn tại không
            var projectWeekExists = db.ProjectWeeks.Any(pw => pw.ProjectWeekId == c.ProjectWeekId);
            if (!projectWeekExists)
            {
                throw new Exception("Không đúng tuần thực tập.");
            }

            // Tạo commit mới
            var commit = new Commit
            {
                CommitHash = c.CommitHash,
                CommitMessage = c.CommitMessage,
                CommitDate = c.CommitDate,
                Author = c.Author,
                ProjectWeekId = c.ProjectWeekId,
                Date = c.Date,
                Period = c.Period,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            // Thêm commit vào cơ sở dữ liệu
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
        public void Update(CommitET commitInfo)
        {
            var existingCommit = db.Commits.SingleOrDefault(c => c.CommitId == commitInfo.CommitId);
            if (existingCommit != null)
            {
                existingCommit.CommitHash = commitInfo.CommitHash;
                existingCommit.CommitMessage = commitInfo.CommitMessage;
                existingCommit.CommitDate = commitInfo.CommitDate;
                existingCommit.Author = commitInfo.Author;
                existingCommit.ProjectWeekId = commitInfo.ProjectWeekId;
                existingCommit.Date = commitInfo.Date;
                existingCommit.Period = commitInfo.Period;
                existingCommit.CreatedAt = DateTime.Now;
                existingCommit.UpdatedAt = DateTime.Now;
                db.SubmitChanges();
            }
        }

        // Lấy tất cả commit
        public List<CommitET> GetAll()
        {
            return db.Commits.OrderByDescending(c => c.CreatedAt).Select(c => new CommitET
            {
                CommitId = c.CommitId,
                CommitHash = c.CommitHash,
                CommitMessage = c.CommitMessage,
                CommitDate = c.CommitDate,
                Author = c.Author,
                ProjectWeekId = c.ProjectWeekId,
                Date = (DateTime)c.Date,
                Period = c.Period,
                CreatedAt = (DateTime)c.CreatedAt,
                UpdatedAt = (DateTime)c.UpdatedAt
            }).ToList();
        }

        // Lấy commit theo ID
        public CommitET GetById(int commitId)
        {
            var commit = db.Commits.SingleOrDefault(c => c.CommitId == commitId);
            if (commit != null)
            {
                return new CommitET
                {
                    CommitId = commit.CommitId,
                    CommitHash = commit.CommitHash,
                    CommitMessage = commit.CommitMessage,
                    CommitDate = commit.CommitDate,
                    Author = commit.Author,
                    ProjectWeekId = commit.ProjectWeekId,
                    Date = (DateTime)commit.Date,
                    Period = commit.Period,
                    CreatedAt = (DateTime)commit.CreatedAt,
                    UpdatedAt = (DateTime)commit.UpdatedAt
                };
            }
            return null;
        }
    }
}
