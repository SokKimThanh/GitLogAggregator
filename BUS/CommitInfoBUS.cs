using DAL;
using ET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BUS
{
    public class CommitInfoBUS
    {
        private CommitInfoDAL commitInfoDAL = new CommitInfoDAL();

        public void CreateCommit(CommitInfo commitInfo)
        {
            commitInfoDAL.Create(commitInfo);
        }

        public void DeleteCommit(int commitId)
        {
            commitInfoDAL.Delete(commitId);
        }

        public void UpdateCommit(CommitInfo commitInfo)
        {
            commitInfoDAL.Update(commitInfo);
        }

        public List<CommitInfo> GetAllCommits()
        {
            return commitInfoDAL.GetAll();
        }

        public CommitInfo GetCommitById(int commitId)
        {
            return commitInfoDAL.GetById(commitId);
        }
    }

}
