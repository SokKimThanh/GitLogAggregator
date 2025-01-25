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
  
    public class ConfigET
    {
        public int ConfigID { get; set; }
        public string ConfigDirectory { get; set; }
        public int ConfigWeeks { get; set; }
        public DateTime FirstCommitDate { get; set; }
        public string FirstCommitAuthor { get; set; }
        public int InternshipDirectoryId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

}
