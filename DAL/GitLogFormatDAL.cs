using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using ClosedXML.Excel;
using ET;

namespace DAL
{
    public class GitLogFormatDAL
    {
        public List<string> LogMessages { get; private set; }

        public GitLogFormatDAL()
        {
            LogMessages = new List<string>();
        }

        public void CreateExcelFile(string filePath, List<WeekData> weekDataList, DateTime internshipEndDate)
        {
            using (var workbook = new XLWorkbook())
            {
                foreach (var weekData in weekDataList)
                {
                    string weekStartDate = weekData.StartDate.ToString("ddMM");
                    string weekEndDate = weekData.EndDate <= internshipEndDate ? weekData.EndDate.ToString("ddMM") : internshipEndDate.ToString("ddMM");
                    var worksheet = workbook.Worksheets.Add($"Tuan {weekData.WeekNumber} ({weekStartDate}-{weekEndDate})");

                    worksheet.Cell(1, 1).Value = "Tuần";
                    worksheet.Cell(1, 2).Value = "Thứ";
                    worksheet.Cell(1, 3).Value = "Buổi";
                    worksheet.Cell(1, 4).Value = "Điểm danh vắng";
                    worksheet.Cell(1, 5).Value = "Công việc được giao";
                    worksheet.Cell(1, 6).Value = "Nội dung – kết quả đạt được";
                    worksheet.Cell(1, 7).Value = "Nhận xét - đề nghị của người hướng dẫn tại doanh nghiệp";
                    worksheet.Cell(1, 8).Value = "Ghi chú";

                    int currentRow = 2;
                    var allCommits = weekDataList.SelectMany(w => w.DayDataList)
                        .SelectMany(d => d.AssignedTasks.Split('\n'))
                        .ToList();

                    int totalCommits = allCommits.Count;
                    int dailyCommits = totalCommits / (weekDataList.Count * 7);
                    int remainingCommits = totalCommits % (weekDataList.Count * 7);

                    for (int day = 0; day < 7; day++)
                    {
                        if (day >= weekData.DayDataList.Count)
                        {
                            weekData.DayDataList.Add(new DayData()); // Thêm ngày trống nếu thiếu
                        }

                        var dayData = weekData.DayDataList[day];
                        DateTime currentDate = weekData.StartDate.AddDays(day);

                        if (currentDate > internshipEndDate)
                        {
                            break; // Ngừng ghi khi vượt quá ngày kết thúc thực tập
                        }

                        int dayCommitsCount = dailyCommits;
                        if (remainingCommits > 0)
                        {
                            dayCommitsCount++;
                            remainingCommits--;
                        }

                        var dayCommits = allCommits.Skip((weekData.WeekNumber - 1) * 7 * dailyCommits + day * dailyCommits).Take(dayCommitsCount).ToList();

                        worksheet.Cell(currentRow, 1).Value = $"Tuần {weekData.WeekNumber}";
                        worksheet.Cell(currentRow, 2).Value = dayData.DayOfWeek;
                        worksheet.Cell(currentRow, 3).Value = dayData.Session;
                        worksheet.Cell(currentRow, 4).Value = dayData.Attendance;
                        worksheet.Cell(currentRow, 5).Value = string.Join("\n", dayCommits); // Công việc được giao
                        worksheet.Cell(currentRow, 6).Value = dayData.AchievedResults; // Nội dung – kết quả đạt được
                        worksheet.Cell(currentRow, 7).Value = dayData.Comments; // Nhận xét - đề nghị của người hướng dẫn tại doanh nghiệp
                        worksheet.Cell(currentRow, 8).Value = dayData.Notes; // Ghi chú

                        currentRow++;
                    }

                    worksheet.Columns().AdjustToContents();
                }

                workbook.SaveAs(filePath);
            }
        }




        public List<WeekData> ConvertDayDataListToWeekDataList(List<DayData> dayDataList, DateTime internshipStartDate, DateTime internshipEndDate)
        {
            List<WeekData> weekDataList = new List<WeekData>();

            for (int i = 0; i < dayDataList.Count; i += 7)
            {
                DateTime weekStartDate = internshipStartDate.AddDays(i);
                DateTime weekEndDate = weekStartDate.AddDays(6) <= internshipEndDate ? weekStartDate.AddDays(6) : internshipEndDate;

                WeekData weekData = new WeekData
                {
                    WeekNumber = (i / 7) + 1,
                    StartDate = weekStartDate,
                    EndDate = weekEndDate,
                    DayDataList = new List<DayData>()
                };

                for (int j = 0; j < 7 && (i + j) < dayDataList.Count; j++)
                {
                    weekData.DayDataList.Add(dayDataList[i + j]);
                }

                weekDataList.Add(weekData);
            }

            return weekDataList;
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
        public DataTable ConvertToDataTable(List<WeekData> weekDataList)
        {
            var dataTable = new DataTable(); dataTable.Columns.Add("Tuần", typeof(int));
            dataTable.Columns.Add("Thứ", typeof(string));
            dataTable.Columns.Add("Buổi", typeof(string));
            dataTable.Columns.Add("Điểm danh vắng", typeof(string));
            dataTable.Columns.Add("Công việc được giao", typeof(string));
            dataTable.Columns.Add("Nội dung – kết quả đạt được", typeof(string));
            dataTable.Columns.Add("Nhận xét - đề nghị của người hướng dẫn tại doanh nghiệp", typeof(string));
            dataTable.Columns.Add("Ghi chú", typeof(string));
            foreach (var weekData in weekDataList)
            {
                foreach (var dayData in weekData.DayDataList)
                {
                    var row = dataTable.NewRow();
                    row["Tuần"] = weekData.WeekNumber;
                    row["Thứ"] = dayData.DayOfWeek;
                    row["Buổi"] = dayData.Session;
                    row["Điểm danh vắng"] = dayData.Attendance;
                    row["Công việc được giao"] = dayData.AssignedTasks;
                    row["Nội dung – kết quả đạt được"] = dayData.AchievedResults;
                    row["Nhận xét - đề nghị của người hướng dẫn tại doanh nghiệp"] = dayData.Comments;
                    row["Ghi chú"] = dayData.Notes; dataTable.Rows.Add(row);
                }
            }
            return dataTable;
        }
     

    }
}
