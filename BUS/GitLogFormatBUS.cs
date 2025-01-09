using DAL;
using ET;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace BUS
{
    public class GitLogFormatBUS
    {
        private readonly GitLogFormatDAL data;

        public List<string> LogMessages => data.LogMessages;

        public GitLogFormatBUS()
        {
            data = new GitLogFormatDAL();
        }
        public void CreateExcelFile(string filePath, List<WeekData> commits, DateTime internshipEndDate)
        {
            data.CreateExcelFile(filePath, commits, internshipEndDate);
        }
        public List<WeekData> ConvertDayDataListToWeekDataList(List<DayData> dayDataList, DateTime internshipStartDate, DateTime internshipEndDate)
        {
            return data.ConvertDayDataListToWeekDataList(dayDataList, internshipStartDate, internshipEndDate);
        }


        public string RunGitCommand(string command, string projectDirectory)
        {
            return data.RunGitCommand(command, projectDirectory);
        }

        /// <summary>
        /// Hiển thị danh sách tuần thực tập trên dgv
        /// </summary>
        /// <param name="weekDataList"></param>
        /// <returns></returns>
        public DataTable ConvertToDataTable(List<WeekData> weekDataList)
        {
            return data.ConvertToDataTable(weekDataList);
        }

        /// <summary>
        /// Chuyển về danh sách ngày thực tập
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        public List<DayData> ConvertDataTableToDayDataList(DataTable dataTable)
        {
            return data.ConvertDataTableToDayDataList(dataTable);
        }
    }
}
