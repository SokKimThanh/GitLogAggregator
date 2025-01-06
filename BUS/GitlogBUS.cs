﻿using System;
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

        public void RunGitCommand(string command, string outputFile, string projectDirectory)
        {
            dataAccess.RunGitCommand(command, outputFile, projectDirectory);
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

        public void CreateBatchFile(string filePath, string command, string projectDirectory)
        {
            dataAccess.CreateBatchFile(filePath, command, projectDirectory);
        }

    }
}