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
        private ConfigAuthorDAL configAuthorDAL = new ConfigAuthorDAL();

        // Lấy tất cả mối quan hệ ConfigAuthor
        public List<ConfigAuthorET> GetAll()
        {
            return configAuthorDAL.GetAll();
        }

        // Lấy mối quan hệ ConfigAuthor theo ConfigID và AuthorID
        public ConfigAuthorET GetByID(int configID, int authorID)
        {
            return configAuthorDAL.GetByID(configID, authorID);
        }

        // Thêm mối quan hệ ConfigAuthor
        public void Add(ConfigAuthorET configAuthor)
        {
            configAuthorDAL.Add(configAuthor);
        }

        // Xóa mối quan hệ ConfigAuthor
        public void Delete(int configID, int authorID)
        {
            configAuthorDAL.Delete(configID, authorID);
        }

        // Lấy danh sách AuthorID theo ConfigID (phương thức bổ sung)
        public List<int> GetAuthorIDsByConfigID(int configID)
        {
            return configAuthorDAL.GetAuthorIDsByConfigID(configID);
        }

        // Lấy danh sách ConfigID theo AuthorID (phương thức bổ sung)
        public List<int> GetConfigIDsByAuthorID(int authorID)
        {
            return configAuthorDAL.GetConfigIDsByAuthorID(authorID);
        }
    }
}
