using DAL;
using DocumentFormat.OpenXml.Wordprocessing;
using ET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class ProjectWeekDAL
    {
        private GitLogAggregatorDataContext db = new GitLogAggregatorDataContext();
        public List<ProjectWeekET> GetAll()
        {
            try
            {
                var query = from p in db.ProjectWeeks
                            orderby p.ProjectWeekName ascending
                            select new ProjectWeekET
                            {
                                ProjectWeekId = p.ProjectWeekId,
                                ProjectWeekName = p.ProjectWeekName,
                                WeekStartDate = p.WeekStartDate.Value,
                                WeekEndDate = p.WeekEndDate.Value,
                                InternshipDirectoryId = p.InternshipDirectoryId,
                                CreatedAt = p.CreatedAt.Value,
                                UpdatedAt = p.UpdatedAt.Value
                            };

                return query.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error in GetAll: " + ex.Message);
            }
        }

        public ProjectWeekET GetByID(int id)
        {
            try
            {
                var query = from p in db.ProjectWeeks
                            where p.ProjectWeekId == id
                            select new ProjectWeekET
                            {
                                ProjectWeekId = p.ProjectWeekId,
                                ProjectWeekName = p.ProjectWeekName,
                                WeekStartDate = p.WeekStartDate.Value,
                                WeekEndDate = p.WeekEndDate.Value,
                                InternshipDirectoryId = p.InternshipDirectoryId,
                                CreatedAt = p.CreatedAt.Value,
                                UpdatedAt = p.UpdatedAt.Value
                            };

                return query.SingleOrDefault();
            }
            catch (Exception ex)
            {
                throw new Exception("Error in GetByID: " + ex.Message);
            }
        }

        public void Add(ProjectWeekET et)
        {
            try
            {
                var entity = new ProjectWeek
                {
                    ProjectWeekName = et.ProjectWeekName,
                    WeekStartDate = et.WeekStartDate,
                    WeekEndDate = et.WeekEndDate,
                    InternshipDirectoryId = et.InternshipDirectoryId,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };
                db.ProjectWeeks.InsertOnSubmit(entity);
                db.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Error in Add: " + ex.Message);
            }
        }
        /// <summary>
        /// Lấy ra bản ghi cuối cùng
        /// </summary> 
        public ProjectWeekET GetLastInserted()
        {

            var projectweek = db.ProjectWeeks
                          .OrderByDescending(pw => pw.ProjectWeekId) // Giả sử Id là khóa chính, tự tăng
                          .FirstOrDefault();

            if (projectweek != null)
            {
                return new ProjectWeekET()
                {
                    ProjectWeekId = projectweek.ProjectWeekId,
                    ProjectWeekName = projectweek.ProjectWeekName,
                    WeekStartDate = projectweek.WeekStartDate.Value,
                    WeekEndDate = projectweek.WeekEndDate.Value,
                    InternshipDirectoryId = projectweek.InternshipDirectoryId,
                    CreatedAt = projectweek.CreatedAt.Value,
                    UpdatedAt = projectweek.UpdatedAt.Value
                };
            }
            return null;
        }

        public void Update(ProjectWeekET et)
        {
            try
            {
                var query = from p in db.ProjectWeeks
                            where p.ProjectWeekId == et.ProjectWeekId
                            select p;

                var entity = query.SingleOrDefault();
                if (entity == null) return;
                entity.ProjectWeekName = et.ProjectWeekName;
                entity.WeekStartDate = et.WeekStartDate;
                entity.WeekEndDate = et.WeekEndDate;
                entity.InternshipDirectoryId = et.InternshipDirectoryId;
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
                var query = from p in db.ProjectWeeks
                            where p.ProjectWeekId == id
                            select p;

                var entity = query.SingleOrDefault();
                if (entity == null) return;

                db.ProjectWeeks.DeleteOnSubmit(entity);
                db.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Error in Delete: " + ex.Message);
            }
        }

    }
}