using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using DAL;
using DocumentFormat.OpenXml.Spreadsheet;
using ET;

namespace GitLogAggregator.DataAccess
{
    public class GitlogDAL
    {
        public List<string> LogMessages { get; private set; }
        ConfigFileDAL gitconfig_dal = new ConfigFileDAL();
        public GitlogDAL()
        {
            LogMessages = new List<string>();
        }
        /// <summary>
        /// Tính số tuần từ ngày bắt đầu thực tập đến ngày bắt đầu dự án
        /// </summary>
        /// <param name="internshipStartDate"></param>
        /// <param name="projectStartDate"></param>
        /// <returns></returns>
        public int CalculateWeekNumber(DateTime internshipStartDate, DateTime projectStartDate)
        {
            // Đảm bảo internshipStartDate không sớm hơn projectStartDate
            if (internshipStartDate < projectStartDate)
            {
                internshipStartDate = projectStartDate; // Chỉ tính từ ngày bắt đầu dự án
            }

            // Tính số ngày giữa internshipStartDate và projectStartDate
            TimeSpan difference = internshipStartDate - projectStartDate;

            // Tính số tuần (làm tròn lên nếu số ngày không chia hết cho 7)
            int weekNumber = (int)Math.Ceiling(difference.TotalDays / 7.0) + 1; // Thêm 1 để bắt đầu từ tuần 1

            return weekNumber;
        }

        public string GetFirstCommitAuthor(string folderPath)
        {
            try
            {
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = "git",
                    Arguments = "log --reverse --format='%ae' -1",
                    WorkingDirectory = folderPath,
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };
                Process process = Process.Start(startInfo);
                process.WaitForExit();
                string author = process.StandardOutput.ReadToEnd().Trim();
                return author;
            }
            catch
            {
                MessageBox.Show("Không thể lấy tác giả commit.");
                throw;
            }
        }

