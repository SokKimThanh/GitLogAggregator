using DAL;
using ET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BUS
{
    public class ProjectWeeksBUS

    {
        private ProjectWeeksDAL data = new ProjectWeeksDAL();

        public int Create(ProjectWeekET projectWeek)
        {
            return data.Create(projectWeek);
        }

        public void Delete(int projectWeekId)
        {
            data.Delete(projectWeekId);
        }

        public void UpdateProjectWeek(ProjectWeekET projectWeek)
        {
            data.Update(projectWeek);
        }

        public List<ProjectWeekET> GetAllProjectWeeks()
        {
            return data.GetAll();
        }

        public ProjectWeekET GetProjectWeekById(int projectWeekId)
        {
            return data.GetById(projectWeekId);
        }
        public void SaveProjectWeekAndCommits(ProjectWeekET projectWeek, List<ET.CommitET> commits)
        {
            data.SaveProjectWeekAndCommits(projectWeek, commits);
        }

        public object GetProjectWeekByDateRangeAndDirectoryId(DateTime weekStartDate, DateTime weekEndDate, int internshipDirectoryId)
        {
            throw new NotImplementedException();
        }
    }
}
