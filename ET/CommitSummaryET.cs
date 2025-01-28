using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET
{
    public class CommitSummaryET
    {
        public int CommitSummaryID { get; set; }
        public int CommitID { get; set; }
        public int SummaryID { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
