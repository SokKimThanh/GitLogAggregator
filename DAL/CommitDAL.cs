using ET;
using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;

namespace DAL
{
    public class CommitDAL
    {
        private readonly GitLogAggregatorDataContext db = new GitLogAggregatorDataContext();

        public List<CommitET> GetAll()
        {
            try
            {
                var query = from c in db.Commits
                            orderby c.CreatedAt descending
                            select new CommitET
                            {
                                CommitId = c.CommitId,
                                CommitHash = c.CommitHash,
                                CommitMessage = c.CommitMessage,
                                CommitDate = c.CommitDate,
                                Author = c.Author,
                                ProjectWeekId = c.ProjectWeekId,
                                Date = c.Date,
                                Period = c.Period,
                                CreatedAt = c.CreatedAt.Value,
                                UpdatedAt = c.UpdatedAt.Value
                            };

                return query.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error in GetAll: " + ex.Message);
            }
        }

        public CommitET GetByID(int id)
        {
            try
            {
                var query = from c in db.Commits
                            where c.CommitId == id
                            select new CommitET
                            {
                                CommitId = c.CommitId,
                                CommitHash = c.CommitHash,
                                CommitMessage = c.CommitMessage,
                                CommitDate = c.CommitDate,
                                Author = c.Author,
                                ProjectWeekId = c.ProjectWeekId,
                                Date = c.Date,
                                Period = c.Period,
                                CreatedAt = c.CreatedAt.Value,
                                UpdatedAt = c.UpdatedAt.Value
                            };

                return query.SingleOrDefault();
            }
            catch (Exception ex)
            {
                throw new Exception("Error in GetByID: " + ex.Message);
            }
        }
        public CommitET GetLastInserted()
        {
            try
            {
                var query = from c in db.Commits
                            orderby c.CreatedAt descending
                            select new CommitET
                            {
                                CommitId = c.CommitId,
                                CommitHash = c.CommitHash,
                                CommitMessage = c.CommitMessage,
                                CommitDate = c.CommitDate,
                                Author = c.Author,
                                ProjectWeekId = c.ProjectWeekId,
                                Date = c.Date,
                                Period = c.Period,
                                CreatedAt = c.CreatedAt.Value,
                                UpdatedAt = c.UpdatedAt.Value
                            };
                return query.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new Exception("Error in GetLastInserted: " + ex.Message);
            }
        }

        // Thêm commit mới
        public void Add(CommitET c)
        {
            try
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
                var entity = new Commit
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
                db.Commits.InsertOnSubmit(entity);
                db.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Error in Add: " + ex.Message);
            }
        }

        public void Update(CommitET et)
        {
            try
            {
                var query = from c in db.Commits
                            where c.CommitId == et.CommitId
                            select c;

                var entity = query.SingleOrDefault();
                if (entity == null) return;

                entity.CommitHash = et.CommitHash;
                entity.CommitMessage = et.CommitMessage;
                entity.CommitDate = et.CommitDate;
                entity.Author = et.Author;
                entity.ProjectWeekId = et.ProjectWeekId;
                entity.Date = et.Date;
                entity.Period = et.Period;
                entity.UpdatedAt = DateTime.Now;

                db.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Error in Update: " + ex.Message);
            }
        }

        public void Delete(int id)
        {
            try
            {
                var query = from c in db.Commits
                            where c.CommitId == id
                            select c;

                var entity = query.SingleOrDefault();
                if (entity == null) return;

                db.Commits.DeleteOnSubmit(entity);
                db.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Error in Delete: " + ex.Message);
            }
        }
    }

}
