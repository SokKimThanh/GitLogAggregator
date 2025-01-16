using DAL;
using ET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BUS
{
    public class ProjectWeekInfoBUS
    {
        private ProjectWeekInfoDAL projectWeekInfoDAL = new ProjectWeekInfoDAL();

        public void CreateProjectWeek(ProjectWeekInfo projectWeek)
        {
            projectWeekInfoDAL.Create(projectWeek);
        }

        public void DeleteProjectWeek(int projectWeekId)
        {
            projectWeekInfoDAL.Delete(projectWeekId);
        }

        public void UpdateProjectWeek(ProjectWeekInfo projectWeek)
        {
            projectWeekInfoDAL.Update(projectWeek);
        }

        public List<ProjectWeekInfo> GetAllProjectWeeks()
        {
            return projectWeekInfoDAL.GetAll();
        }

        public ProjectWeekInfo GetProjectWeekById(int projectWeekId)
        {
            return projectWeekInfoDAL.GetById(projectWeekId);
        }
    }

}
