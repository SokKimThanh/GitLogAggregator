using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET
{
    public class CommitGroupMemberET
    {
        public int CommitGroupId { get; set; }
        public int CommitId { get; set; }
        public DateTime AddedAt { get; set; }
    }

}
