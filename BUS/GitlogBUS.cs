using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using ET;
using GitLogAggregator.DataAccess;

namespace GitLogAggregator.BusinessLogic
{
    public class GitlogBUS
    {
        public GitlogDAL dataAccess = new GitlogDAL();


        /// <summary>
        /// Tính số tuần từ ngày bắt đầu thực tập đến ngày bắt đầu dự án
        /// </summary>
        /// <param name="internshipStartDate"></param>
        /// <param name="projectStartDate"></param>
        /// <returns></returns>
        public int CalculateWeekNumber(DateTime internshipStartDate, DateTime projectStartDate)
        {
            return dataAccess.CalculateWeekNumber(internshipStartDate, projectStartDate);
        }
        public string RunGitCommand(string command, string projectDirectory)
        {
            return dataAccess.RunGitCommand(command, projectDirectory);
        }

        public List<string> GetGitAuthors(string projectDirectory)
        {
            return dataAccess.GetGitAuthors(projectDirectory);
        }

        /// <summary>
        /// Lưu thông tin tổng hợp
        /// </summary>
        /// <param name="aggregateInfo"></param>
        public void SaveAggregateInfo(AggregateInfo aggregateInfo)
        {
            dataAccess.SaveAggregateInfo(aggregateInfo);
        }
        /// <summary>
        /// Tải thông tin tổng hợp
        /// </summary>
        /// <param name="aggregateInfoPath">thông tin cấu hình commit tổng hợp</param>
        /// <returns></returns>
        public AggregateInfo LoadAggregateInfo(string aggregateInfoPath)
        {
            return dataAccess.LoadAggregateInfo(aggregateInfoPath);
        }

        /// <summary>
        /// Lệnh Git tìm ngày commit đầu tiên
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public DateTime GetProjectStartDate(string projectDirectory)
        {
            return dataAccess.GetProjectStartDate(projectDirectory);
        }

        
        public List<string> AggregateCommits(string projectDirectory, string author, DateTime internshipStartDate, string internshipWeekFolder)
        {
            return dataAccess.AggregateCommits(projectDirectory, author, internshipStartDate, internshipWeekFolder);
        }
    }
}