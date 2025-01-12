using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace GitLogAggregator.Utilities
{
    public static class FileHelper
    {
        // Đọc nội dung file
        public static string ReadFile(string path)
        {
            if (File.Exists(path))
            {
                return File.ReadAllText(path);
            }
            throw new FileNotFoundException($"Không tìm thấy file tại {path}");
        }

        // Ghi nội dung vào file
        public static void WriteFile(string path, string content)
        {
            File.WriteAllText(path, content);
        }

        // Đảm bảo thư mục tồn tại
        public static void EnsureDirectoryExists(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        // Ghi log vào file
        public static void WriteLog(string logMessage)
        {
            string logPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log.txt");
            File.AppendAllText(logPath, $"{DateTime.Now}: {logMessage}\n");
        }
    }
}

