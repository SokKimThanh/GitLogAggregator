﻿using ClosedXML.Excel;
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
namespace DAL
{
    public class GitLogCheckCommitDAL
    {
        public List<string> GetProjectDirectories(string rootDirectory)
        {
            try
            {
                return Directory.GetDirectories(rootDirectory).ToList();

            }
            catch
            {
                throw new Exception("Không tìm thấy thư mục dự án.");
            }
        }

        public List<string> ReadCommitsFromFile(string filePath)
        {
            List<string> commits = new List<string>();

            if (!File.Exists(filePath))
            {
                Console.WriteLine($"Lỗi: File {filePath} không tồn tại.");
                return commits; // Trả về danh sách trống nếu file không tồn tại
            }

            using (StreamReader reader = new StreamReader(filePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    commits.Add(line);
                }
            }

            return commits;
        }



        public string RunGitCommand(string command, string projectDirectory)
        {
            var processStartInfo = new ProcessStartInfo("git", command)
            {
                WorkingDirectory = projectDirectory,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                StandardOutputEncoding = System.Text.Encoding.UTF8
            };
            using (var process = Process.Start(processStartInfo))
            {
                using (var reader = process.StandardOutput)
                {
                    return reader.ReadToEnd();
                }
            }
        }
        /// <summary>
        /// Đảm bảo rằng các giá trị mặc định hoặc "N/A" được sử dụng nếu không có dữ liệu thực tế.
        /// </summary>
        /// <param name="commit"></param>
        /// <returns></returns>
        public CommitItem ParseCommit(string commit)
        {
            var parts = commit.Split(new[] { " - ", ", ", " : " }, StringSplitOptions.None);

            string commitDateStr = parts.Length > 2 ? parts[2] : string.Empty;
            DateTime commitDate;
            bool isValidDate = DateTime.TryParse(commitDateStr, out commitDate);

            return new CommitItem
            {
                Week = GetWeekFromCommitDate(isValidDate ? commitDate : DateTime.Now),
                Day = GetDayOfWeekFromCommitDate(isValidDate ? commitDate : DateTime.Now),
                Session = GetSessionFromCommitDate(isValidDate ? commitDate : DateTime.Now),
                Attendance = parts.Length > 2 ? "Có mặt" : "N/A",
                CommitContent = parts.Length > 3 ? parts[3] : "N/A",
                Comments = parts.Length > 4 ? parts[4] : "Nhận xét N/A",
                Notes = parts.Length > 5 ? parts[5] : "Ghi chú N/A"
            };
        }

        public string GetWeekFromCommitDate(DateTime date)
        {
            // Tính tuần từ ngày
            var cal = System.Globalization.DateTimeFormatInfo.CurrentInfo.Calendar;
            return $"Tuần {cal.GetWeekOfYear(date, System.Globalization.CalendarWeekRule.FirstDay, DayOfWeek.Monday)}";
        }

        public string GetDayOfWeekFromCommitDate(DateTime date)
        {
            return date.ToString("dddd", new System.Globalization.CultureInfo("vi-VN"));
        }

        public string GetSessionFromCommitDate(DateTime date)
        {
            // Giả sử buổi sáng: trước 12h, buổi chiều: từ 12h đến 18h, buổi tối: sau 18h
            if (date.Hour < 12)
                return "Sáng";
            else if (date.Hour < 18)
                return "Chiều";
            else
                return "Tối";
        }

        public void ProcessErrorCommits(List<string> commits, CheckedListBox.CheckedItemCollection checkedItems)
        {
            foreach (var item in checkedItems)
            {
                commits.Remove(item.ToString());
            }
        }


