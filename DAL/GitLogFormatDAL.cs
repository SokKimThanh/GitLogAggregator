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
        /// Hãy đảm bảo rằng hàm CreateExcelFile in ra đủ số tuần từ danh sách weekDataList.
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="weekDataList"></param>
        /// <param name="internshipEndDate"></param>
        public void CreateExcelFile(string filePath, List<WeekData> weekDataList, DateTime internshipEndDate)
        {
            // Kiểm tra nếu file đang được mở
            if (IsFileLocked(new FileInfo(filePath)))
            {
                throw new IOException($"File {filePath} đang được mở bởi một quá trình khác. Vui lòng đóng file trước khi tiếp tục.");
            }

            using (var workbook = new XLWorkbook())
            {
                var allCommits = weekDataList.SelectMany(w => w.DayDataList)
                    .SelectMany(d => d.AssignedTasks.Split('\n'))
                    .ToList();

                int totalCommits = allCommits.Count;
                int totalDays = weekDataList.Count * 11; // 5 ngày làm việc cả ngày và 1 ngày làm việc buổi sáng
                int dailyCommits = totalCommits / totalDays;
                int remainingCommits = totalCommits % totalDays;

                int commitIndex = 0;

                foreach (var weekData in weekDataList)
                {
                    string weekStartDate = weekData.StartDate.ToString("dd.MM");
                    string weekEndDate = weekData.EndDate <= internshipEndDate ? weekData.EndDate.ToString("dd.MM") : internshipEndDate.ToString("dd.MM");
                    var worksheet = workbook.Worksheets.Add($"Tuần {weekData.WeekNumber} ({weekStartDate}-{weekEndDate})");

                    worksheet.Cell(1, 1).Value = "Tuần";
                    worksheet.Cell(1, 2).Value = "Thứ";
                    worksheet.Cell(1, 3).Value = "Buổi";
                    worksheet.Cell(1, 4).Value = "Điểm danh vắng";
                    worksheet.Cell(1, 5).Value = "Công việc được giao";
                    worksheet.Cell(1, 6).Value = "Nội dung – kết quả đạt được";
                    worksheet.Cell(1, 7).Value = "Nhận xét - đề nghị của người hướng dẫn tại doanh nghiệp";
                    worksheet.Cell(1, 8).Value = "Ghi chú";

                    int currentRow = 2;
                    DayOfWeek[] daysOfWeek = new DayOfWeek[]
                    {
                DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday,
                DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday
                    };

                    string[] sessions = new string[] { "Sáng", "Chiều" };

                    for (int day = 0; day < 6; day++)
                    {
                        foreach (var session in sessions)
                        {
                            if (day == 5 && session == "Chiều") break; // Thứ Bảy chỉ làm việc buổi sáng

                            if (day * 2 + (session == "Chiều" ? 1 : 0) >= weekData.DayDataList.Count)
                            {
                                weekData.DayDataList.Add(new DayData
                                {
                                    Day = daysOfWeek[day].ToString(),
                                    Session = session,
                                    Attendance = "Có mặt",
                                    AssignedTasks = "N/A",
                                    AchievedResults = "N/A",
                                    Comments = "N/A",
                                    Notes = "N/A"
                                }); // Thêm ngày trống với dữ liệu mặc định
                            }

                            var dayData = weekData.DayDataList[day * 2 + (session == "Chiều" ? 1 : 0)];
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

                            var dayCommits = allCommits.Skip(commitIndex).Take(dayCommitsCount).ToList();
                            commitIndex += dayCommitsCount;

                            worksheet.Cell(currentRow, 1).Value = $"Tuần {weekData.WeekNumber}";
                            worksheet.Cell(currentRow, 2).Value = dayData.Day;
                            worksheet.Cell(currentRow, 3).Value = dayData.Session;
                            worksheet.Cell(currentRow, 4).Value = dayData.Attendance;
                            worksheet.Cell(currentRow, 5).Value = string.Join("\n", dayCommits); // Công việc được giao
                            worksheet.Cell(currentRow, 6).Value = dayData.AchievedResults; // Nội dung – kết quả đạt được
                            worksheet.Cell(currentRow, 7).Value = dayData.Comments; // Nhận xét - đề nghị của người hướng dẫn tại doanh nghiệp
                            worksheet.Cell(currentRow, 8).Value = dayData.Notes; // Ghi chú

                            currentRow++;
                        }
                    }

                    worksheet.Columns().AdjustToContents();
                }

                workbook.SaveAs(filePath);
            }
        }

        private bool IsFileLocked(FileInfo file)
        {
            try
            {
                using (FileStream stream = file.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None))
                {
                    stream.Close();
                }
            }
            catch (IOException)
            {
                return true;
            }

            return false;
        }



        /// <summary>
        /// Hãy chắc chắn rằng hàm ConvertDayDataListToWeekDataList đã chia đủ 8 tuần:
        /// </summary>
        /// <param name="dayDataList"></param>
        /// <param name="internshipStartDate"></param>
        /// <param name="internshipEndDate"></param>
        /// <returns></returns>
        public List<WeekData> ConvertDayDataListToWeekDataList(List<DayData> dayDataList, DateTime internshipStartDate, DateTime internshipEndDate)
        {
            List<WeekData> weekDataList = new List<WeekData>();

            int totalDays = (internshipEndDate - internshipStartDate).Days + 1; // Tổng số ngày trong khoảng thời gian thực tập
            int totalWeeks = (totalDays + 6) / 7; // Tổng số tuần, làm tròn lên

            for (int i = 0; i < totalWeeks; i++)
            {
                DateTime weekStartDate = internshipStartDate.AddDays(i * 7);
                DateTime weekEndDate = weekStartDate.AddDays(6) <= internshipEndDate ? weekStartDate.AddDays(6) : internshipEndDate;

                WeekData weekData = new WeekData
                {
                    WeekNumber = i + 1,
                    StartDate = weekStartDate,
                    EndDate = weekEndDate,
                    DayDataList = new List<DayData>()
                };

                for (int j = 0; j < 7 && (i * 7 + j) < dayDataList.Count; j++)
                {
                    weekData.DayDataList.Add(dayDataList[i * 7 + j]);
                }

                weekDataList.Add(weekData);
            }

            return weekDataList;
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
                    row["Thứ"] = dayData.Day;
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

        public List<DayData> ConvertDataTableToDayDataList(DataTable dataTable)
        {
            List<DayData> dayDataList = new List<DayData>();

            foreach (DataRow row in dataTable.Rows)
            {
                dayDataList.Add(new DayData
                {
                    Day = row["Thứ"].ToString(), // Sử dụng tên cột "Thứ"
                    Session = row["Buổi"].ToString(), // Sử dụng tên cột "Buổi"
                    Attendance = row["Điểm danh vắng"].ToString(), // Sử dụng tên cột "Điểm danh vắng"
                    AssignedTasks = row["Nội dung commit"].ToString(), // Sử dụng tên cột "Nội dung commit"
                    AchievedResults = row["Nội dung commit"].ToString(), // Sử dụng tên cột "Nội dung commit" (giả định rằng kết quả đạt được trùng với nội dung commit)
                    Comments = row["Nhận xét"].ToString(), // Sử dụng tên cột "Nhận xét"
                    Notes = row["Ghi chú"].ToString() // Sử dụng tên cột "Ghi chú"
                });
            }

            return dayDataList;
        }

    }
}
