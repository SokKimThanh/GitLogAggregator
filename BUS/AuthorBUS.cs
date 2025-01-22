using DAL;
using ET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BUS
{
    public class AuthorBUS
    {
        private AuthorDAL authorDAL = new AuthorDAL();

        // Lấy tất cả tác giả
        public List<AuthorET> GetAll()
        {
            return authorDAL.GetAll();
        }

        // Lấy tác giả theo ChatbotSummaryID
        public AuthorET GetByID(int authorID)
        {
            return authorDAL.GetByID(authorID);
        }

        // Thêm tác giả mới
        public void Add(AuthorET author)
        {
            authorDAL.Add(author);
        }

        // Cập nhật thông tin tác giả
        public void Update(AuthorET author)
        {
            authorDAL.Update(author);
        }

        // Xóa tác giả
        public void Delete(int authorID)
        {
            authorDAL.Delete(authorID);
        }

        // Lấy tác giả theo tên (phương thức bổ sung)
        public AuthorET GetByName(string authorName)
        {
            return authorDAL.GetByName(authorName);
        }
        public AuthorET GetByEmail(string authorEmail)
        {
            return authorDAL.GetByEmail(authorEmail);
        }
    }
}