using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET
{
    /// <summary>
    /// Lưu trữ thông tin tổng hợp về dự án, bao gồm thông tin tác giả, ngày bắt đầu và danh sách thư mục 8 tuần. 
    /// Phù hợp cho việc quản lý thông tin dự án thực tập.
    /// </summary>
    public class ConfigFileET
    {
        public int Id { get; set; }
        /// <summary>
        /// Đường dẫn đến thư mục dự án
        /// </summary>
        public string ProjectDirectory { get; set; }
        /// <summary>
        /// Đường dẫn đến thư mục thực tập
        /// </summary>
        public string InternshipWeekFolder { get; set; }
        /// <summary>
        /// Tác giả thực hiện commit
        /// </summary>
        public string Author { get; set; }
        /// <summary>
        /// Ngày bắt đầu thực tập.
        /// </summary>
        public DateTime StartDate { get; set; }
        /// <summary>
        /// Ngày kết thúc thực tập.
        /// </summary>
        public DateTime EndDate { get; set; }
        /// <summary>
        /// Lưu thêm số tuần thực tập
        /// </summary>
        public int Weeks { get; set; }
       

        /// <summary>
        /// Ngày commit đầu tiên
        /// </summary>
        public DateTime FirstCommitDate { get; set; }
    }
}
