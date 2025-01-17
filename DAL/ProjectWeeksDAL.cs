using ET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{


    public class ProjectWeeksDAL
    {
        private GitLogAggregatorDataContext db = new GitLogAggregatorDataContext();

        private CommitDAL commitInfoDAL = new CommitDAL();

        public void SaveProjectWeekAndCommits(ProjectWeekET projectWeek, List<ET.CommitET> commits)
        {
            // Lưu thông tin tuần thực tập
            Create(projectWeek);

            // Lưu thông tin commit
            foreach (var commit in commits)
            {
                commitInfoDAL.Create(commit);
            }
        }

        // Thêm tuần dự án mới
        public int Create(ProjectWeekET projectWeek)
        {
            var projectWeekEntity = new ProjectWeek
            {
                ProjectWeekId = projectWeek.ProjectWeekId,
                InternshipDirectoryId = projectWeek.InternshipDirectoryId,
                CreatedAt = projectWeek.CreatedAt,
                UpdatedAt = projectWeek.UpdatedAt
            };
            db.ProjectWeeks.InsertOnSubmit(projectWeekEntity);
            db.SubmitChanges();
            return projectWeekEntity.ProjectWeekId;
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
        public void Update(ProjectWeekET projectWeek)
        {
            var existingWeek = db.ProjectWeeks.SingleOrDefault(p => p.ProjectWeekId == projectWeek.ProjectWeekId);
            if (existingWeek != null)
            {
                existingWeek.InternshipDirectoryId = projectWeek.InternshipDirectoryId;
                existingWeek.CreatedAt = projectWeek.CreatedAt;
                existingWeek.UpdatedAt = projectWeek.UpdatedAt;
                db.SubmitChanges();
            }
        }

        // Lấy tất cả tuần dự án
        public List<ProjectWeekET> GetAll()
        {
            return db.ProjectWeeks.Select(p => new ProjectWeekET
            {
                ProjectWeekId = p.ProjectWeekId,
                InternshipDirectoryId = p.InternshipDirectoryId,
                CreatedAt = (DateTime)p.CreatedAt,
                UpdatedAt = (DateTime)p.UpdatedAt
            }).ToList();
        }

        // Lấy tuần dự án theo ID
        public ProjectWeekET GetById(int projectWeekId)
        {
            var projectWeek = db.ProjectWeeks.SingleOrDefault(p => p.ProjectWeekId == projectWeekId);
            if (projectWeek != null)
            {
                return new ProjectWeekET
                {
                    ProjectWeekId = projectWeek.ProjectWeekId,
                    InternshipDirectoryId = projectWeek.InternshipDirectoryId,
                    CreatedAt = (DateTime)projectWeek.CreatedAt,
                    UpdatedAt = (DateTime)projectWeek.UpdatedAt
                };
            }
            return null;
        }

        // Hàm lấy thông tin tuần dựa trên khoảng thời gian và ID thư mục thực tập
        public ProjectWeekET GetProjectWeekByDateRangeAndDirectoryId(DateTime startDate, DateTime endDate, int internshipDirectoryId)
        {
            var projectWeek = db.ProjectWeeks.FirstOrDefault(pw =>
                pw.WeekStartDate == startDate && pw.WeekEndDate == endDate && pw.InternshipDirectoryId == internshipDirectoryId
            );

            if (projectWeek != null)
            {
                return new ProjectWeekET
                {
                    ProjectWeekId = projectWeek.ProjectWeekId,
                    InternshipDirectoryId = projectWeek.InternshipDirectoryId,
                    CreatedAt = (DateTime)projectWeek.CreatedAt,
                    UpdatedAt = (DateTime)projectWeek.UpdatedAt,
                    WeekStartDate = (DateTime)projectWeek.WeekStartDate,
                    WeekEndDate = (DateTime)projectWeek.WeekEndDate
                };
            }
            return null;
        }
    } 
}
