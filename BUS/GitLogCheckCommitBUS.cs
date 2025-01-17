using DAL;
using ET;
using GitLogAggregator.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BUS
{
    public class GitLogCheckCommitBUS
    {

        readonly GitCheckCommitDAL data = new GitCheckCommitDAL();

        public DataTable ConvertToDataTable(List<WeekData> weekDataList)
        {
            return data.ConvertToDataTable(weekDataList);
        }

        /// <summary>
        /// Xử lý dữ liệu trong file combined_commits.txt của một tuần
        /// </summary>
        public void ProcessCommitsInWeek(string filePath, int week, DateTime startDate, DateTime endDate, List<WeekData> weekDatas, List<string> invalidCommits)
        {
            data.ProcessCommitsInWeek(filePath, week, startDate, endDate, weekDatas, invalidCommits);
        }

        /// <summary>
        /// Phân tích nội dung từng dòng trong file combined_commits.txt
        /// </summary>
        public void ParseCommitLines(string[] lines, DateTime startDate, DateTime endDate, WeekData weekData, List<string> invalidCommits)
        {
            data.ParseCommitLines(lines, startDate, endDate, weekData, invalidCommits);
        }
        /// <summary>
        /// Cài đặt datagridview
        /// </summary>        
        public void InitializeDataGridView(DataGridView dataGridViewCommits)
        {
            data.InitializeDataGridView(dataGridViewCommits);
        }
    }
}
