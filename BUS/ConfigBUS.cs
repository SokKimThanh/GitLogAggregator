using DAL;
using ET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BUS
{
    public class ConfigBUS
    {
        ConfigFileDAL data;

        public ConfigBUS()
        {
            data = new ConfigFileDAL();
        }

        // Thêm
        public void AddConfigFile(ConfigET configFile)
        {
            data.AddConfigFile(configFile);
        }

        // Xóa
        public void DeleteConfigFile(int id)
        {
            data.DeleteConfigFile(id);
        }

        // Sửa
        public void UpdateConfigFile(ConfigET configFile)
        {
            data.UpdateConfigFile(configFile);
        }

        // Liệt kê
        public List<ConfigET> GetAllConfigFiles()
        {
            return data.GetAllConfigFiles();
        }

        // Tìm kiếm
        public ConfigET GetConfigFileById(int id)
        {
            return data.GetConfigFileById(id);
        }
    }
}
