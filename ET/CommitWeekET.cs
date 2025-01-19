using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET
{
    public class CommitGroupET
    {
        public int CommitGroupId { get; set; }
        public string GroupName { get; set; } // GroupName = "Week 1 Commits",
        public string TimeRange { get; set; } // TimeRange = "Từ ngày 2025-01-01 đến ngày 2025-01-07",  // Khoảng thời gian

        public DateTime WeekStartDate { get; set; }
        public DateTime WeekEndDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

}
