using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET
{
    public class AuthorET
    {
        public int AuthorID { get; set; } // Khóa chính
        public string AuthorName { get; set; } // Tên tác giả
        public string AuthorEmail { get; set; } // Email tác giả
        public DateTime CreatedAt { get; set; } // Ngày tạo
        public DateTime UpdatedAt { get; set; } // Ngày cập nhật
    }
}
