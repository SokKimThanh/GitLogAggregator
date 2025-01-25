using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET
{
    public class CommitET
    {
        public int CommitID { get; set; }
        public string CommitHash { get; set; }
        public string CommitMessages { get; set; }
        public DateTime CommitDate { get; set; }
        public int ConfigID { get; set; }
        public int AuthorID { get; set; }
        public int WeekId { get; set; }
        public int PeriodID { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Navigation properties
        public ConfigET ConfigFile { get; set; }
        public AuthorET Author { get; set; }
        public WeekET Week { get; set; }
        public CommitPeriodET CommitPeriod { get; set; }
    }
}
