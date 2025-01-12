using ClosedXML.Excel;
using ET;
using GitLogAggregator.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace DAL
{
    public class GitLogCheckCommitDAL
    {

        // Method to convert List<WeekData> to DataTable
        public DataTable ConvertToDataTable(List<WeekData> weekDataList)
        {
            // Create a new DataTable with specified columns
            var dataTable = new DataTable();
            dataTable.Columns.Add("Tuần", typeof(int));
            dataTable.Columns.Add("Thứ", typeof(string));
            dataTable.Columns.Add("Buổi", typeof(string));
            dataTable.Columns.Add("Điểm danh vắng", typeof(string));
            dataTable.Columns.Add("Công việc được giao", typeof(string));
            dataTable.Columns.Add("Nội dung – kết quả đạt được", typeof(string));
            dataTable.Columns.Add("Nhận xét - đề nghị của người hướng dẫn tại doanh nghiệp", typeof(string));
            dataTable.Columns.Add("Ghi chú", typeof(string));

            // Iterate through each WeekData and DayData to add rows to the DataTable
            foreach (var weekData in weekDataList)
            {
                foreach (var dayData in weekData.DayDataList)
                {
                    dataTable.Rows.Add(
                        weekData.WeekNumber,
                        dayData.DayOfWeek,
                        dayData.Session,
                        dayData.Attendance,
                        dayData.AssignedTasks,
                        dayData.AchievedResults,
                        dayData.Comments,
                        dayData.Notes
                    );
                }
            }

            return dataTable;
        }

        /// <summary>
        /// Xử lý dữ liệu trong file combined_commits.txt của một tuần
        /// </summary>
        public void ProcessCommitsInWeek(string filePath, int week, DateTime startDate, DateTime endDate, List<WeekData> weekDatas, List<string> invalidCommits)
        {
            // Đọc toàn bộ nội dung file combined_commits.txt
            var lines = File.ReadAllLines(filePath);

            // Tạo đối tượng WeekData để lưu trữ thông tin của tuần hiện tại
            var weekData = new WeekData
            {
                WeekNumber = week,
                DayDataList = new List<DayData>()
            };

            // Danh sách commit không hợp lệ trong tuần này
            var invalidCommitsThisWeek = new List<string>();

            // Phân tích từng dòng trong file combined_commits.txt
            ParseCommitLines(lines, startDate, endDate, weekData, invalidCommitsThisWeek);

            // Nếu có commit hợp lệ, tính toán ngày bắt đầu và kết thúc của tuần
            if (weekData.DayDataList.Any())
            {
                var dates = weekData.DayDataList.Select(dd => DateHelpers.ParseDayOfWeek(dd.DayOfWeek).GetDateFromDayOfWeek(startDate));
                weekData.StartDate = dates.Min();
                weekData.EndDate = dates.Max();
            }

            // Thêm dữ liệu tuần vào danh sách weekDatas
            weekDatas.Add(weekData);

            // Thêm commit không hợp lệ vào danh sách chung
            invalidCommits.AddRange(invalidCommitsThisWeek);
        }

        /// <summary>
        /// Phân tích nội dung từng dòng trong file combined_commits.txt
        /// </summary>
        public void ParseCommitLines(string[] lines, DateTime startDate, DateTime endDate, WeekData weekData, List<string> invalidCommits)
        {
            List<string> currentCommit = new List<string>();
            bool isCommitValid = true;

            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];

                if (line.StartsWith("commit "))
                {
                    if (currentCommit.Count > 0)
                    {
                        // Process the previous commit
                        ProcessCommit(currentCommit, startDate, endDate, weekData, invalidCommits);
                        currentCommit.Clear();
                    }
                    // Start a new commit
                    currentCommit.Add(line);
                }
                else
                {
                    currentCommit.Add(line);
                }

                // Process the last commit after the loop
                if (i == lines.Length - 1 && currentCommit.Count > 0)
                {
                    ProcessCommit(currentCommit, startDate, endDate, weekData, invalidCommits);
                    currentCommit.Clear();
                }
            }
        }

        private void ProcessCommit(List<string> commitLines, DateTime startDate, DateTime endDate, WeekData weekData, List<string> invalidCommits)
        {
            bool isMergeCommit = false;
            DateTime commitTime;
            string commitMessage = "";
            string dateLine = "";
            bool dateFound = false;

            foreach (string line in commitLines)
            {
                if (line.StartsWith("Merge:"))
                {
                    isMergeCommit = true;
                    break;
                }
                else if (line.StartsWith("Date:"))
                {
                    dateLine = line;
                    dateFound = true;
                }
            }

            if (!dateFound)
            {
                invalidCommits.AddRange(commitLines);
                return;
            }

            if (!dateLine.StartsWith("Date:") || dateLine.Length <= "Date:".Length)
            {
                invalidCommits.AddRange(commitLines);
                return;
            }

            string dateString = dateLine.Substring("Date:".Length).Trim();

            if (!DateTime.TryParseExact(dateString, "ddd MMM d HH:mm:ss yyyy K", CultureInfo.InvariantCulture, DateTimeStyles.None, out commitTime))
            {
                invalidCommits.AddRange(commitLines);
                return;
            }

            if (commitTime < startDate || commitTime > endDate)
            {
                invalidCommits.AddRange(commitLines);
                return;
            }

            // Collect commit message lines
            bool isMessageSection = false;
            foreach (string line in commitLines)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    isMessageSection = true;
                    continue;
                }
                if (isMessageSection)
                {
                    if (line.StartsWith("    "))
                    {
                        commitMessage += line.Substring(4).Trim() + " ";
                    }
                    else
                    {
                        invalidCommits.AddRange(commitLines);
                        return;
                    }
                }
            }

            // Check for "Merge branch" in commit message
            if (commitMessage.Contains("Merge branch") || isMergeCommit)
            {
                invalidCommits.AddRange(commitLines);
                return;
            }

            // Create DayData for valid commits
            var dayData = new DayData
            {
                DayOfWeek = commitTime.DayOfWeek.ToString(),
                Session = commitTime.Hour < 12 ? "Sáng" : "Chiều",
                Attendance = "Có mặt",
                AssignedTasks = commitMessage.Trim(),
                AchievedResults = commitMessage.Trim(),
                Comments = "",
                Notes = ""
            };
            weekData.DayDataList.Add(dayData);
        }


        public void InitializeDataGridView(DataGridView dataGridViewCommits)
        {
            // Đặt chế độ tự động điều chỉnh cột để chiếm 100% không gian
            dataGridViewCommits.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Định dạng các cột
            foreach (DataGridViewColumn column in dataGridViewCommits.Columns)
            {
                column.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            }
            dataGridViewCommits.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
        }

    }
}
