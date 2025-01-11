using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET
{
    public class CommitItem
    {
        public string Week { get; set; }
        public string Day { get; set; }
        public string Session { get; set; }
        public string Attendance { get; set; }
        public string FileName { get; set; }
        public string CommitContent { get; set; }
        public DateTime CommitDate { get; set; }
        public string Status { get; set; }
        public string Notes { get; set; }
    }

}
