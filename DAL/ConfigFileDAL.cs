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

        public List<ConfigET> GetAll()
        {
            try
            {
                var query = from c in db.ConfigFiles
                            orderby c.FirstCommitDate ascending
                            select new ConfigET
                            {
                                ConfigID = c.ConfigID,
                                InternshipDirectoryId = c.InternshipDirectoryId,
                                FirstCommitAuthor = c.FirstCommitAuthor,
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
                                           join internshipDirectory in db.InternshipDirectories
                                           on config.InternshipDirectoryId equals internshipDirectory.InternshipDirectoryId
                                           where config.ConfigID == configId
                                           select internshipDirectory.InternshipStartDate).FirstOrDefault();

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
        public ConfigET GetLastAddedConfigFile()
        {
            // Sử dụng LINQ để lấy đối tượng vừa thêm
            var configFile = db.ConfigFiles
                               .OrderByDescending(cf => cf.ConfigID)
                               .FirstOrDefault(); // Lấy bản ghi đầu tiên

            if (configFile == null) return null;

            return new ConfigET()
            {
                ConfigID = configFile.ConfigID,
                ConfigDirectory = configFile.ConfigDirectory,
                InternshipDirectoryId = configFile.InternshipDirectoryId,
                FirstCommitAuthor = configFile.FirstCommitAuthor,
                ConfigWeeks = configFile.ConfigWeeks,
                FirstCommitDate = configFile.FirstCommitDate,
                CreatedAt = configFile.CreatedAt.HasValue ? configFile.CreatedAt.Value : DateTime.MinValue,
                UpdatedAt = configFile.UpdatedAt.HasValue ? configFile.UpdatedAt.Value : DateTime.MinValue
            };
        }

        public ConfigET GetByID(int configID)
        {
            try
            {
                var query = from c in db.ConfigFiles
                            where c.ConfigID == configID
                            select new ConfigET
                            {
                                ConfigID = c.ConfigID,
                                ConfigDirectory = c.ConfigDirectory,
                                ConfigWeeks = c.ConfigWeeks,
                                InternshipDirectoryId = c.InternshipDirectoryId,
                                FirstCommitAuthor = c.FirstCommitAuthor,
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

        public void Add(ConfigET et)
        {
            try
            {
                var entity = new ConfigFile
                {
                    ConfigDirectory = et.ConfigDirectory,
                    ConfigWeeks = et.ConfigWeeks,
                    InternshipDirectoryId = et.InternshipDirectoryId,
                    FirstCommitAuthor = et.FirstCommitAuthor,
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

        public void Update(ConfigET et)
        {
            try
            {
                var query = from c in db.ConfigFiles
                            where c.ConfigID == et.ConfigID
                            select c;

                var entity = query.SingleOrDefault();
                if (entity == null) return;

                entity.ConfigDirectory = et.ConfigDirectory;
                entity.InternshipDirectoryId = et.InternshipDirectoryId;
                entity.FirstCommitAuthor = et.FirstCommitAuthor;
                entity.ConfigWeeks = et.ConfigWeeks;
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
