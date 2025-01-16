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

        // Thêm
        public void AddConfigFile(ConfigFileET configFile)
        {
            var newConfigFile = new ConfigFile
            {
                ProjectDirectory = configFile.ProjectDirectory,
                InternshipDirectoryId = (int)configFile.InternshipDirectoryId,
                Author = configFile.Author,
                StartDate = (DateTime)configFile.StartDate,
                EndDate = (DateTime)configFile.EndDate,
                Weeks = configFile.Weeks,
                FirstCommitDate = (DateTime)configFile.FirstCommitDate
            };
            db.ConfigFiles.InsertOnSubmit(newConfigFile);
            db.SubmitChanges();
        }

        // Xóa
        public void DeleteConfigFile(int id)
        {
            var configFile = db.ConfigFiles.SingleOrDefault(cf => cf.ID == id);
            if (configFile != null)
            {
                db.ConfigFiles.DeleteOnSubmit(configFile);
                db.SubmitChanges();
            }
        }

        // Sửa
        public void UpdateConfigFile(ConfigFileET configFile)
        {
            var query = from cf in db.ConfigFiles
                        where cf.ID == configFile.Id
                        select cf;

            var existingConfigFile = query.SingleOrDefault();
            if (existingConfigFile != null)
            {
                existingConfigFile.ProjectDirectory = configFile.ProjectDirectory;
                existingConfigFile.InternshipDirectoryId = (int)configFile.InternshipDirectoryId;
                existingConfigFile.Author = configFile.Author;
                existingConfigFile.StartDate = (DateTime)configFile.StartDate;
                existingConfigFile.EndDate = (DateTime)configFile.EndDate;
                existingConfigFile.Weeks = configFile.Weeks;
                existingConfigFile.FirstCommitDate = (DateTime)configFile.FirstCommitDate;
                db.SubmitChanges();
            }
        }

        // Liệt kê
        public List<ConfigFileET> GetAllConfigFiles()
        {
            var query = from configFile in db.ConfigFiles
                        join directory in db.InternshipDirectories on configFile.InternshipDirectoryId equals directory.ID into directories
                        from dir in directories.DefaultIfEmpty()
                        select new ConfigFileET
                        {
                            Id = configFile.ID,
                            ProjectDirectory = configFile.ProjectDirectory,
                            InternshipDirectoryId = configFile.InternshipDirectoryId,
                            InternshipWeekFolder = dir != null ? dir.InternshipWeekFolder : null,
                            Author = configFile.Author,
                            StartDate = configFile.StartDate,
                            EndDate = configFile.EndDate,
                            Weeks = (int)configFile.Weeks,
                            FirstCommitDate = configFile.FirstCommitDate
                        };
            return query.ToList();
        }

        // Tìm kiếm
        public ConfigFileET GetConfigFileById(int id)
        {
            var query = from configFile in db.ConfigFiles
                        join directory in db.InternshipDirectories on configFile.InternshipDirectoryId equals directory.ID into directories
                        from dir in directories.DefaultIfEmpty()
                        where configFile.ID == id
                        select new ConfigFileET
                        {
                            Id = configFile.ID,
                            ProjectDirectory = configFile.ProjectDirectory,
                            InternshipDirectoryId = configFile.InternshipDirectoryId,
                            InternshipWeekFolder = dir != null ? dir.InternshipWeekFolder : null,
                            Author = configFile.Author,
                            StartDate = configFile.StartDate,
                            EndDate = configFile.EndDate,
                            Weeks = (int)configFile.Weeks,
                            FirstCommitDate = configFile.FirstCommitDate
                        };

            return query.SingleOrDefault();
        }
    }
}