        public void ConfirmDeleteCommits(List<string> commitsToDelete, string filePath, List<string> allCommits)
        {
            foreach (var commit in commitsToDelete)
            {
                allCommits.Remove(commit);
            }

            UpdateLogFile(filePath, allCommits);
            MessageBox.Show("Đã xóa commit lỗi thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }


        public Dictionary<string, List<string>> GroupErrorCommits(List<string> commits)
        {
            var groupedCommits = new Dictionary<string, List<string>>();
            groupedCommits["ErrorCommits"] = new List<string>();

            foreach (var commit in commits)
            {
                // Chuyển đổi commit về chữ thường
                string commitLower = commit.ToLower();

                // Kiểm tra xem commit có chứa từ khóa lỗi hay không
                if (commitLower.Contains("merge"))
                {
                    groupedCommits["ErrorCommits"].Add(commit);
                }
            }

            return groupedCommits;
        }


        public void UpdateLogFile(string filePath, List<string> commits)
        {
            File.WriteAllLines(filePath, commits);
        }
        /// <summary>
        /// Hiển thị commit lên datagridview
        /// </summary>
        /// <param name="commitItems"></param>
        /// <param name="dataGridViewCommits"></param>
        public void DisplayCommits(List<CommitItem> commitItems, DataGridView dataGridViewCommits)
        {
            // Chuyển đổi danh sách CommitItem thành DataTable
            DataTable dataTable = ConvertToDataTable(commitItems);

            // Hiển thị dữ liệu lên DataGridView
            dataGridViewCommits.DataSource = dataTable;
        }



        public void InitializeDataGridView(DataGridView dataGridViewCommits)
        {
            DataTable dataTable = new DataTable();

            // Thêm các cột vào DataTable
            dataTable.Columns.Add("Tuần", typeof(string));
            dataTable.Columns.Add("Thứ", typeof(string));
            dataTable.Columns.Add("Buổi", typeof(string));
            dataTable.Columns.Add("Điểm danh vắng", typeof(string));
            dataTable.Columns.Add("Tên tệp", typeof(string));
            dataTable.Columns.Add("Nội dung commit", typeof(string));
            dataTable.Columns.Add("Ngày commit", typeof(DateTime));
            dataTable.Columns.Add("Nhận xét", typeof(string));
            dataTable.Columns.Add("Trạng thái", typeof(string));
            dataTable.Columns.Add("Ghi chú", typeof(string));

            // Gán DataTable cho DataGridView
            dataGridViewCommits.DataSource = dataTable;

            // Đặt chế độ tự động điều chỉnh cột để chiếm 100% không gian
            dataGridViewCommits.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Định dạng các cột
            foreach (DataGridViewColumn column in dataGridViewCommits.Columns)
            {
                column.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            }
            dataGridViewCommits.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
        }



        public void UpdateCheckedListBox(Dictionary<string, List<string>> groupedCommits, CheckedListBox checkedListBoxCommits)
        {
            checkedListBoxCommits.BeginUpdate();
            try
            {
                checkedListBoxCommits.Items.Clear();

                if (groupedCommits.ContainsKey("ErrorCommits"))
                {
                    foreach (var commit in groupedCommits["ErrorCommits"])
                    {
                        int index = checkedListBoxCommits.Items.Add(commit);
                        checkedListBoxCommits.SetItemChecked(index, true); // Đánh dấu sẵn commit lỗi để xóa
                    }
                }
            }
            finally
            {
                checkedListBoxCommits.EndUpdate();
            }
        }
        public DataTable ConvertToDataTable(List<CommitItem> commitItems)
        {
            DataTable dataTable = new DataTable();

            // Thêm các cột vào DataTable
            dataTable.Columns.Add("Tuần", typeof(string));
            dataTable.Columns.Add("Thứ", typeof(string));
            dataTable.Columns.Add("Buổi", typeof(string));
            dataTable.Columns.Add("Điểm danh vắng", typeof(string));
            dataTable.Columns.Add("Nội dung commit", typeof(string));
            dataTable.Columns.Add("Nhận xét", typeof(string));
            dataTable.Columns.Add("Ghi chú", typeof(string));

            // Thêm dữ liệu vào DataTable
            foreach (var commitItem in commitItems)
            {
                dataTable.Rows.Add(
                    commitItem.Week,
                    commitItem.Day,
                    commitItem.Session,
                    commitItem.Attendance,
                    commitItem.CommitContent,
                    commitItem.Comments,
                    commitItem.Notes
                );
            }

            return dataTable;
        }


    }
}