        public List<string> GetGitAuthors(string projectDirectory)
        {
            // Kiểm tra thư mục dự án
            if (string.IsNullOrEmpty(projectDirectory) || !Directory.Exists(projectDirectory))
            {
                throw new Exception("Thư mục dự án không tồn tại hoặc không hợp lệ.");
            }

            // Chạy lệnh Git để lấy danh sách tác giả
            string gitCommand = "shortlog -sne";
            string output = RunGitCommand(gitCommand, projectDirectory);

            // Kiểm tra nếu output rỗng
            if (string.IsNullOrEmpty(output))
            {
                throw new Exception("Lỗi: Không tìm thấy danh sách tác giả commit.");
            }

            HashSet<string> authors = new HashSet<string>();
            try
            {
                // Chia nhỏ output thành các dòng
                foreach (var line in output.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    // Kiểm tra dòng không trống
                    if (!string.IsNullOrWhiteSpace(line))
                    {
                        // Tìm vị trí của dấu ngoặc nhọn '<'
                        int emailStart = line.IndexOf('<');
                        if (emailStart != -1)
                        {
                            // Trích xuất tên tác giả, bỏ số commit và email
                            string authorName = line.Substring(line.IndexOf('\t') + 1, emailStart - line.IndexOf('\t') - 1).Trim();
                            // Thêm tên tác giả vào danh sách
                            authors.Add(authorName);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error processing authors: {ex.Message}");
            }

            return authors.ToList(); // Chuyển đổi HashSet thành List để trả về
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
        /// Lệnh Git tìm ngày commit đầu tiên
        /// </summary>
        /// <param name="folderPath"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public DateTime GetFirstCommitDate(string folderPath)
        {
            // Sử dụng lệnh Git chính xác
            string command = "log --reverse --pretty=format:\"%ad\" --date=short";
            string output = RunGitCommand(command, folderPath);

            // Kiểm tra kết quả lệnh Git
            if (string.IsNullOrEmpty(output))
            {
                throw new Exception("Không thể tìm thấy thông tin ngày bắt đầu dự án.");
            }

            // Lấy ra chuỗi đầu tiên là ngày commit đầu tiên
            string[] dates = output.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            if (dates.Length == 0)
            {
                throw new Exception("Không tìm thấy commit nào trong dự án.");
            }

            // Đọc nội dung và kiểm tra định dạng ngày
            string dateStr = dates[0].Trim();
            if (DateTime.TryParseExact(dateStr, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime projectStartDate))
            {
                return projectStartDate;
            }
            else
            {
                throw new Exception($"Ngày không hợp lệ: {dateStr}");
            }
        }
        /// <summary>
        /// Tổng hợp commit và xuất ra desktop từng thư mục dự án
        /// </summary>
        /// <param name="projectDirectory"></param>
        /// <param name="author"></param>
        /// <param name="internshipStartDate"></param>
        /// <param name="totalWeeks"></param>
        public void AggregateCommits(string projectDirectory, string author, DateTime internshipStartDate, int totalWeeks, string internshipWeekFolder)
        {
            try
            {
                // Tạo thư mục tổng hợp nếu chưa tồn tại
                if (!Directory.Exists(internshipWeekFolder))
                {
                    Directory.CreateDirectory(internshipWeekFolder);
                }

                for (int weekOffset = 0; weekOffset < totalWeeks; weekOffset++)
                {
                    // Xác định thư mục cho tuần hiện tại
                    DateTime weekStartDate = internshipStartDate.AddDays(weekOffset * 7);
                    DateTime weekEndDate = weekStartDate.AddDays(6);
                    int currentWeek = weekOffset + 1;
                    string weekFolder = Path.Combine(internshipWeekFolder, "Week_" + currentWeek);
                    if (!Directory.Exists(weekFolder))
                    {
                        Directory.CreateDirectory(weekFolder);
                    }

                    string combinedFile = Path.Combine(weekFolder, "combined_commits.txt");
                    bool hasCommits = false;

                    // Duyệt qua từng ngày trong tuần
                    for (int dayOffset = 0; dayOffset < 7; dayOffset++)
                    {
                        DateTime currentDate = weekStartDate.AddDays(dayOffset);
                        string dayOfWeekName = GetDayOfWeekName(currentDate.DayOfWeek); // Hàm chuyển đổi từ DayOfWeek sang tên tiếng Việt
                        string dailyFile = Path.Combine(weekFolder, $"{dayOfWeekName}_commits.txt");

                        string since = $"{currentDate:yyyy-MM-dd} 00:00";
                        string until = $"{currentDate:yyyy-MM-dd} 23:59";
                        string gitLogCommand = $"log --author=\"{author}\" --since=\"{since}\" --until=\"{until}\"";

                        string logOutput = RunGitCommand(gitLogCommand, projectDirectory); // Hàm chạy lệnh git

                        if (!string.IsNullOrEmpty(logOutput))
                        {
                            // Ghi log vào tệp daily và tệp tổng hợp
                            AppendToFile(dailyFile, logOutput); // Hàm ghi nội dung vào tệp
                            AppendToFile(combinedFile, logOutput);
                            hasCommits = true;
                        }

                        if (!File.Exists(dailyFile) && File.Exists(combinedFile))
                        {
                            File.Delete(dailyFile); // Xóa tệp daily nếu không có commit
                        }
                    }

                    if (!hasCommits)
                    {
                        // Xóa thư mục tuần nếu không có commit
                        Directory.Delete(weekFolder);
                    }
                    else
                    {
                        LogMessages.Add($"Week {currentWeek} commits đã tổng hợp vào: {combinedFile}");
                    }
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                LogMessages.Add($"Lỗi: Không có quyền truy cập vào đường dẫn '{internshipWeekFolder}'. Chi tiết: {ex.Message}\n");
            }
            catch (Exception ex)
            {
                LogMessages.Add($"Lỗi: Đã xảy ra lỗi khi tổng hợp commits. Chi tiết: {ex.Message}\n");
            }
        }

        private void AppendToFile(string filePath, string content)
        {
            File.AppendAllText(filePath, content + Environment.NewLine);
        }

        /// <summary>
        /// Chuyển đổi từ DayOfWeek sang tên tiếng Việt
        /// </summary>
        private string GetDayOfWeekName(DayOfWeek dayOfWeek)
        {
            switch (dayOfWeek)
            {
                case DayOfWeek.Monday:
                    return "Thứ hai";
                case DayOfWeek.Tuesday:
                    return "Thứ ba";
                case DayOfWeek.Wednesday:
                    return "Thứ tư";
                case DayOfWeek.Thursday:
                    return "Thứ năm";
                case DayOfWeek.Friday:
                    return "Thứ sáu";
                case DayOfWeek.Saturday:
                    return "Thứ bảy";
                default: // Nếu cần thêm Chủ nhật
                    return "Chủ nhật";
            }
        }
        /// <summary>
        /// Hiển thị danh sách tác giả trên combobox
        /// </summary>
        public List<string> LoadAuthorsCombobox(string projectDirectory)
        {
            List<string> authors;
            try
            {
                authors = GetGitAuthors(projectDirectory);
            }
            catch (Exception ex)
            {
                throw new Exception("Error loading authors: " + ex.Message);
            }
            return authors;
        }
        public List<DayData> GetCommits(string projectDirectory, string author, DateTime internshipStartDate, DateTime internshipEndDate)
        {
            List<DayData> dayDataList = new List<DayData>();

            for (DateTime date = internshipStartDate; date <= internshipEndDate; date = date.AddDays(1))
            {
                string gitLogCommand = $"log --author=\"{author}\" --since=\"{date:yyyy-MM-dd} 00:00\" --until=\"{date:yyyy-MM-dd} 23:59\" --pretty=format:\"%s\"";
                string output = RunGitCommand(gitLogCommand, projectDirectory);

                if (!string.IsNullOrEmpty(output))
                {
                    var tasks = output.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries)
                                      .Select(task => task.Trim())
                                      .ToArray();

                    dayDataList.Add(new DayData
                    {
                        DayOfWeek = date.DayOfWeek.ToString(),
                        Session = "Sáng", // hoặc "Chiều", "Tối" tùy theo thông tin bạn có
                        Attendance = "Có mặt",
                        AssignedTasks = string.Join("\n", tasks),
                        AchievedResults = "N/A",
                        Comments = "N/A",
                        Notes = "N/A"
                    });
                }
            }

            return dayDataList;
        }


        public DateTime CalculateEndDate(DateTime startDate, int weeks)
        {
            return startDate.AddDays(weeks * 7 - 1); // -1 để bao gồm ngày bắt đầu
        }
        public DataTable ConvertDayDataListToDataTable(List<DayData> dayDataList)
        {
            DataTable dataTable = new DataTable();

            dataTable.Columns.Add("DayOfWeek", typeof(string));
            dataTable.Columns.Add("Session", typeof(string));
            dataTable.Columns.Add("Attendance", typeof(string));
            dataTable.Columns.Add("AssignedTasks", typeof(string));
            dataTable.Columns.Add("AchievedResults", typeof(string));
            dataTable.Columns.Add("Comments", typeof(string));
            dataTable.Columns.Add("Notes", typeof(string));

            foreach (var dayData in dayDataList)
            {
                dataTable.Rows.Add(dayData.DayOfWeek, dayData.Session, dayData.Attendance, dayData.AssignedTasks, dayData.AchievedResults, dayData.Comments, dayData.Notes);
            }

            return dataTable;
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
    }
}