using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET
{
    public class ProjectWeekInfo
    {
        public int ProjectWeekId { get; set; }
        public int ConfigFileId { get; set; }
        public int InternshipDirectoryId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

}
