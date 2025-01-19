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
                                CommitId = c.CommitId,
                                CommitHash = c.CommitHash,
                                CommitMessage = c.CommitMessage,
                                CommitDate = c.CommitDate,
                                Author = c.Author,
                                AuthorEmail = c.AuthorEmail,
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
        // Tìm kiếm commit theo tên và projectWeekId
        public List<CommitET> SearchCommits(string searchValue, int projectWeekId)
        {
            try
            {
                // Truy vấn dữ liệu bằng LINQ
                var result = (from c in db.Commits
                                  // Tìm kiếm theo CommitMessage
                              where (c.CommitMessage != null && (string.IsNullOrEmpty(searchValue) || c.CommitMessage.Contains(searchValue)))
                              // Lọc theo ProjectWeekId
                                    && c.ProjectWeekId == projectWeekId
                              // Sắp xếp theo ngày tạo giảm dần
                              orderby c.AuthorEmail descending
                              select new CommitET
                              {
                                  CommitId = c.CommitId,
                                  CommitHash = c.CommitHash,
                                  CommitMessage = c.CommitMessage,
                                  CommitDate = c.CommitDate,
                                  Author = c.Author,
                                  AuthorEmail = c.AuthorEmail,
                                  ProjectWeekId = c.ProjectWeekId,
                                  Date = c.Date,
                                  Period = c.Period,
                                  CreatedAt = c.CreatedAt.Value,
                                  UpdatedAt = c.UpdatedAt.Value
                              }).ToList();

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in SearchCommits: " + ex.Message);
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
                            where c.CommitId == id
                            select new CommitET
                            {
                                CommitId = c.CommitId,
                                CommitHash = c.CommitHash,
                                CommitMessage = c.CommitMessage,
                                CommitDate = c.CommitDate,
                                Author = c.Author,
                                AuthorEmail = c.AuthorEmail,
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
                                AuthorEmail = c.AuthorEmail,
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
                    // Nếu tuần thực tập chưa tồn tại, tạo mới
                    var newProjectWeek = new ProjectWeek
                    {
                        ProjectWeekId = c.ProjectWeekId,
                        ProjectWeekName = $"Tuần {c.ProjectWeekId}", // Tên tuần mặc định
                        WeekStartDate = DateTime.Now, // Ngày bắt đầu mặc định
                        WeekEndDate = DateTime.Now.AddDays(6), // Ngày kết thúc mặc định
                        InternshipDirectoryId = 1, // ID thư mục thực tập mặc định
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now
                    };

                    db.ProjectWeeks.InsertOnSubmit(newProjectWeek);
                    db.SubmitChanges();
                }

                // Thêm commit vào database
                var entity = new Commit
                {
                    CommitHash = c.CommitHash,
                    CommitMessage = c.CommitMessage,
                    CommitDate = c.CommitDate,
                    Author = c.Author,
                    AuthorEmail = c.AuthorEmail,
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
                entity.AuthorEmail = et.AuthorEmail;
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
