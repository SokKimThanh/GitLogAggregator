using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using ET;
using GitLogAggregator.DataAccess;

namespace GitLogAggregator.BusinessLogic
{
    public class GitlogBUS
    {
        public GitlogDAL data = new GitlogDAL();


        /// <summary>
        /// Tính số tuần từ ngày bắt đầu thực tập đến ngày bắt đầu dự án
        /// </summary>
        /// <param name="internshipStartDate"></param>
        /// <param name="projectStartDate"></param>
        /// <returns></returns>
        public int CalculateWeekNumber(DateTime internshipStartDate, DateTime projectStartDate)
        {
            return data.CalculateWeekNumber(internshipStartDate, projectStartDate);
        }
        public string RunGitCommand(string command, string projectDirectory)
        {
            return data.RunGitCommand(command, projectDirectory);
        }

        public List<string> GetGitAuthors(string projectDirectory)
        {
            return data.GetGitAuthors(projectDirectory);
        }

        /// <summary>
        /// Lưu thông tin tổng hợp
        /// </summary>
        /// <param name="aggregateInfo"></param>
        public void SaveAggregateInfo(AggregateInfo aggregateInfo)
        {
            data.SaveAggregateInfo(aggregateInfo);
        }
        /// <summary>
        /// Tải thông tin tổng hợp
        /// </summary>
        /// <param name="aggregateInfoPath">thông tin cấu hình commit tổng hợp</param>
        /// <returns></returns>
        public AggregateInfo LoadAggregateInfo(string aggregateInfoPath)
        {
            return data.LoadAggregateInfo(aggregateInfoPath);
        }

        /// <summary>
        /// Lệnh Git tìm ngày commit đầu tiên
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public DateTime GetProjectStartDate(string projectDirectory)
        {
            return data.GetProjectStartDate(projectDirectory);
        }


        public List<string> AggregateCommits(string projectDirectory, string author, DateTime internshipStartDate, string internshipWeekFolder)
        {
            return data.AggregateCommits(projectDirectory, author, internshipStartDate, internshipWeekFolder);
        }

        /// <summary>
        /// Hiển thị danh sách tác giả trên combobox
        /// </summary>
        public List<string> LoadAuthorsCombobox(string projectDirectory)
        {
            return data.LoadAuthorsCombobox(projectDirectory);
        }

        public List<DayData> GetCommits(string projectDirectory, string author, DateTime internshipStartDate, DateTime internshipEndDate)
        {
            return data.GetCommits(projectDirectory, author, internshipStartDate, internshipEndDate);
        }

        public DateTime CalculateEndDate(DateTime startDate, int weeks)
        {
            return data.CalculateEndDate(startDate, weeks);
        }
        public DataTable ConvertDayDataListToDataTable(List<DayData> dayDataList)
        {
            return data.ConvertDayDataListToDataTable(dayDataList);
        }
     
    }
}