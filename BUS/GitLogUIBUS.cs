using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using ET;
using GitLogAggregator.DataAccess;

namespace GitLogAggregator.BusinessLogic
{
    public class GitLogUIBUS
    {
        public GitlogUIDAL data = new GitlogUIDAL();


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
        public string GetFirstCommitAuthor(string folderPath)
        {
            return data.GetFirstCommitAuthor(folderPath);
        }
        public List<string> GetAuthorsFromRepository(string projectDirectory)
        {
            return data.GetAuthorsFromRepository(projectDirectory);
        }
        /// <summary>
        /// Lệnh Git tìm ngày commit đầu tiên
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public DateTime GetFirstCommitDate(string folderPath)
        {
            return data.GetFirstCommitDate(folderPath);
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
        public List<string> ReadCommitsFromFile(string filePath)
        {
            return data.ReadCommitsFromFile(filePath);
        }
    }
}