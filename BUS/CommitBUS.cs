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
        private CommitDAL commitInfoDAL = new CommitDAL();

        public void Create(CommitET commitInfo)
        {
            commitInfoDAL.Create(commitInfo);
        }

        public void Delete(int commitId)
        {
            commitInfoDAL.Delete(commitId);
        }

        public void Update(CommitET commitInfo)
        {
            commitInfoDAL.Update(commitInfo);
        }

        public List<CommitET> GetAll()
        {
            return commitInfoDAL.GetAll();
        }

        public CommitET GetById(int commitId)
        {
            return commitInfoDAL.GetById(commitId);
        }
    }

}
