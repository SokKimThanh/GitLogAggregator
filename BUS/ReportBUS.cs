using DAL;
using ET;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BUS
{
    public class ReportBUS
    {
        private ReportDAL _reportDAL = new ReportDAL();

        // Lấy dữ liệu từ stored procedure
        public List<WorkHistoryReportET> GetWorkHistoryData()
        {
            try
            {
                return _reportDAL.GetWorkHistoryReport();
            }
            catch (Exception ex)
            {
                // Xử lý exception (ghi log, ném lại, v.v.)
                throw new Exception("Lỗi khi lấy dữ liệu báo cáo", ex);
            }
        }
    }
}
