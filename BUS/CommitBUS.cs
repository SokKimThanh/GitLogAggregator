using DAL;
using ET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BUS
{
    public class CommitBUS
    {
        private CommitDAL dal = new CommitDAL();

        public List<CommitET> GetAll()
        {
            return dal.GetAll();
        }

        public CommitET GetByID(int id)
        {
            return dal.GetByID(id);
        }

        public void Add(CommitET entity)
        {
            dal.Add(entity);
        }

        public void Update(CommitET entity)
        {
            dal.Update(entity);
        }

        public void Delete(int id)
        {
            dal.Delete(id);
        }

        public CommitET GetLastInserted()
        {
            return dal.GetLastInserted();
        }
        /// <summary>
        /// Bạn có thể sửa lại hàm SearchCommits để nhận một tham số int thay vì bool để đại diện cho việc tìm kiếm tất cả các tuần (1 là tìm kiếm tất cả, 0 là tìm kiếm theo tuần cụ thể).
        /// </summary>
        public List<CommitET> SearchCommits(string searchValue, int projectWeekId, int searchAllWeeks = 0)
        {
            return dal.SearchCommits(searchValue, projectWeekId, searchAllWeeks);
        }

        public void Dispose()
        {
            dal.Dispose();
        }
    }
}
