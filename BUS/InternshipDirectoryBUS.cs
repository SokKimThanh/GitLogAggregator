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
        InternshipDirectoryDAL data = new InternshipDirectoryDAL();
        public List<InternshipDirectoryET> GetAllInternshipDirectories()
        {
            return data.GetAllInternshipDirectories();
        }

        public void InsertInternshipDirectory(string folderPath)
        {
            data.InsertInternshipDirectory(folderPath);
        }

        public int GetLatestInternshipDirectoryId()
        {
            return data.GetLatestInternshipDirectoryId();
        }
    }
}
