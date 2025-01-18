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
        public int ID { get; set; }
        public string ProjectDirectory { get; set; }
        public int InternshipDirectoryId { get; set; }
        public string Author { get; set; }
        public DateTime InternshipStartDate { get; set; }
        public DateTime InternshipEndDate { get; set; }
        public int Weeks { get; set; }
        public DateTime FirstCommitDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

}
