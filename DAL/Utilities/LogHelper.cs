using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace GitLogAggregator.Utilities
{
    public static class LogHelper
    {
        public static void WriteLog(string message)
        {
            string logPath = "log.txt";
            File.AppendAllText(logPath, $"{DateTime.Now}: {message}\n");
        }
    }
}
