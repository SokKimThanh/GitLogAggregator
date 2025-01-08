using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Security.Cryptography;
using System.Text;
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

        public void SaveAggregateInfo(AggregateInfo aggregateInfo)
        {
            string configFolderPath = Path.Combine(aggregateInfo.ProjectDirectory, "internship_week");
            if (!Directory.Exists(configFolderPath))
            {
                Directory.CreateDirectory(configFolderPath);
            }

            string configFile = Path.Combine(configFolderPath, "config.txt");
            using (StreamWriter writer = new StreamWriter(configFile))
            {
                writer.WriteLine($"ProjectDirectory={aggregateInfo.ProjectDirectory}");
                writer.WriteLine($"Author={aggregateInfo.Author}");
                writer.WriteLine($"StartDate={aggregateInfo.StartDate:yyyy-MM-dd}");
                writer.WriteLine("Folders=");
                foreach (var folder in aggregateInfo.Folders)
                {
                    writer.WriteLine(folder);
                }
            }
        }

        public AggregateInfo LoadAggregateInfo(string aggregateInfoPath)
        {
            if (!File.Exists(aggregateInfoPath))
            {
                throw new FileNotFoundException("Config file not found.");
            }

            string author = "";
            string projectDirectory = "";

            DateTime startDate = DateTime.Now;
            List<string> folders = new List<string>();

            string[] lines = File.ReadAllLines(aggregateInfoPath);
            foreach (var line in lines)
            {
                if (line.StartsWith("Author="))
                    author = line.Substring(7);
                else if (line.StartsWith("ProjectDirectory="))
                    projectDirectory = line.Substring(17);// bỏ 17 kí tự, trên dòng còn nhiêu kí tự lấy hết
                else if (line.StartsWith("StartDate="))
                    DateTime.TryParseExact(line.Substring(10), "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out startDate);
                else if (!string.IsNullOrWhiteSpace(line) && line != "Folders=")
                    folders.Add(line);
            }

            return new AggregateInfo
            {
                ProjectDirectory = projectDirectory,
                Author = author,
                StartDate = startDate,
                Folders = folders
            };
        }

        /// <summary>
        /// Lệnh Git tìm ngày commit đầu tiên
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public DateTime GetProjectStartDate(string projectDirectory)
        {
            string outputFilePath = Path.Combine(projectDirectory, "project_start_date.txt");
            string batchFilePath = Path.Combine(projectDirectory, "get_project_start_date.bat");

            // Sử dụng lệnh Git chính xác
            string command = @"git log --reverse --pretty=format:""%%ad"" --date=short -n 1 > project_start_date.txt";

            RunGitCommand(command, projectDirectory);

            // Kiểm tra file đầu ra
            if (!File.Exists(outputFilePath))
            {
                throw new Exception("Không thể tìm thấy file ngày bắt đầu dự án.");
            }

            // Đọc nội dung và kiểm tra định dạng ngày
            string dateStr = File.ReadAllText(outputFilePath).Trim();
            //resultRichTextBox.Text += $"Ngày đọc được: {dateStr}\n";

            if (DateTime.TryParseExact(dateStr, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime projectStartDate))
            {
                File.Delete(outputFilePath); // Xóa file sau khi đọc
                File.Delete(batchFilePath); // Xóa file sau khi đọc
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
                Console.Write($"Week {currentWeek} commits đã tổng hợp vào: {combinedFile}\n");
            }
            return folders;
        }
    }
}