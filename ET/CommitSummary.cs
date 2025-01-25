using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET
{
    public class CommitSummary
    {
        public int CommitSummaryID { get; set; }
        public int CommitID { get; set; }
        public int SummaryID { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Navigation properties
        public CommitET Commit { get; set; }
        public SummaryET Summary { get; set; }
    }
}
