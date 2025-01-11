using ClosedXML.Excel;
using ET;
using System;
using System.Collections.Generic;
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
            return File.ReadAllLines(filePath).ToList();
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

        public CommitItem ParseCommit(string commit)
        {
            var parts = commit.Split(new[] { " - ", ", ", " : " }, StringSplitOptions.None);

            string commitDateStr = parts.Length > 2 ? parts[2] : string.Empty;
            DateTime commitDate;
            bool isValidDate = DateTime.TryParse(commitDateStr, out commitDate);

            return new CommitItem
            {
                Week = "TuầnPlaceholder", // Thay thế bằng giá trị thực tế nếu cần
                Day = "ThứPlaceholder", // Thay thế bằng giá trị thực tế nếu cần
                Session = "BuổiPlaceholder", // Thay thế bằng giá trị thực tế nếu cần
                Attendance = "Vắng", // Thay thế bằng giá trị thực tế nếu cần
                FileName = "FileNamePlaceholder", // Thay thế bằng giá trị thực tế nếu cần
                CommitContent = parts.Length > 3 ? parts[3] : "No content",
                CommitDate = isValidDate ? commitDate : DateTime.Now,
                Status = "Không lỗi",
                Notes = string.Empty // Thay thế bằng giá trị thực tế nếu cần
            };
        }



        public void CreateExcelReport(string filePath, List<CommitItem> commitItems)
        {
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Report");

                worksheet.Cell(1, 1).Value = "Tuần";
                worksheet.Cell(1, 2).Value = "Thứ";
                worksheet.Cell(1, 3).Value = "Buổi";
                worksheet.Cell(1, 4).Value = "Công việc được giao";
                worksheet.Cell(1, 5).Value = "Nội dung – kết quả đạt được";
                worksheet.Cell(1, 6).Value = "Nhận xét – đề nghị";
                worksheet.Cell(1, 7).Value = "Ghi chú";

                int currentRow = 2;
                foreach (var commitItem in commitItems)
                {
                    worksheet.Cell(currentRow, 1).Value = GetWeekFromCommitDate(commitItem.CommitDate);
                    worksheet.Cell(currentRow, 2).Value = GetDayOfWeekFromCommitDate(commitItem.CommitDate);
                    worksheet.Cell(currentRow, 3).Value = GetSessionFromCommitDate(commitItem.CommitDate);
                    worksheet.Cell(currentRow, 4).Value = commitItem.CommitContent;
                    worksheet.Cell(currentRow, 5).Value = commitItem.CommitContent;
                    worksheet.Cell(currentRow, 6).Value = string.Empty;
                    worksheet.Cell(currentRow, 7).Value = string.Empty;
                    currentRow++;
                }

                workbook.SaveAs(filePath);
            }
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

        public void UpdateDataGridView(List<string> commits, DataGridView dataGridViewCommits)
        {
            dataGridViewCommits.Rows.Clear();
            int stt = 1;

            foreach (var commit in commits)
            {
                var commitDetails = ParseCommit(commit);
                dataGridViewCommits.Rows.Add(stt++, commitDetails.FileName, commitDetails.CommitContent, commitDetails.CommitDate, "Không lỗi");
            }
        }
        public void ConfirmDeleteCommits(List<string> commitsToDelete, string filePath, List<string> allCommits, DataGridView dataGridViewCommits, CheckedListBox checkedListBoxCommits)
        {
            var result = MessageBox.Show("Bạn có chắc chắn muốn xóa các commit được chọn?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                foreach (var commit in commitsToDelete)
                {
                    allCommits.Remove(commit);
                }

                UpdateLogFile(filePath, allCommits);
                MessageBox.Show("Đã xóa commit lỗi thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                var groupedCommits = GroupCommits(allCommits);
                DisplayCommits(groupedCommits, dataGridViewCommits, checkedListBoxCommits);
            }
        }

        public Dictionary<string, List<string>> GroupCommits(List<string> commits)
        {
            var groupedCommits = new Dictionary<string, List<string>>();
            foreach (var commit in commits)
            {
                string key = commit.IndexOf("merge", StringComparison.OrdinalIgnoreCase) >= 0 ? "merge" : commit;
                if (!groupedCommits.ContainsKey(key))
                {
                    groupedCommits[key] = new List<string>();
                }
                groupedCommits[key].Add(commit);
            }
            return groupedCommits;
        }

        public void UpdateLogFile(string filePath, List<string> commits)
        {
            File.WriteAllLines(filePath, commits);
        }
        
        public void DisplayCommits(Dictionary<string, List<string>> groupedCommits, DataGridView dataGridViewCommits, CheckedListBox checkedListBoxCommits)
        {
            dataGridViewCommits.Rows.Clear();

            // Tạm dừng cập nhật giao diện của CheckedListBox để tránh lỗi
            checkedListBoxCommits.BeginUpdate();
            try
            {
                checkedListBoxCommits.Items.Clear();
            }
            finally
            {
                // Tiếp tục cập nhật giao diện của CheckedListBox
                checkedListBoxCommits.EndUpdate();
            }

            int stt = 1;

            foreach (var group in groupedCommits)
            {
                if (group.Key == "merge")
                {
                    foreach (var commit in group.Value)
                    {
                        checkedListBoxCommits.Items.Add(commit);
                    }
                }
                else
                {
                    var commitDetails = ParseCommit(group.Value.FirstOrDefault());
                    dataGridViewCommits.Rows.Add(stt++, commitDetails.Week, commitDetails.Day, commitDetails.Session, commitDetails.Attendance, commitDetails.FileName, commitDetails.CommitContent, commitDetails.CommitDate, commitDetails.Status, commitDetails.Notes);
                }
            }
        }



    }
}
