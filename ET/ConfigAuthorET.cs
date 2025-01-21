using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET
{
    public class ConfigAuthorET
    {
        public int ConfigID { get; set; } // Khóa ngoại tham chiếu đến ConfigFiles
        public int AuthorID { get; set; } // Khóa ngoại tham chiếu đến Authors

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
