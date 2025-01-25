using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET
{
    public class InternshipDirectoryET
    {
        public int InternshipDirectoryId { get; set; }
        public string InternshipWeekFolder { get; set; }
        public DateTime InternshipStartDate { get; set; }
        public DateTime InternshipEndDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
