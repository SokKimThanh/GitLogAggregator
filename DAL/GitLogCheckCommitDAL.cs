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
            // Phân tích commit để lấy thông tin chi tiết
            var parts = commit.Split('-');
            return new CommitItem
            {
                FileName = parts[0].Trim(),
                CommitContent = parts[2].Trim(),
                CommitDate = DateTime.Parse(parts[1].Trim())
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
        public void DisplayCommitsInCheckedListBox(List<string> commits, CheckedListBox checkedListBoxCommits)
        {
            checkedListBoxCommits.Items.Clear();
            foreach (var commit in commits)
            {
                checkedListBoxCommits.Items.Add(commit);
            }
            Console.WriteLine("Hoàn thành danh sách commit!");
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
    }
}
