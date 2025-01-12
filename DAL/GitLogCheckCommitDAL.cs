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
            string commitMessage = "";

            for (int i = 0; i < lines.Length; i++)
            {
                var line = lines[i];

                // Skip merge commits
                if (line.StartsWith("commit") && line.Contains("merge"))
                {
                    invalidCommits.Add(line);
                    // Skip to the next commit
                    while (i < lines.Length && !lines[i].StartsWith("commit"))
                        i++;
                    continue;
                }

                // Skip author lines
                if (line.StartsWith("Author:"))
                {
                    invalidCommits.Add(line);
                    continue;
                }

                // Process date lines to get commit time
                if (line.StartsWith("Date:"))
                {
                    var dateStr = line.Substring("Date:".Length).Trim();
                    if (!DateTime.TryParseExact(dateStr, "ddd MMM d HH:mm:ss yyyy K", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime commitTime))
                    {
                        invalidCommits.Add(line);
                        continue;
                    }

                    if (commitTime < startDate || commitTime > endDate)
                    {
                        invalidCommits.Add(line);
                        continue;
                    }

                    // Skip the empty line after "Date:"
                    if (i + 1 < lines.Length && string.IsNullOrWhiteSpace(lines[i + 1]))
                    {
                        i++;
                        // Collect all lines starting with four spaces as part of the commit message
                        while (i + 1 < lines.Length && lines[i + 1].StartsWith("    "))
                        {
                            commitMessage += lines[i + 1].Trim() + " ";
                            i++;
                        }
                        // Create DayData with the full commit message
                        var dayData = new DayData
                        {
                            DayOfWeek = commitTime.DayOfWeek.ToString(),
                            Session = commitTime.Hour < 12 ? "Morning" : "Afternoon",
                            Attendance = "Present",
                            AssignedTasks = commitMessage.Trim(),
                            AchievedResults = commitMessage.Trim(),
                            Comments = "",
                            Notes = ""
                        };
                        weekData.DayDataList.Add(dayData);
                        // Reset commitMessage for next commit
                        commitMessage = "";
                    }
                    else
                    {
                        // If there's no empty line after "Date:", mark as invalid
                        invalidCommits.Add(line);
                    }
                }
            }
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
