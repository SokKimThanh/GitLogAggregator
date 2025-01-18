using DAL;
using ET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BUS
{
    public class CommitGroupMemberBUS
    {
        private CommitGroupMemberDAL dal = new CommitGroupMemberDAL();

        public List<CommitGroupMemberET> GetAll()
        {
            return dal.GetAll();
        }

        public CommitGroupMemberET GetByID(int groupId, int commitId)
        {
            return dal.GetByID(groupId, commitId);
        }

        public void Add(CommitGroupMemberET entity)
        {
            dal.Add(entity);
        }

        public void Update(CommitGroupMemberET entity)
        {
            dal.Update(entity);
        }

        public void Delete(int groupId, int commitId)
        {
            dal.Delete(groupId, commitId);
        }
    }

}
