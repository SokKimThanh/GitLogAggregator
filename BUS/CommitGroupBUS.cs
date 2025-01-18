using DAL;
using ET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BUS
{
    public class CommitGroupBUS
    {
        private CommitGroupDAL dal = new CommitGroupDAL();

        public List<CommitGroupET> GetAll()
        {
            return dal.GetAll();
        }

        public CommitGroupET GetByID(int id)
        {
            return dal.GetByID(id);
        }

        public void Add(CommitGroupET entity)
        {
            dal.Add(entity);
        }

        public void Update(CommitGroupET entity)
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
        public CommitGroupET GetLastInserted()
        {
            return dal.GetLastInserted();
        }
    }

}
