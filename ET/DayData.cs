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
        public string Day { get; set; }
        public string Session { get; set; }
        public string Attendance { get; set; }
        public string AssignedTasks { get; set; }
        public string AchievedResults { get; set; }
        public string Comments { get; set; }
        public string Notes { get; set; }
    }
}
