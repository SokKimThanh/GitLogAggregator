using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;
using ET;
using GitLogAggregator.Entities;

namespace GitLogAggregator.DataAccess
{
    public class GitlogDAL
    {
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
        /// <summary>
        /// Parse Git Log Output
        /// </summary>
        /// <param name="logOutput"></param>
        /// <returns></returns>
        public List<CommitEntity> ParseCommitLog(string logOutput)
        {
            List<CommitEntity> commits = new List<CommitEntity>();
            string[] commitLines = logOutput.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string commitLine in commitLines)
            {
                string[] parts = commitLine.Split(new[] { "|" }, StringSplitOptions.None);
                if (parts.Length >= 4)
                {
                    string author = parts[0];
                    string dateStr = parts[1];
                    string message = parts[2];

                    DateTime date = DateTime.Parse(dateStr);

                    commits.Add(new CommitEntity
                    {

                        Author = author,
                        Date = date,
                        Message = message
                    });
                }
            }

            return commits;
        }
        /// <summary>
        /// Organize Commits by Week
        /// </summary>
        /// <param name="commits"></param>
        /// <param name="internshipStartDate"></param>
        /// <param name="projectStartDate"></param>
        /// <returns></returns>
        public Dictionary<int, List<CommitEntity>> GroupCommitsByWeek(List<CommitEntity> commits, DateTime internshipStartDate, DateTime projectStartDate)
        {
            var groupedCommits = new Dictionary<int, List<CommitEntity>>();

            foreach (var commit in commits)
            {
                int weekNumber = CalculateWeekNumber(commit.Date, projectStartDate);
                if (!groupedCommits.ContainsKey(weekNumber))
                {
                    groupedCommits[weekNumber] = new List<CommitEntity>();
                }
                groupedCommits[weekNumber].Add(commit);
            }

            return groupedCommits;
        }
        public void RunGitCommand(string command, string outputFile, string projectDirectory)
        {
            string batchFilePath = Path.Combine(projectDirectory, "runGitCommand.bat");

            // Tạo file batch
            CreateBatchFile(batchFilePath, $"git {command} > \"{outputFile}\"", projectDirectory);

            // Chạy lệnh từ file batch
            RunBatchFile(batchFilePath);

            // Kiểm tra sự tồn tại của file output
            if (!File.Exists(outputFile))
            {
                Console.WriteLine($"File {outputFile} not created.");
            }

            // Xóa file batch sau khi hoàn thành nhiệm vụ
            File.Delete(batchFilePath);
        }
        /// <summary>
        /// Tạo batch file động để chạy lệnh Git và xử lý kết quả.
        /// Sử dụng cmd hoặc PowerShell để chạy lệnh và đọc output.
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="command"></param>
        public void CreateBatchFile(string filePath, string command, string projectDirectory)
        {
            string batchContent = $@"
@echo off
cd /d ""{projectDirectory}""
echo Running Git command...
{command}
echo Git command completed.
";
            File.WriteAllText(filePath, batchContent);
        }

        public void RunBatchFile(string filePath)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments = $"/c \"{filePath}\"",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (Process process = new Process { StartInfo = startInfo })
            {
                if (!process.Start())
                {
                    throw new InvalidOperationException("Process failed to start.");
                }

                using (StreamReader reader = process.StandardOutput)
                {
                    try
                    {
                        string output = reader.ReadToEnd();
                        Console.WriteLine(output);// Hiển thị kết quả của lệnh Git hoặc thông báo lỗi
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"An error occurred: {ex.Message}");
                    }
                }

                process.WaitForExit();
            }
        }
        public List<string> GetGitAuthors(string projectDirectory)
        {
            // Kiểm tra thư mục dự án
            if (string.IsNullOrEmpty(projectDirectory) || !Directory.Exists(projectDirectory))
            {
                Console.WriteLine("Project directory is invalid or does not exist.");
                return new List<string>();
            }

            // Đường dẫn file output
            string outputFilePath = Path.Combine(projectDirectory, "authors_output.txt");

            // Chạy lệnh Git để tạo file mới
            string gitCommand = $"shortlog -sne";
            RunGitCommand(gitCommand, outputFilePath, projectDirectory);

            // Tạo file bằng lệnh Git nếu chưa tồn tại
            if (!File.Exists(outputFilePath))
            {
                Console.WriteLine("Failed to generate authors_output.txt.");
                return new List<string>();
            }

            HashSet<string> authors = new HashSet<string>();
            try
            {
                foreach (var line in File.ReadLines(outputFilePath, Encoding.UTF8))
                {
                    if (!string.IsNullOrWhiteSpace(line))
                    {
                        string[] parts = line.Split('\t');
                        if (parts.Length >= 2)
                        {
                            string authorWithEmail = parts[1];
                            int emailStart = authorWithEmail.IndexOf('<');
                            string authorName = emailStart != -1
                                ? authorWithEmail.Substring(0, emailStart).Trim()
                                : authorWithEmail.Trim();
                            authors.Add(authorName);
                        }
                        else
                        {
                            Console.WriteLine($"Unexpected line format: {line}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing authors: {ex.Message}");
                return new List<string>();
            }

            // Xóa file sau khi xử lý thành công
            File.Delete(outputFilePath);

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
            DateTime startDate = DateTime.Now;
            List<string> folders = new List<string>();

            string[] lines = File.ReadAllLines(aggregateInfoPath);
            foreach (var line in lines)
            {
                if (line.StartsWith("Author="))
                    author = line.Substring(7);
                else if (line.StartsWith("StartDate="))
                    DateTime.TryParseExact(line.Substring(10), "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out startDate);
                else if (!string.IsNullOrWhiteSpace(line) && line != "Folders=")
                    folders.Add(line);
            }

            return new AggregateInfo
            {
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
            CreateBatchFile(batchFilePath, command, projectDirectory);

            // Chạy batch file
            RunBatchFile(batchFilePath);

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
    }
}