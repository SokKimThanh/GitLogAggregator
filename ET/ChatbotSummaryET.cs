using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET
{
    public class ChatbotSummaryET
    {
        public int ID { get; set; }
        public int GroupId { get; set; }
        public string Attendance { get; set; }
        public string AssignedTasks { get; set; }
        public string ContentResults { get; set; }
        public string SupervisorComments { get; set; }
        public string Notes { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

}
