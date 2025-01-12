using DAL;
using ET;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BUS
{
    public class GitLogCheckCommitBUS
    {

        readonly GitLogCheckCommitDAL data = new GitLogCheckCommitDAL();

        public DataTable ConvertToDataTable(List<WeekData> weekDataList)
        {
            return data.ConvertToDataTable(weekDataList);
        }

        public void ParseCommitLines(string[] lines, DateTime startDate, DateTime endDate, WeekData weekData, List<string> invalidCommits)
        {
            data.ParseCommitLines(lines, startDate, endDate, weekData, invalidCommits);
        }
    }
}
