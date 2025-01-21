using DAL;
using ET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BUS
{
    public class ConfigAuthorBUS
    {
        private ConfigAuthorDAL dal = new ConfigAuthorDAL();

        // Lấy tất cả mối quan hệ ConfigAuthor
        public List<ConfigAuthorET> GetAll()
        {
            return dal.GetAll();
        }

        // Lấy mối quan hệ ConfigAuthor theo ConfigID và AuthorID
        public ConfigAuthorET GetByID(int configID, int authorID)
        {
            return dal.GetByID(configID, authorID);
        }

        // Thêm mối quan hệ ConfigAuthor
        public void Add(ConfigAuthorET configAuthor)
        {
            dal.Add(configAuthor);
        }

        // Xóa mối quan hệ ConfigAuthor
        public void Delete(int configID, int authorID)
        {
            dal.Delete(configID, authorID);
        }

        // Lấy danh sách AuthorID theo ConfigID (phương thức bổ sung)
        public List<int> GetAuthorIDsByConfigID(int configID)
        {
            return dal.GetAuthorIDsByConfigID(configID);
        }

        // Lấy danh sách ConfigID theo AuthorID (phương thức bổ sung)
        public List<int> GetConfigIDsByAuthorID(int authorID)
        {
            return dal.GetConfigIDsByAuthorID(authorID);
        }

        public bool Exists(int configID, int authorID)
        {
            return dal.Exists(configID, authorID);
        }
    }
}
