using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET
{
    public class SearchResult
    {
        public string ProjectWeekName { get; set; }
        public DateTime WeekStartDate { get; set; }
        public DateTime WeekEndDate { get; set; }
        public string Period { get; set; }
        public string PeriodDuration { get; set; }
        public string Author { get; set; }
        public string AuthorEmail { get; set; }
        public string CommitMessage { get; set; } // Có thể null nếu isSimpleView = true
    }

}
