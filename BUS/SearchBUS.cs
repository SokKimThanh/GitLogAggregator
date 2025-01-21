using DAL;
using ET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BUS
{
    public class SearchBUS
    {
        private readonly SearchDAL _searchDAL = new SearchDAL();

        public List<SearchResult> SearchCommits(string keyword, int projectWeekId, bool searchAllWeeks, DateTime? minDate, DateTime? maxDate, string author = null)
        {
            try
            {
                // Gọi DAL để truy xuất dữ liệu
                return _searchDAL.SearchCommits(keyword, projectWeekId, searchAllWeeks, minDate, maxDate, author);
            }
            catch (Exception ex)
            {
                // Xử lý ngoại lệ và ghi log nếu cần
                throw new ApplicationException("Đã xảy ra lỗi khi tìm kiếm dữ liệu.", ex);
            }
        }
        public DateTime? GetFirstCommitDateByProject(int projectId)
        {
            try
            {
                return _searchDAL.GetFirstCommitDateByProject(projectId);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Đã xảy ra lỗi khi lấy ngày commit đầu tiên.", ex);
            }
        }
    }
}
