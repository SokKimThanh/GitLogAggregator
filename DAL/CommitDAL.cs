using ET;
using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Data.SqlClient;
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
                                CommitID = c.CommitID,
                                CommitHash = c.CommitHash,
                                CommitMessages = c.CommitMessages,
                                CommitDate = c.CommitDate,
                                ConfigID = c.ConfigID,
                                AuthorID = c.AuthorID,
                                WeekId = c.WeekId,
                                PeriodID = c.PeriodID,
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

        // Giải phóng tài nguyên khi đối tượng CommitDAL bị hủy
        public void Dispose()
        {
            db.Dispose();
        }
        public CommitET GetByID(int id)
        {
            try
            {
                var query = from c in db.Commits
                            where c.CommitID == id
                            select new CommitET
                            {
                                CommitID = c.CommitID,
                                CommitHash = c.CommitHash,
                                CommitMessages = c.CommitMessages,
                                CommitDate = c.CommitDate,
                                ConfigID = c.ConfigID,
                                AuthorID = c.AuthorID,
                                WeekId = c.WeekId,
                                PeriodID = c.PeriodID,
                                CreatedAt = c.CreatedAt.Value,
                                UpdatedAt = c.UpdatedAt.Value
                            };

                return query.SingleOrDefault();
            }
            catch (Exception ex)
            {
                throw new Exception("Error in GetAuthorIDByConfig: " + ex.Message);
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
                                CommitID = c.CommitID,
                                CommitHash = c.CommitHash,
                                CommitMessages = c.CommitMessages,
                                CommitDate = c.CommitDate,
                                ConfigID = c.ConfigID,
                                AuthorID = c.AuthorID,
                                WeekId = c.WeekId,
                                PeriodID = c.PeriodID,
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
            // Kiểm tra trùng CommitHash
            if (db.Commits.Any(x => x.CommitHash == c.CommitHash))
                throw new Exception("Commit này đã tồn tại");

            // Kiểm tra các ID có tồn tại trong bảng khác không
            if (!db.ConfigFiles.Any(cf => cf.ConfigID == c.ConfigID))
                throw new Exception("ConfigID không tồn tại");

            if (!db.Authors.Any(a => a.AuthorID == c.AuthorID))
                throw new Exception("AuthorID không tồn tại");

            if (!db.Weeks.Any(w => w.WeekId == c.WeekId))
                throw new Exception("WeekId không tồn tại");

            if (!db.CommitPeriods.Any(p => p.PeriodID == c.PeriodID))
                throw new Exception("PeriodID không tồn tại");

            // Thêm vào database
            var newCommit = new Commit
            {
                CommitHash = c.CommitHash,
                CommitMessages = c.CommitMessages,
                CommitDate = c.CommitDate,
                ConfigID = c.ConfigID,
                AuthorID = c.AuthorID,
                WeekId = c.WeekId,
                PeriodID = c.PeriodID,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            db.Commits.InsertOnSubmit(newCommit);
            db.SubmitChanges();
        }

        public void Update(CommitET et)
        {
            try
            {
                var query = from c in db.Commits
                            where c.CommitID == et.CommitID
                            select c;

                var entity = query.SingleOrDefault();
                if (entity == null) return;

                entity.CommitHash = et.CommitHash;
                entity.CommitMessages = et.CommitMessages;
                entity.CommitDate = et.CommitDate;
                entity.ConfigID = et.ConfigID;
                entity.AuthorID = et.AuthorID;
                entity.WeekId = et.WeekId;
                entity.PeriodID = et.PeriodID;
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
                            where c.CommitID == id
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
