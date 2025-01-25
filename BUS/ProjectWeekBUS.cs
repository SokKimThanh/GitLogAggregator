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

        public List<WeekET> GetAll()
        {
            return dal.GetAll();
        }

        public WeekET GetByID(int id)
        {
            return dal.GetByID(id);
        }

        public void Add(WeekET entity)
        {
            dal.Add(entity);
        }

        public void Update(WeekET entity)
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
        public WeekET GetLastInserted()
        {

            return dal.GetLastInserted();
        }
        public bool IsInternshipWeekListCreated()
        {
            return dal.IsInternshipWeekListCreated();
        }
    }
}
