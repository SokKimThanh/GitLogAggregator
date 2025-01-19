using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET
{
    public class CommitPeriodET
    {
        public int PeriodID { get; set; }
        public string PeriodName { get; set; } // PeriodName = "Week 1 Commits",
        public string PeriodDuration { get; set; } // PeriodDuration = "Từ ngày 2025-01-01 đến ngày 2025-01-07",  // Khoảng thời gian

        public DateTime PeriodStartDate { get; set; }
        public DateTime PeriodEndDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

}
