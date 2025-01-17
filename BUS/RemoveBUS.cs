using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BUS
{
    public class RemoveBUS
    {
        RemoveDAL removeDAL = new RemoveDAL();
        public void ClearAllTables()
        {
            removeDAL.ClearAllTables();
        }
    }
}
