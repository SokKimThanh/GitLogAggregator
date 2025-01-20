﻿using DAL;
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
        public ConfigFileET GetLastAddedConfigFile()
        {
            return dal.GetLastAddedConfigFile();
        }
        public DateTime GetInternshipStartDate(int configId)
        {
            return dal.GetInternshipStartDate(configId);
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
