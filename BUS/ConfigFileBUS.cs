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
        ConfigFileDAL data;

        public ConfigFileBUS()
        {
            data = new ConfigFileDAL();
        }

        // Thêm
        public void AddConfigFile(ConfigFileET configFile)
        {
            data.AddConfigFile(configFile);
        }

        // Xóa
        public void DeleteConfigFile(int id)
        {
            data.DeleteConfigFile(id);
        }

        // Sửa
        public void UpdateConfigFile(ConfigFileET configFile, int id)
        {
            data.UpdateConfigFile(configFile, id);
        }

        // Liệt kê
        public List<ConfigFileET> GetAllConfigFiles()
        {
            return data.GetAllConfigFiles();
        }

        // Tìm kiếm
        public ConfigFileET GetConfigFileById(int id)
        {
            return data.GetConfigFileById(id);
        }
    }
}
