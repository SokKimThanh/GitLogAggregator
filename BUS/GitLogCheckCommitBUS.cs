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

        public List<string> GetProjectDirectories(string rootDirectory)
        {
            return data.GetProjectDirectories(rootDirectory);
        }

        public List<string> ReadCommitsFromFile(string filePath)
        {
            return data.ReadCommitsFromFile(filePath);
        }

        public string RunGitCommand(string command, string projectDirectory)
        {
            return data.RunGitCommand(command, projectDirectory);
        }

        public CommitItem ParseCommit(string commit)
        {
            return data.ParseCommit(commit);
        }
        public void CreateExcelReport(string filePath, List<CommitItem> commitItems)
        {
            data.CreateExcelReport(filePath, commitItems);
        }
        public string GetWeekFromCommitDate(DateTime date)
        {
            return data.GetWeekFromCommitDate(date);
        }

        public string GetDayOfWeekFromCommitDate(DateTime date)
        {
            return data.GetDayOfWeekFromCommitDate((DateTime)date);
        }

        public string GetSessionFromCommitDate(DateTime date)
        {
            return data.GetSessionFromCommitDate(date);
        }

        public void ProcessErrorCommits(List<string> commits, CheckedListBox.CheckedItemCollection checkedItems)
        {
            data.ProcessErrorCommits(commits, checkedItems);
        }

        public void UpdateDataGridView(List<string> commits, DataGridView dataGridViewCommits)
        {
            data.UpdateDataGridView(commits, dataGridViewCommits);
        }

        public void ConfirmDeleteCommits(List<string> commitsToDelete, string filePath, List<string> allCommits)
        {
            data.ConfirmDeleteCommits(commitsToDelete, filePath, allCommits);
        }

        public Dictionary<string, List<string>> GroupErrorCommits(List<string> commits)
        {
            return data.GroupErrorCommits(commits);
        }

        public void UpdateLogFile(string filePath, List<string> commits)
        {
            File.WriteAllLines(filePath, commits);
        }
        /// <summary>
        /// Chịu trách nhiệm tạo DataTable và hiển thị dữ liệu lên DataGridView.
        /// </summary>
        /// <param name="groupedCommits"></param>
        /// <param name="dataGridViewCommits"></param>
        public void DisplayCommits(List<CommitItem> commitItems, DataGridView dataGridViewCommits)
        {
            data.DisplayCommits(commitItems, dataGridViewCommits);
        }
        /// <summary>
        /// Chịu trách nhiệm cập nhật CheckedListBox với danh sách commit.
        /// </summary>
        /// <param name="groupedCommits"></param>
        /// <param name="checkedListBoxCommits"></param>
        public void UpdateCheckedListBox(Dictionary<string, List<string>> groupedCommits, CheckedListBox checkedListBoxCommits)
        {
            data.UpdateCheckedListBox(groupedCommits, checkedListBoxCommits);
        }

    }
}
