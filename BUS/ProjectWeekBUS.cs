using DAL;
using ET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BUS
{
    public class ProjectWeekBUS
    {
        private ProjectWeekDAL dal = new ProjectWeekDAL();

        public List<ProjectWeekET> GetAll()
        {
            return dal.GetAll();
        }

        public ProjectWeekET GetByID(int id)
        {
            return dal.GetByID(id);
        }

        public void Add(ProjectWeekET entity)
        {
            dal.Add(entity);
        }

        public void Update(ProjectWeekET entity)
        {
            dal.Update(entity);
        }

        public void Delete(int id)
        {
            dal.Delete(id);
        }
        /// <summary>
        /// Lấy ra bản ghi cuối cùng
        /// </summary> 
        public ProjectWeekET GetLastInserted()
        {

            return dal.GetLastInserted();
        }
        public bool IsInternshipWeekListCreated()
        {
            return dal.IsInternshipWeekListCreated();
        }
    }
}
