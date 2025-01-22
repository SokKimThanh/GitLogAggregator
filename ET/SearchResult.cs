using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET
{
    public class SearchResult
    {
        public string CommitHash { get; set; }
        public string CommitMessage { get; set; }
        public DateTime CommitDate { get; set; }
        public string Author { get; set; }
        public string AuthorEmail { get; set; }
        public int ProjectWeekId { get; set; }
        public DateTime Date { get; set; }
        public string Period { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
