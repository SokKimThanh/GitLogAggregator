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
    public class GitCommitBUS
    {
        private readonly GitCommitDAL data;

        public List<string> LogMessages => data.LogMessages;

        public GitCommitBUS()
        {
            data = new GitCommitDAL();
        }
        public void CreateExcelFile(string filePath, List<WeekData> commits, DateTime internshipEndDate)
        {
            data.CreateExcelFile(filePath, commits, internshipEndDate);
        }
        public List<WeekData> ConvertToWeekDataList(DataTable dataTable)
        {
            return data.ConvertToWeekDataList(dataTable);
        }


        public string RunGitCommand(string command, string projectDirectory)
        {
            return data.RunGitCommand(command, projectDirectory);
        }
        public DataTable ConvertToDataTable(List<WeekData> weekDataList)
        {
            return data.ConvertToDataTable(weekDataList);
        }
    }
}
