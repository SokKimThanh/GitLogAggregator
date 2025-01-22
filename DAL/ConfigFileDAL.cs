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
                                ConfigID = c.ConfigID,
                                ProjectDirectory = c.ProjectDirectory,
                                InternshipDirectoryId = c.InternshipDirectoryId,
                                FirstCommitAuthor = c.FirstCommitAuthor,
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
                throw new Exception("Error in GetAll ConfigFileDAL: " + ex.Message);
            }
        }


        /// <summary>
        /// Lấy ngày bắt đầu thực tập
        /// </summary>
        public DateTime GetInternshipStartDate(int configId)
        {
            try
            {
                // Nếu configId = 0, trả về null
                if (configId == 0)
                {
                    return DateTime.Now;
                }

                // Truy vấn để lấy ngày bắt đầu thực tập dựa trên ConfigID cấu hình
                var internshipStartDate = (from config in db.ConfigFiles
                                           where config.ConfigID == configId
                                           select config.InternshipStartDate).FirstOrDefault();

                // Kiểm tra nếu internshipStartDate là DateTime.MinValue (01/01/0001)
                if (internshipStartDate == DateTime.MinValue)
                {
                    return DateTime.Now;
                }

                // Trả về ngày bắt đầu thực tập
                return internshipStartDate;
            }
            catch (Exception ex)
            {
                // Xử lý ngoại lệ và thông báo lỗi
                throw new Exception("Lỗi khi lấy ngày bắt đầu thực tập: " + ex.Message);
            }
        }
        public ConfigFileET GetLastAddedConfigFile()
        {
            // Sử dụng LINQ để lấy đối tượng vừa thêm
            var configFile = db.ConfigFiles
                      .OrderByDescending(cf => cf.ConfigID)
                      .FirstOrDefault(); // Lấy bản ghi đầu tiên

            return new ConfigFileET()
            {
                ConfigID = configFile.ConfigID,
                ProjectDirectory = configFile.ProjectDirectory,
                InternshipDirectoryId = configFile.InternshipDirectoryId,
                FirstCommitAuthor = configFile.FirstCommitAuthor,
                InternshipStartDate = configFile.InternshipStartDate,
                InternshipEndDate = configFile.InternshipEndDate,
                Weeks = configFile.Weeks,
                FirstCommitDate = configFile.FirstCommitDate,
                CreatedAt = configFile.CreatedAt.Value,
                UpdatedAt = configFile.UpdatedAt.Value
            };
        }
        public ConfigFileET GetByID(int configID)
        {
            try
            {
                var query = from c in db.ConfigFiles
                            where c.ConfigID == configID
                            select new ConfigFileET
                            {
                                ConfigID = c.ConfigID,
                                ProjectDirectory = c.ProjectDirectory,
                                InternshipDirectoryId = c.InternshipDirectoryId,
                                FirstCommitAuthor = c.FirstCommitAuthor,
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
                throw new Exception("Error in GetAuthorByConfig ConfigFileDAL: " + ex.Message);
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
                    FirstCommitAuthor = et.FirstCommitAuthor,
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
                throw new Exception("Error in Add ConfigFileDAL: " + ex.Message);
            }
        }

        public void Update(ConfigFileET et)
        {
            try
            {
                var query = from c in db.ConfigFiles
                            where c.ConfigID == et.ConfigID
                            select c;

                var entity = query.SingleOrDefault();
                if (entity == null) return;

                entity.ProjectDirectory = et.ProjectDirectory;
                entity.InternshipDirectoryId = et.InternshipDirectoryId;
                entity.FirstCommitAuthor = et.FirstCommitAuthor;
                entity.InternshipStartDate = et.InternshipStartDate;
                entity.InternshipEndDate = et.InternshipEndDate;
                entity.Weeks = et.Weeks;
                entity.FirstCommitDate = et.FirstCommitDate;
                entity.UpdatedAt = DateTime.Now;

                db.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Error in Update ConfigFileDAL: " + ex.Message);
            }
        }

        public void Delete(int id)
        {
            try
            {
                var query = from c in db.ConfigFiles
                            where c.ConfigID == id
                            select c;

                var entity = query.SingleOrDefault();
                if (entity == null) return;

                db.ConfigFiles.DeleteOnSubmit(entity);
                db.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Error in Delete ConfigFileDAL: " + ex.Message);
            }
        }


    }

}
