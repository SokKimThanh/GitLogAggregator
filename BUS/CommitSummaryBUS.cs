using DAL;
using ET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BUS
{
    public class CommitSummaryBUS
    {
        private readonly CommitSummaryDAL dal = new CommitSummaryDAL();

        // Thêm liên kết Commit-Summary
        public void Add(CommitSummaryET commitSummary)
        {
            dal.Add(commitSummary);
        }
        public bool Exists(int commitId, int summaryId)
        {
            return dal.Exists(commitId, summaryId);
        }
        // Lấy danh sách liên kết theo Commit
        public List<CommitSummaryET> GetByCommitId(int commitId)
        {
            return dal.GetByCommitId(commitId);
        }
        public CommitSummaryET GetBySummaryId(int commitSummaryId)
        {
            return dal.GetBySummaryId(commitSummaryId);
        }
        // Cập nhật liên kết (ít dùng trong quan hệ nhiều-nhiều, thường xóa và thêm mới)
        public void Update(CommitSummaryET commitSummary)
        {
            if (commitSummary == null)
                throw new ArgumentNullException(nameof(commitSummary));

            dal.Update(commitSummary);
        }

        // Xóa liên kết
        public void Delete(int commitSummaryId)
        {
            dal.Delete(commitSummaryId);
        }
    }
}
