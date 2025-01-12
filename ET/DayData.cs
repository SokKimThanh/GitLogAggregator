using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET
{
    /// <summary>
    /// Lớp dữ liệu cho mỗi ngày
    /// </summary>
    public class DayData
    {
        public string DayOfWeek { get; set; }
        public List<SessionData> SessionDataList { get; set; }
        public string Session { get; set; } // 'S', 'C', 'T'
        public string Attendance { get; set; } // 'Có mặt', 'Vắng mặt'
        public string AssignedTasks { get; set; }
        public string AchievedResults { get; set; }
        public string Comments { get; set; }
        public string Notes { get; set; }
    }
}
