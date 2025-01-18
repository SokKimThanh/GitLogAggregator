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
                            orderby c.CreatedAt descending
                            select new ConfigFileET
                            {
                                ID = c.ID,
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

        public ConfigFileET GetByID(int id)
        {
            try
            {
                var query = from c in db.ConfigFiles
                            where c.ID == id
                            select new ConfigFileET
                            {
                                ID = c.ID,
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
                throw new Exception("Error in GetByID: " + ex.Message);
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
                            where c.ID == et.ID
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
