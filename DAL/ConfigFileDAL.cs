using ET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class ConfigFileDAL
    {
        private GitLogAggregatorDataContext db;

        public ConfigFileDAL()
        {
            db = new GitLogAggregatorDataContext();
        }

        // Thêm
        public void AddConfigFile(ConfigFileET configFile)
        {
            var newConfigFile = new ConfigFile
            {
                ProjectDirectory = configFile.ProjectDirectory,
                InternshipWeekFolder = configFile.InternshipWeekFolder,
                Author = configFile.Author,
                StartDate = configFile.InternshipStartDate,
                EndDate = configFile.InternshipEndDate,
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
        public void UpdateConfigFile(ConfigFileET configFile, int id)
        {
            var existingConfigFile = db.ConfigFiles.SingleOrDefault(cf => cf.Id == id);
            if (existingConfigFile != null)
            {
                existingConfigFile.ProjectDirectory = configFile.ProjectDirectory;
                existingConfigFile.InternshipWeekFolder = configFile.InternshipWeekFolder;
                existingConfigFile.Author = configFile.Author;
                existingConfigFile.StartDate = configFile.InternshipStartDate;
                existingConfigFile.EndDate = configFile.InternshipEndDate;
                existingConfigFile.Weeks = configFile.Weeks;
                existingConfigFile.FirstCommitDate = configFile.FirstCommitDate;
                db.SubmitChanges();
            }
        }

        // Liệt kê
        public List<ConfigFileET> GetAllConfigFiles()
        {
            return db.ConfigFiles.Select(cf => new ConfigFileET
            {
                Id = cf.Id,
                ProjectDirectory = cf.ProjectDirectory,
                InternshipWeekFolder = cf.InternshipWeekFolder,
                Author = cf.Author,
                InternshipStartDate = (DateTime)cf.StartDate,
                InternshipEndDate = (DateTime)cf.EndDate,
                Weeks = (int)cf.Weeks,
                FirstCommitDate = (DateTime)cf.FirstCommitDate
            }).ToList();
        }

        // Tìm kiếm
        public ConfigFileET GetConfigFileById(int id)
        {
            var configFile = db.ConfigFiles.SingleOrDefault(cf => cf.Id == id);
            if (configFile != null)
            {
                return new ConfigFileET
                {
                    ProjectDirectory = configFile.ProjectDirectory,
                    InternshipWeekFolder = configFile.InternshipWeekFolder,
                    Author = configFile.Author,
                    InternshipStartDate = (DateTime)configFile.StartDate,
                    InternshipEndDate = (DateTime)configFile.EndDate,
                    Weeks = (int)configFile.Weeks,
                    FirstCommitDate = (DateTime)configFile.FirstCommitDate
                };
            }
            return null;
        }
    }
}
