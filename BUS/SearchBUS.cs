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

        public List<SearchResult> SearchCommits(
        string keyword,
        int? projectWeekId,
        bool searchAllWeeks,
        bool searchAllAuthors,
        //DateTime? minDate,
        //DateTime? maxDate,
        //out bool requireUserConfirmation, // Thêm tham số để thông báo cần xác nhận
        //out DateTime? internshipEndDate, // Trả về ngày kết thúc thực tập
        int? author = 0)
        {
            //requireUserConfirmation = false;
            //internshipEndDate = _searchDAL.GetInternshipEndDate(projectWeekId);
            //DateTime effectiveMaxDate = maxDate ?? DateTime.Now;

            //if (internshipEndDate.HasValue)
            //{
            //    if (DateTime.Now > internshipEndDate.Value && !maxDate.HasValue)
            //    {
            //        // Đánh dấu cần hỏi người dùng
            //        requireUserConfirmation = true;
            //        return new List<SearchResult>(); // Trả về danh sách rỗng tạm thời
            //    }
            //    else if (DateTime.Now <= internshipEndDate.Value)
            //    {
            //        effectiveMaxDate = DateTime.Now;
            //    }
            //}

            //requireUserConfirmation = false;
            return _searchDAL.SearchCommits(
                //keyword, projectWeekId, searchAllWeeks, searchAllAuthors,
                //minDate, effectiveMaxDate, 
                //author
            );
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
