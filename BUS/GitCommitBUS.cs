using DAL;
using ET;
using System;
using System.Collections.Generic;

namespace BUS
{
    public class GitCommitBUS
    {
        private readonly GitCommitDAL _gitCommitDAL;

        public List<string> LogMessages => _gitCommitDAL.LogMessages;

        public GitCommitBUS()
        {
            _gitCommitDAL = new GitCommitDAL();
        }

        public List<WeekData> GetCommits(string projectDirectory, string author, DateTime startDate, int weeks)
        {
            return _gitCommitDAL.GetCommits(projectDirectory, author, startDate, weeks);
        }

        public void ExportCommitsToExcel(string filePath, List<WeekData> weekDataList)
        {
            _gitCommitDAL.ExportCommitsToExcel(filePath, weekDataList);
        }
    }
}
