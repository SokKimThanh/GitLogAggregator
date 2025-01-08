using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using ClosedXML.Excel;
using ET;

namespace DAL
{
    public class GitCommitDAL
    {
        public List<string> LogMessages { get; private set; }

        public GitCommitDAL()
        {
            LogMessages = new List<string>();
        }

        public List<WeekData> GetCommits(string projectDirectory, string author, DateTime startDate, int weeks)
        {
            List<WeekData> weekDataList = new List<WeekData>();
            LogMessages.Clear();

            for (int week = 0; week < weeks; week++)
            {
                DateTime weekStart = startDate.AddDays(week * 7);
                DateTime weekEnd = weekStart.AddDays(6);

                List<DayData> dayDataList = new List<DayData>();
                string[] daysOfWeek = { "Thứ hai", "Thứ ba", "Thứ tư", "thứ năm", "Thứ sáu", "Thứ bảy", "Chủ Nhật" };

                for (int day = 0; day < 7; day++)
                {
                    DateTime currentDate = weekStart.AddDays(day);
                    // Chỉ lấy nội dung tin nhắn commit
                    string gitLogCommand = $"log --author=\"{author}\" --since=\"{currentDate:yyyy-MM-dd} 00:00\" --until=\"{currentDate:yyyy-MM-dd} 23:59\" --pretty=format:\"%s\"";
                    string output = RunGitCommand(gitLogCommand, projectDirectory);

                    DayData dayData = new DayData
                    {
                        DayOfWeek = daysOfWeek[day],
                        Session = "", // Có thể cập nhật sau
                        Attendance = "", // Có thể cập nhật sau
                        AssignedTasks = string.Join("\n", output.Split('\n')),
                        AchievedResults = "", // Có thể cập nhật sau
                        Comments = "", // Có thể cập nhật sau
                        Notes = "" // Có thể cập nhật sau
                    };

                    dayDataList.Add(dayData);
                }

                weekDataList.Add(new WeekData
                {
                    WeekNumber = week + 1,
                    StartDate = weekStart,
                    EndDate = weekEnd,
                    DayDataList = dayDataList
                });

                LogMessages.Add($"Đã lấy dữ liệu commit cho tuần {week + 1}");
            }

            return weekDataList;
        }

        private string RunGitCommand(string command, string projectDirectory)
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


        public void ExportCommitsToExcel(string filePath, List<WeekData> weekDataList)
        {
            using (var workbook = new XLWorkbook())
            {
                foreach (var weekData in weekDataList)
                {
                    var worksheet = workbook.Worksheets.Add($"Tuần {weekData.WeekNumber}");

                    // Thiết lập tiêu đề bảng
                    worksheet.Cell(1, 1).Value = "Tuần";
                    worksheet.Cell(1, 2).Value = "Thứ";
                    worksheet.Cell(1, 3).Value = "Buổi";
                    worksheet.Cell(1, 4).Value = "Điểm danh vắng";
                    worksheet.Cell(1, 5).Value = "Công việc được giao";
                    worksheet.Cell(1, 6).Value = "Nội dung – kết quả đạt được";
                    worksheet.Cell(1, 7).Value = "Nhận xét - đề nghị của người hướng dẫn tại doanh nghiệp";
                    worksheet.Cell(1, 8).Value = "Ghi chú";

                    int currentRow = 2;

                    foreach (var dayData in weekData.DayDataList)
                    {
                        worksheet.Cell(currentRow, 1).Value = $"Tuần {weekData.WeekNumber} Từ {weekData.StartDate:dd/MM/yyyy} Đến {weekData.EndDate:dd/MM/yyyy}";
                        worksheet.Cell(currentRow, 2).Value = dayData.DayOfWeek;
                        worksheet.Cell(currentRow, 3).Value = dayData.Session;
                        worksheet.Cell(currentRow, 4).Value = dayData.Attendance;
                        worksheet.Cell(currentRow, 5).Value = dayData.AssignedTasks;
                        worksheet.Cell(currentRow, 6).Value = dayData.AchievedResults;
                        worksheet.Cell(currentRow, 7).Value = dayData.Comments;
                        worksheet.Cell(currentRow, 8).Value = dayData.Notes;
                        currentRow++;
                    }

                    worksheet.Columns().AdjustToContents();
                }

                workbook.SaveAs(filePath);
            }

            LogMessages.Add("Xuất Excel thành công!");
        }
    }
}
