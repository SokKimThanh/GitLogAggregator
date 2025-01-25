using DAL;
using ET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BUS
{
    public class CommitPeriodBUS
    {
        private CommitPeriodDAL dal = new CommitPeriodDAL();

        public List<CommitPeriodET> GetAll()
        {
            return dal.GetAll();
        }

        public CommitPeriodET GetByID(int id)
        {
            return dal.GetByID(id);
        }

        public void Add(CommitPeriodET entity)
        {
            dal.Add(entity);
        }

        public void Update(CommitPeriodET entity)
        {
            dal.Update(entity);
        }

        public void Delete(int id)
        {
            dal.Delete(id);
        }
        /// <summary>
        /// Lấy thông tin của bản ghi cuối cùng được thêm vào
        /// </summary> 
        public CommitPeriodET GetLastInserted()
        {
            return dal.GetLastInserted();
        }
        public CommitPeriod GetCommitPeriod(DateTime commitDate)
        {
            return dal.GetCommitPeriod(commitDate);
        }
    }

}
