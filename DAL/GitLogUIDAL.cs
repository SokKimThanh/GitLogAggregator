using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using DocumentFormat.OpenXml.Spreadsheet;
using ET;

namespace GitLogAggregator.DataAccess
{
    public class GitlogDAL
    {
        public List<string> LogMessages { get; private set; }
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
                throw new Exception("Tìm thấy danh sách tác giả commit thất bại.");

            }

            HashSet<string> authors = new HashSet<string>();
            try
            {
                //Chia nhỏ output thành các dòng
                foreach (var line in output.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries))
                {
                    //Kiểm tra dòng không trống
                    if (!string.IsNullOrWhiteSpace(line))
                    {
                        //Chia nhỏ dòng thành các phần
                        string[] parts = line.Split('\t');
                        //Kiểm tra số phần
                        if (parts.Length >= 2)
                        {
                            //Lấy tên tác giả
                            string authorWithEmail = parts[1];
                            //Xác định vị trí email
                            int emailStart = authorWithEmail.IndexOf('<');
                            //Trích xuất tên tác giả:
                            string authorName = emailStart != -1
                                ? authorWithEmail.Substring(0, emailStart).Trim()
                                : authorWithEmail.Trim();
                            //Thêm tên tác giả vào danh sách
                            authors.Add(authorName);
                        }
                        else
                        //Xử lý lỗi định dạng Nếu số phần ít hơn 2
                        {
                            // Ghi nhật ký lỗi và tiếp tục vòng lặp
                            Console.WriteLine($"Unexpected line format: {line}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error processing authors: {ex.Message}");
            }

            return new List<string>(authors);
        }

        /// <summary>
        /// Truy cập file bằng streamwriter
        /// </summary>
        /// <param name="aggregateInfo"></param>
        public void SaveAggregateInfo(AggregateInfo aggregateInfo)
        {
            string configFile = Path.Combine(aggregateInfo.ProjectDirectory, "internship_week", "config.txt");
            using (StreamWriter writer = new StreamWriter(configFile))
            {
                writer.WriteLine($"Author: {aggregateInfo.Author}");
                writer.WriteLine($"StartDate: {aggregateInfo.StartDate:yyyy-MM-dd}");
                writer.WriteLine($"EndDate: {aggregateInfo.EndDate:yyyy-MM-dd}"); // Lưu ngày kết thúc thực tập
                writer.WriteLine($"Weeks: {aggregateInfo.Weeks}"); // Lưu số tuần thực tập
                writer.WriteLine($"FirstCommitDate: {aggregateInfo.FirstCommitDate:yyyy-MM-dd}");
                writer.WriteLine($"ProjectDirectory: {aggregateInfo.ProjectDirectory}");
                writer.WriteLine("Folders:");
                foreach (var folder in aggregateInfo.Folders)
                {
                    writer.WriteLine(folder);
                }
            }
        }


        /// <summary>
        /// Truy cập file config bằng streamreader
        /// </summary>
        /// <param name="configFile"></param>
        /// <returns></returns>
        public AggregateInfo LoadAggregateInfo(string configFile)
        {
            AggregateInfo aggregateInfo = new AggregateInfo();
            using (StreamReader reader = new StreamReader(configFile))
            {
                aggregateInfo.Author = reader.ReadLine().Split(':')[1].Trim();
                aggregateInfo.StartDate = DateTime.Parse(reader.ReadLine().Split(':')[1].Trim());
                aggregateInfo.EndDate = DateTime.Parse(reader.ReadLine().Split(':')[1].Trim()); // Đọc ngày kết thúc thực tập
                aggregateInfo.Weeks = int.Parse(reader.ReadLine().Split(':')[1].Trim()); // Đọc số tuần thực tập
                aggregateInfo.FirstCommitDate = DateTime.Parse(reader.ReadLine().Split(':')[1].Trim());
                aggregateInfo.ProjectDirectory = reader.ReadLine().Split(':')[1].Trim();
                aggregateInfo.Folders = new List<string>();
                reader.ReadLine(); // Bỏ qua dòng "Folders:"
                while (!reader.EndOfStream)
                {
                    aggregateInfo.Folders.Add(reader.ReadLine().Trim());
                }
            }
            return aggregateInfo;
        }



        /// <summary>
        /// Lệnh Git tìm ngày commit đầu tiên
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public DateTime GetProjectStartDate(string projectDirectory)
        {
            // Sử dụng lệnh Git chính xác
            string command = "log --reverse --pretty=format:\"%ad\" --date=short";
            string output = RunGitCommand(command, projectDirectory);

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



        public List<string> AggregateCommits(string projectDirectory, string author, DateTime internshipStartDate, string internshipWeekFolder)
        {
            DateTime projectStartDate = GetProjectStartDate(projectDirectory);
            int startingWeek = CalculateWeekNumber(internshipStartDate, projectStartDate);

            List<string> folders = new List<string>();

            for (int weekOffset = 0; weekOffset < 8; weekOffset++)
            {
                DateTime currentWeekStart = internshipStartDate.AddDays(weekOffset * 7);
                int currentWeek = startingWeek + weekOffset;
                string weekFolder = Path.Combine(internshipWeekFolder, "Week_" + currentWeek);
                string combinedFile = Path.Combine(weekFolder, "combined_commits.txt");

                Directory.CreateDirectory(weekFolder);

                if (File.Exists(combinedFile))
                {
                    File.Delete(combinedFile);
                }

                string[] days = { "monday", "tuesday", "wednesday", "thursday", "friday", "saturday", "sunday" };
                foreach (string day in days)
                {
                    string dailyFile = Path.Combine(weekFolder, $"{day}_commits.txt");

                    string since = $"{currentWeekStart:yyyy-MM-dd} 00:00";
                    string until = $"{currentWeekStart:yyyy-MM-dd} 23:59";
                    string gitLogCommand = $"log --author=\"{author}\" --since=\"{since}\" --until=\"{until}\"";

                    // Chạy lệnh Git và lưu kết quả vào biến
                    string logOutput = RunGitCommand(gitLogCommand, projectDirectory);

                    if (!string.IsNullOrEmpty(logOutput))
                    {
                        // Nếu có commit, ghi kết quả vào file
                        File.WriteAllText(dailyFile, logOutput);
                        using (StreamWriter writer = new StreamWriter(combinedFile, true))
                        {
                            writer.Write(logOutput);
                            writer.WriteLine();
                        }
                    }
                    else
                    {
                        // Xóa dailyFile nếu không có commit
                        if (File.Exists(dailyFile))
                        {
                            File.Delete(dailyFile);
                        }
                    }

                    currentWeekStart = currentWeekStart.AddDays(1);
                }

                folders.Add(weekFolder);
                LogMessages.Add($"Week {currentWeek} commits đã tổng hợp vào: {combinedFile}\n");
            }
            return folders;
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

    }
}