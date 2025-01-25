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

        public List<ConfigET> GetAll()
        {
            return dal.GetAll();
        }
        public ConfigET GetByID(int id)
        {
            return dal.GetByID(id);
        }
        public ConfigET GetLastAddedConfigFile()
        {
            return dal.GetLastAddedConfigFile();
        }

        public void Add(ConfigET entity)
        {
            dal.Add(entity);
        }

        public void Update(ConfigET entity)
        {
            dal.Update(entity);
        }

        public void Delete(int id)
        {
            dal.Delete(id);
        }
    }
}
