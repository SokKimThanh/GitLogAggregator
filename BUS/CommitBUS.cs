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
         
        public void Dispose()
        {
            dal.Dispose();
        }
    }
}
