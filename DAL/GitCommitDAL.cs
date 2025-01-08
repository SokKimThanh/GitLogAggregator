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
    public class GitCommitDAL
    {
        public List<string> LogMessages { get; private set; }

        public GitCommitDAL()
        {
            LogMessages = new List<string>();
        }

        public List<string> GetCommits(string projectDirectory, string author, DateTime startDate, int weeks)
        {
            List<string> commits = new List<string>();

            for (int week = 0; week < weeks; week++)
            {
                DateTime weekStart = startDate.AddDays(week * 7);
                DateTime weekEnd = weekStart.AddDays(6);

                string[] daysOfWeek = { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };

                for (int day = 0; day < 7; day++)
                {
                    DateTime currentDate = weekStart.AddDays(day);
                    string gitLogCommand = $"log --author=\"{author}\" --since=\"{currentDate:yyyy-MM-dd} 00:00\" --until=\"{currentDate:yyyy-MM-dd} 23:59\" --pretty=format:\"%s\""; // Chỉ lấy nội dung tin nhắn commit
                    string output = RunGitCommand(gitLogCommand, projectDirectory);

                    if (!string.IsNullOrEmpty(output))
                    {
                        commits.AddRange(output.Split('\n').Where(commit => !string.IsNullOrWhiteSpace(commit)).ToList());
                    }
                }
            }

            return commits;
        }


        public void CreateExcelFile(string filePath, List<WeekData> weekDataList)
        {
            using (var workbook = new XLWorkbook())
            {
                int totalCommits = weekDataList.Sum(w => w.DayDataList.Sum(d => d.AssignedTasks.Split('\n').Length));
                int weeklyCommits = totalCommits / 8;
                int internSpareCommits = totalCommits % 8;

                foreach (var weekData in weekDataList)
                {
                    string weekStartDate = weekData.StartDate.ToString("ddMM");
                    string weekEndDate = weekData.EndDate.ToString("ddMM");
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
                    int dailyCommits = weeklyCommits / 6;
                    int weeklySpareCommits = weeklyCommits % 6;

                    int commitIndex = (weekData.WeekNumber - 1) * weeklyCommits;

                    for (int day = 0; day < 7; day++)
                    {
                        if (day >= weekData.DayDataList.Count)
                        {
                            continue; // Bỏ qua ngày không có dữ liệu
                        }

                        var dayData = weekData.DayDataList[day];
                        DateTime currentDate = weekData.StartDate.AddDays(day);
                        worksheet.Cell(currentRow, 1).Value = $"Tuần {weekData.WeekNumber}";
                        worksheet.Cell(currentRow, 2).Value = dayData.DayOfWeek;
                        worksheet.Cell(currentRow, 3).Value = dayData.Session;
                        worksheet.Cell(currentRow, 4).Value = dayData.Attendance;

                        if (day < 6)
                        {
                            var dayCommits = weekDataList.SelectMany(w => w.DayDataList).Skip(commitIndex).Take(dailyCommits).SelectMany(d => d.AssignedTasks.Split('\n')).ToList();
                            worksheet.Cell(currentRow, 5).Value = string.Join("\n", dayCommits); // Công việc được giao
                            commitIndex += dayCommits.Count;
                        }
                        else
                        {
                            var dayCommits = weekDataList.SelectMany(w => w.DayDataList).Skip(commitIndex).Take(dailyCommits + weeklySpareCommits).SelectMany(d => d.AssignedTasks.Split('\n')).ToList();
                            worksheet.Cell(currentRow, 5).Value = string.Join("\n", dayCommits); // Công việc được giao
                            commitIndex += dayCommits.Count;
                        }

                        worksheet.Cell(currentRow, 6).Value = dayData.AchievedResults; // Nội dung – kết quả đạt được
                        worksheet.Cell(currentRow, 7).Value = dayData.Comments; // Nhận xét - đề nghị của người hướng dẫn tại doanh nghiệp
                        worksheet.Cell(currentRow, 8).Value = dayData.Notes; // Ghi chú

                        currentRow++;
                    }

                    if (weekData.WeekNumber == 8 && internSpareCommits > 0)
                    {
                        worksheet.Cell(currentRow, 1).Value = "Thêm commits dư của kỳ thực tập:";
                        var spareCommits = weekDataList.SelectMany(w => w.DayDataList).Skip(commitIndex).Take(internSpareCommits).SelectMany(d => d.AssignedTasks.Split('\n')).ToList();
                        worksheet.Cell(currentRow, 5).Value = string.Join("\n", spareCommits); // Công việc được giao
                    }

                    worksheet.Columns().AdjustToContents();
                }

                // Đảm bảo giải phóng tài nguyên trước khi lưu file
                workbook.SaveAs(filePath);
            }
        }


        public List<WeekData> ConvertToWeekDataList(DataTable dataTable)
        {
            var weekDataList = new List<WeekData>();

            foreach (DataRow row in dataTable.Rows)
            {
                int weekNumber = Convert.ToInt32(row["Tuần"]);
                DateTime startDate = DateTime.Now; // Giả sử bạn có phương thức để lấy giá trị này
                DateTime endDate = startDate.AddDays(6); // Giả sử bạn có phương thức để lấy giá trị này

                var weekData = weekDataList.Find(w => w.WeekNumber == weekNumber);
                if (weekData == null)
                {
                    weekData = new WeekData
                    {
                        WeekNumber = weekNumber,
                        StartDate = startDate,
                        EndDate = endDate,
                        DayDataList = new List<DayData>()
                    };
                    weekDataList.Add(weekData);
                }

                weekData.DayDataList.Add(new DayData
                {
                    DayOfWeek = row["Thứ"].ToString(),
                    Session = row["Buổi"].ToString(),
                    Attendance = row["Điểm danh vắng"].ToString(),
                    AssignedTasks = row["Công việc được giao"].ToString(),
                    AchievedResults = row["Nội dung – kết quả đạt được"].ToString(),
                    Comments = row["Nhận xét - đề nghị của người hướng dẫn tại doanh nghiệp"].ToString(),
                    Notes = row["Ghi chú"].ToString()
                });
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

        public List<WeekData> LoadCommitsFromFolders(List<string> folderPaths)
        {
            var weekDataList = new List<WeekData>();

            foreach (var folderPath in folderPaths)
            {
                var weekData = new WeekData();
                string weekNumberStr = Path.GetFileName(folderPath).Replace("Week_", "");
                if (int.TryParse(weekNumberStr, out int weekNumber))
                {
                    weekData.WeekNumber = weekNumber;
                }

                var dayDataList = new List<DayData>();
                string[] dayFiles = Directory.GetFiles(folderPath, "*_commits.txt");

                foreach (var dayFile in dayFiles)
                {
                    var dayData = new DayData();
                    string dayOfWeek = Path.GetFileName(dayFile).Replace("_commits.txt", "");
                    dayData.DayOfWeek = dayOfWeek;

                    string[] commitMessages = File.ReadAllLines(dayFile);
                    dayData.AssignedTasks = string.Join("\n", commitMessages);
                    dayDataList.Add(dayData);
                }

                weekData.DayDataList = dayDataList;
                weekDataList.Add(weekData);
            }

            return weekDataList;
        }

    }
}
