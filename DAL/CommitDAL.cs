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
            try
            {
                // Kiểm tra xem CommitHash đã tồn tại chưa
                var isDuplicate = db.Commits.Any(x => x.CommitHash == c.CommitHash);
                if (isDuplicate)
                {
                    throw new Exception("Trùng dữ liệu.");
                }

                // Kiểm tra xem WeekId có tồn tại không
                var projectWeekExists = db.Weeks.Any(pw => pw.WeekId == c.WeekId);
                if (!projectWeekExists)
                {
                    // Nếu tuần thực tập chưa tồn tại, tạo mới
                    var newProjectWeek = new Week
                    {
                        WeekId = c.WeekId,
                        WeekName = $"Tuần {c.WeekId}", // Tên tuần mặc định
                        WeekStartDate = DateTime.Now, // Ngày bắt đầu mặc định
                        WeekEndDate = DateTime.Now.AddDays(6), // Ngày kết thúc mặc định 
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now
                    };

                    db.Weeks.InsertOnSubmit(newProjectWeek);
                    db.SubmitChanges();
                }

                // Thêm commit vào database
                var entity = new Commit
                {
                    CommitHash = c.CommitHash,
                    CommitMessages = c.CommitMessages,
                    CommitDate = c.CommitDate,
                    AuthorID = c.AuthorID,
                    WeekId = c.WeekId,
                    PeriodID = c.PeriodID,
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
                            where c.CommitID == et.CommitID
                            select c;

                var entity = query.SingleOrDefault();
                if (entity == null) return;

                entity.CommitHash = et.CommitHash;
                entity.CommitMessages = et.CommitMessages;
                entity.CommitDate = et.CommitDate;
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
