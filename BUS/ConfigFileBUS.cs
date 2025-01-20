using DAL;
using ET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BUS
{
    public class ConfigFileBUS
    {
        private ConfigFileDAL dal = new ConfigFileDAL();

        public List<ConfigFileET> GetAll()
        {
            return dal.GetAll();
        }
        public ConfigFileET GetByID(int id)
        {
            return dal.GetByID(id);
        }
        public List<string> GetAllAuthors()
        {
            return dal.GetAllAuthors();
        }
        /// <summary>
        /// Lấy danh sách tác giả từ bảng ConfigFiles theo id dự án
        /// </summary> 
        public List<string> GetAuthorsByProjectId(int projectId)
        {
            return dal.GetAuthorsByProjectId(projectId);
        }

        public DateTime GetInternshipStartDate(int configId)
        {
            return dal.GetInternshipStartDate(configId).Value;
        }

        public void Add(ConfigFileET entity)
        {
            dal.Add(entity);
        }

        public void Update(ConfigFileET entity)
        {
            dal.Update(entity);
        }

        public void Delete(int id)
        {
            dal.Delete(id);
        }
    }
}
