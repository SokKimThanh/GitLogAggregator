using ET;
using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;

namespace DAL
{
    public class ConfigFileDAL
    {
        private GitLogAggregatorDataContext db = new GitLogAggregatorDataContext();

        public List<ConfigFileET> GetAll()
        {
            try
            {
                var query = from c in db.ConfigFiles
                            orderby c.FirstCommitDate ascending
                            select new ConfigFileET
                            {
                                ConfigID = c.ID,
                                ProjectDirectory = c.ProjectDirectory,
                                InternshipDirectoryId = c.InternshipDirectoryId,
                                Author = c.Author,
                                InternshipStartDate = c.InternshipStartDate,
                                InternshipEndDate = c.InternshipEndDate,
                                Weeks = c.Weeks,
                                FirstCommitDate = c.FirstCommitDate,
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
        /// <summary>
        /// Lấy danh sách tác giả từ bảng ConfigFiles theo id dự án
        /// </summary> 
        public List<string> GetAuthorsByProjectId(int projectId)
        {
            try
            {
                var authors = (from config in db.ConfigFiles
                               where config.ID == projectId
                               select config.Author).Distinct().ToList();

                return authors;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy danh sách tác giả: " + ex.Message);
            }
        }
        public List<string> GetAllAuthors()
        {
            try
            {
                // Truy vấn để lấy danh sách tác giả duy nhất từ bảng ConfigFiles
                var authors = (from c in db.ConfigFiles
                               where c.Author != null // Lọc các giá trị null
                               select c.Author).Distinct().ToList(); // Lấy danh sách tác giả duy nhất

                return authors; // Trả về danh sách tác giả
            }
            catch (Exception ex)
            {
                throw new Exception("Error in GetAllAuthors: " + ex.Message);
            }
        }
        /// <summary>
        /// Lấy ngày bắt đầu thực tập
        /// </summary>
        public DateTime? GetInternshipStartDate(int configId)
        {
            try
            {
                // Truy vấn để lấy ngày bắt đầu thực tập dựa trên ConfigID cấu hình
                var startDate = (from config in db.ConfigFiles
                                 where config.ID == configId
                                 select config.InternshipStartDate).FirstOrDefault();

                // Trả về ngày bắt đầu thực tập (có thể là null nếu không có giá trị)
                return startDate;
            }
            catch (Exception ex)
            {
                // Xử lý ngoại lệ và thông báo lỗi
                throw new Exception("Lỗi khi lấy ngày bắt đầu thực tập: " + ex.Message);
            }
        }

        public ConfigFileET GetByID(int id)
        {
            try
            {
                var query = from c in db.ConfigFiles
                            where c.ID == id
                            select new ConfigFileET
                            {
                                ConfigID = c.ID,
                                ProjectDirectory = c.ProjectDirectory,
                                InternshipDirectoryId = c.InternshipDirectoryId,
                                Author = c.Author,
                                InternshipStartDate = c.InternshipStartDate,
                                InternshipEndDate = c.InternshipEndDate,
                                Weeks = c.Weeks,
                                FirstCommitDate = c.FirstCommitDate,
                                CreatedAt = c.CreatedAt.Value,
                                UpdatedAt = c.UpdatedAt.Value
                            };

                return query.SingleOrDefault();
            }
            catch (Exception ex)
            {
                throw new Exception("Error in GetAuthorByConfig: " + ex.Message);
            }
        }

        public void Add(ConfigFileET et)
        {
            try
            {
                var entity = new ConfigFile
                {
                    ProjectDirectory = et.ProjectDirectory,
                    InternshipDirectoryId = et.InternshipDirectoryId,
                    Author = et.Author,
                    InternshipStartDate = et.InternshipStartDate,
                    InternshipEndDate = et.InternshipEndDate,
                    Weeks = et.Weeks,
                    FirstCommitDate = et.FirstCommitDate,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };

                db.ConfigFiles.InsertOnSubmit(entity);
                db.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Error in Add: " + ex.Message);
            }
        }

        public void Update(ConfigFileET et)
        {
            try
            {
                var query = from c in db.ConfigFiles
                            where c.ID == et.ConfigID
                            select c;

                var entity = query.SingleOrDefault();
                if (entity == null) return;

                entity.ProjectDirectory = et.ProjectDirectory;
                entity.InternshipDirectoryId = et.InternshipDirectoryId;
                entity.Author = et.Author;
                entity.InternshipStartDate = et.InternshipStartDate;
                entity.InternshipEndDate = et.InternshipEndDate;
                entity.Weeks = et.Weeks;
                entity.FirstCommitDate = et.FirstCommitDate;
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
                var query = from c in db.ConfigFiles
                            where c.ID == id
                            select c;

                var entity = query.SingleOrDefault();
                if (entity == null) return;

                db.ConfigFiles.DeleteOnSubmit(entity);
                db.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Error in Delete: " + ex.Message);
            }
        }


    }

}
