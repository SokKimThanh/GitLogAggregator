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
                InternshipDirectoryId = configFile.InternshipDirectoryId,
                Author = configFile.Author,
                StartDate = configFile.StartDate,
                EndDate = configFile.EndDate,
                Weeks = configFile.Weeks,
                FirstCommitDate = configFile.FirstCommitDate
            };
            db.ConfigFiles.InsertOnSubmit(newConfigFile);
            db.SubmitChanges();
        }

        // Xóa
        public void DeleteConfigFile(int id)
        {
            var configFile = db.ConfigFiles.SingleOrDefault(cf => cf.Id == id);
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
                        where cf.Id == configFile.Id
                        select cf;

            var existingConfigFile = query.SingleOrDefault();
            if (existingConfigFile != null)
            {
                existingConfigFile.ProjectDirectory = configFile.ProjectDirectory;
                existingConfigFile.InternshipDirectoryId = configFile.InternshipDirectoryId;
                existingConfigFile.Author = configFile.Author;
                existingConfigFile.StartDate = configFile.StartDate;
                existingConfigFile.EndDate = configFile.EndDate;
                existingConfigFile.Weeks = configFile.Weeks;
                existingConfigFile.FirstCommitDate = configFile.FirstCommitDate;
                db.SubmitChanges();
            }
        }

        // Liệt kê
        public List<ConfigFileET> GetAllConfigFiles()
        {
            var query = from configFile in db.ConfigFiles
                        join directory in db.InternshipDirectories on configFile.InternshipDirectoryId equals directory.Id into directories
                        from dir in directories.DefaultIfEmpty()
                        select new ConfigFileET
                        {
                            Id = configFile.Id,
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
                        join directory in db.InternshipDirectories on configFile.InternshipDirectoryId equals directory.Id into directories
                        from dir in directories.DefaultIfEmpty()
                        where configFile.Id == id
                        select new ConfigFileET
                        {
                            Id = configFile.Id,
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
