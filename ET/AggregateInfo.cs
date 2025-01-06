using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET
{
    /// <summary>
    /// Thông tin commit tổng hợp
    /// </summary>
    public class AggregateInfo
    {
        /// <summary>
        /// Tác giả thực hiện commit
        /// </summary>
        public string Author { get; set; }
        /// <summary>
        /// internshipStartDate
        /// </summary>
        public DateTime StartDate { get; set; }
        /// <summary>
        /// Danh sách thư mục 8 tuần
        /// </summary>
        public List<string> Folders { get; set; }

        /// <summary>
        /// Đường dẫn gốc
        /// </summary>
        public string ProjectDirectory { get; set; }
    }
}
