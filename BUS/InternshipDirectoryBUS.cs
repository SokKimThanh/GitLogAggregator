using DAL;
using ET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BUS
{

    public class InternshipDirectoryBUS
    {
        private InternshipDirectoryDAL dal = new InternshipDirectoryDAL();
        public int GetLatestInternshipDirectoryId()
        {
            return dal.GetLatestInternshipDirectoryId();
        }
        public List<InternshipDirectoryET> GetAll()
        {
            return dal.GetAll();
        }

        public InternshipDirectoryET GetByID(int id)
        {
            return dal.GetByID(id);
        }

        public InternshipDirectoryET GetByPath(string internshipWeekFolder)
        {
            return dal.GetByPath(internshipWeekFolder);
        }

        public void Add(InternshipDirectoryET entity)
        {
            dal.Add(entity);
        }

        public void Update(InternshipDirectoryET entity)
        {
            dal.Update(entity);
        }

        public void Delete(int id)
        {
            dal.Delete(id);
        }
    }

}
