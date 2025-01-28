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
                                ConfigDirectory = c.ConfigDirectory,
                                InternshipDirectoryId = c.InternshipDirectoryId,
                                FirstCommitAuthor = c.FirstCommitAuthor,
                                FirstCommitDate = c.FirstCommitDate.Value,
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
                ConfigWeeks = configFile.ConfigWeeks.Value,
                FirstCommitDate = configFile.FirstCommitDate.Value,
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
                                ConfigWeeks = c.ConfigWeeks.Value,
                                InternshipDirectoryId = c.InternshipDirectoryId,
                                FirstCommitAuthor = c.FirstCommitAuthor,
                                FirstCommitDate = c.FirstCommitDate.Value,
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
