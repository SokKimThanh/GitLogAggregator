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
        public string ProjectDirectory { get; set; }
        public int? InternshipDirectoryId { get; set; } // Thêm thuộc tính này
        public string InternshipWeekFolder { get; set; }
        public string Author { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int Weeks { get; set; }
        public DateTime? FirstCommitDate { get; set; }
    }
}
