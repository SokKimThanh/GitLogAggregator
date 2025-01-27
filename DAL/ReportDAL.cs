using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ET;

namespace DAL
{


    public class ReportDAL
    {
        private GitLogAggregatorDataContext db = new GitLogAggregatorDataContext();

        // Phương thức gọi stored procedure và trả về danh sách kết quả
        public List<WorkHistoryReportET> GetWorkHistoryReport()
        {
            // Gọi stored procedure và ánh xạ kết quả vào model
            var result = db.ExecuteQuery<WorkHistoryReportET>( // Change Database.SqlQuery to ExecuteQuery
                "EXEC sp_GenerateWorkHistoryReport"
            ).ToList();

            return result;
        }

        // Hoặc dùng async (nếu cần)
        public async Task<List<WorkHistoryReportET>> GetWorkHistoryReportAsync()
        {
            var result = await Task.Run(() => db.ExecuteQuery<WorkHistoryReportET>( // Change Database.SqlQuery to ExecuteQuery
                "EXEC sp_GenerateWorkHistoryReport"
            ).ToList());

            return result;
        }
    }
}
