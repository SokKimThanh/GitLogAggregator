using ET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class ProjectWeekInfoDAL
    {
        private GitLogAggregatorDataContext db = new GitLogAggregatorDataContext();

        // Thêm tuần dự án mới
        public void Create(ProjectWeekInfo projectWeek)
        {
            var projectWeekEntity = new ProjectWeek
            {
                ProjectWeekId = projectWeek.ProjectWeekId,
                ConfigFileId = projectWeek.ConfigFileId,
                InternshipDirectoryId = projectWeek.InternshipDirectoryId,
                CreatedAt = projectWeek.CreatedAt,
                UpdatedAt = projectWeek.UpdatedAt
            };
            db.ProjectWeeks.InsertOnSubmit(projectWeekEntity);
            db.SubmitChanges();
        }


        // Xóa tuần dự án theo ID
        public void Delete(int projectWeekId)
        {
            var projectWeek = db.ProjectWeeks.SingleOrDefault(p => p.ProjectWeekId == projectWeekId);
            if (projectWeek != null)
            {
                db.ProjectWeeks.DeleteOnSubmit(projectWeek);
                db.SubmitChanges();
            }
        }

        // Cập nhật thông tin tuần dự án
        public void Update(ProjectWeekInfo projectWeek)
        {
            var existingWeek = db.ProjectWeeks.SingleOrDefault(p => p.ProjectWeekId == projectWeek.ProjectWeekId);
            if (existingWeek != null)
            {
                existingWeek.ConfigFileId = projectWeek.ConfigFileId;
                existingWeek.InternshipDirectoryId = projectWeek.InternshipDirectoryId;
                existingWeek.CreatedAt = projectWeek.CreatedAt;
                existingWeek.UpdatedAt = projectWeek.UpdatedAt;
                db.SubmitChanges();
            }
        }

        // Lấy tất cả tuần dự án
        public List<ProjectWeekInfo> GetAll()
        {
            return db.ProjectWeeks.Select(p => new ProjectWeekInfo
            {
                ProjectWeekId = p.ProjectWeekId,
                ConfigFileId = p.ConfigFileId,
                InternshipDirectoryId = p.InternshipDirectoryId,
                CreatedAt = (DateTime)p.CreatedAt,
                UpdatedAt = (DateTime)p.UpdatedAt
            }).ToList();
        }

        // Lấy tuần dự án theo ID
        public ProjectWeekInfo GetById(int projectWeekId)
        {
            var projectWeek = db.ProjectWeeks.SingleOrDefault(p => p.ProjectWeekId == projectWeekId);
            if (projectWeek != null)
            {
                return new ProjectWeekInfo
                {
                    ProjectWeekId = projectWeek.ProjectWeekId,
                    ConfigFileId = projectWeek.ConfigFileId,
                    InternshipDirectoryId = projectWeek.InternshipDirectoryId,
                    CreatedAt = (DateTime)projectWeek.CreatedAt,
                    UpdatedAt = (DateTime)projectWeek.UpdatedAt
                };
            }
            return null;
        }
    }

}
