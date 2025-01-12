using ClosedXML.Excel;
using ET;
using GitLogAggregator.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace DAL
{
    public class GitLogCheckCommitDAL
    {

        // Method to convert List<WeekData> to DataTable
        public DataTable ConvertToDataTable(List<WeekData> weekDataList)
        {
            var dataTable = new DataTable();
            dataTable.Columns.Add("Tuần", typeof(string));
            dataTable.Columns.Add("Thứ", typeof(string));
            dataTable.Columns.Add("Buổi", typeof(string));
            dataTable.Columns.Add("Điểm danh vắng", typeof(string));
            dataTable.Columns.Add("Công việc được giao", typeof(string));
            dataTable.Columns.Add("Nội dung – kết quả đạt được", typeof(string));
            dataTable.Columns.Add("Nhận xét - đề nghị của người hướng dẫn tại doanh nghiệp", typeof(string));
            dataTable.Columns.Add("Ghi chú", typeof(string));

            foreach (var weekData in weekDataList)
            {
                string weekRange = $"Tuần {weekData.WeekNumber} Từ {weekData.StartDate.ToString("dd/MM/yyyy")} đến {weekData.EndDate.ToString("dd/MM/yyyy")}";
                foreach (var dayData in weekData.DayDataList)
                {
                    foreach (var sessionData in dayData.SessionDataList)
                    {
                        dataTable.Rows.Add(
                            weekRange,
                            dayData.DayOfWeek,
                            sessionData.Session,
                            sessionData.Attendance,
                            sessionData.AssignedTasks,
                            sessionData.AchievedResults,
                            sessionData.Comments,
                            sessionData.Notes
                        );
                    }
                }
            }

            return dataTable;
        }

        // Modified ParseCommitLines method
        public void ParseCommitLines(string[] lines, DateTime startDate, DateTime endDate, WeekData weekData, List<string> invalidCommits)
        {
            List<string> currentCommit = new List<string>();
            bool isCommitValid = true;
            List<string[]> validCommitLines = new List<string[]>();

            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];

                if (line.StartsWith("commit "))
                {
                    if (currentCommit.Count > 0)
                    {
                        if (isCommitValid)
                        {
                            validCommitLines.Add(currentCommit.ToArray());
                        }
                        else
                        {
                            invalidCommits.AddRange(currentCommit);
                        }
                        currentCommit.Clear();
                    }
                    currentCommit.Add(line);
                    isCommitValid = true;
                }
                else
                {
                    currentCommit.Add(line);
                }

                if (line.StartsWith("Merge:"))
                {
                    isCommitValid = false;
                }
                else if (line.StartsWith("Date:"))
                {
                    string dateString = line.Substring("Date:".Length).Trim();
                    if (!DateTime.TryParseExact(dateString, "ddd MMM d HH:mm:ss yyyy K", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime commitTime))
                    {
                        isCommitValid = false;
                    }
                    else if (commitTime < startDate || commitTime > endDate)
                    {
                        isCommitValid = false;
                    }
                }
            }

            if (currentCommit.Count > 0)
            {
                if (isCommitValid)
                {
                    validCommitLines.Add(currentCommit.ToArray());
                }
                else
                {
                    invalidCommits.AddRange(currentCommit);
                }
            }

            AssignCommitsToSessions(validCommitLines, weekData);
        }

        // New AssignCommitsToSessions method
        private void AssignCommitsToSessions(List<string[]> validCommitLines, WeekData weekData)
        {
            var commits = new List<Tuple<DateTime, string>>();
            var commitDates = new List<DateTime>();

            foreach (var commitLines in validCommitLines)
            {
                string dateLine = commitLines.FirstOrDefault(l => l.StartsWith("Date:"));
                if (dateLine != null)
                {
                    string dateString = dateLine.Substring("Date:".Length).Trim();
                    if (DateTime.TryParseExact(dateString, "ddd MMM d HH:mm:ss yyyy K", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime commitTime))
                    {
                        string commitMessage = "";
                        bool isMessageSection = false;
                        foreach (var line in commitLines)
                        {
                            if (string.IsNullOrWhiteSpace(line))
                            {
                                isMessageSection = true;
                                continue;
                            }
                            if (isMessageSection)
                            {
                                if (line.StartsWith("    "))
                                {
                                    commitMessage += line.Substring(4).Trim() + " ";
                                }
                                else
                                {
                                    continue;
                                }
                            }
                        }
                        commits.Add(Tuple.Create(commitTime, commitMessage));
                        commitDates.Add(commitTime);
                    }
                }
            }

            var nonExcessCommits = commits.Where(c => !(c.Item1.DayOfWeek == DayOfWeek.Saturday && c.Item1.Hour >= 12) && !(c.Item1.DayOfWeek == DayOfWeek.Sunday)).ToList();
            var excessCommits = commits.Where(c => (c.Item1.DayOfWeek == DayOfWeek.Saturday && c.Item1.Hour >= 12) || c.Item1.DayOfWeek == DayOfWeek.Sunday).ToList();

            var sessions = new Dictionary<string, List<Tuple<DateTime, string>>>
            {
                {"S", new List<Tuple<DateTime, string>>()},
                {"C", new List<Tuple<DateTime, string>>()},
                {"T", new List<Tuple<DateTime, string>>()}
            };

            foreach (var commit in nonExcessCommits)
            {
                string session;
                if (commit.Item1.Hour < 12)
                {
                    session = "S";
                }
                else if (commit.Item1.Hour < 19)
                {
                    session = "C";
                }
                else
                {
                    session = "T";
                }
                sessions[session].Add(commit);
            }

            foreach (var session in sessions)
            {
                while (session.Value.Count < 4 && nonExcessCommits.Any())
                {
                    var commitToAdd = nonExcessCommits.FirstOrDefault();
                    if (commitToAdd != null)
                    {
                        session.Value.Add(commitToAdd);
                        nonExcessCommits.Remove(commitToAdd);
                    }
                }
            }

            while (nonExcessCommits.Any())
            {
                foreach (var session in sessions)
                {
                    if (nonExcessCommits.Any())
                    {
                        var commitToAdd = nonExcessCommits.FirstOrDefault();
                        if (commitToAdd != null)
                        {
                            session.Value.Add(commitToAdd);
                            nonExcessCommits.Remove(commitToAdd);
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }

            foreach (var commit in excessCommits)
            {
                sessions["S"].Add(commit);
            }

            foreach (var session in sessions)
            {
                string sessionCode = session.Key;
                var sessionCommits = session.Value;

                foreach (var commit in sessionCommits)
                {
                    string dayOfWeek = DateHelpers.GetVietnameseDayOfWeek(commit.Item1.DayOfWeek);

                    var existingDayData = weekData.DayDataList.FirstOrDefault(dd => dd.DayOfWeek == dayOfWeek);
                    if (existingDayData == null)
                    {
                        existingDayData = new DayData { DayOfWeek = dayOfWeek, SessionDataList = new List<SessionData>() };
                        weekData.DayDataList.Add(existingDayData);
                    }

                    var existingSessionData = existingDayData.SessionDataList.FirstOrDefault(sd => sd.Session == sessionCode);
                    if (existingSessionData == null)
                    {
                        existingSessionData = new SessionData
                        {
                            Session = sessionCode,
                            Attendance = "Có mặt",
                            AssignedTasks = "",
                            AchievedResults = "",
                            Comments = "",
                            Notes = ""
                        };
                        existingDayData.SessionDataList.Add(existingSessionData);
                    }

                    existingSessionData.AssignedTasks += commit.Item2.Trim() + " ";
                    existingSessionData.AchievedResults += commit.Item2.Trim() + " ";
                }
            }

            if (commitDates.Any())
            {
                weekData.StartDate = commitDates.Min();
                weekData.EndDate = commitDates.Max();
            }
        }

        // Adjusted ProcessCommitsInWeek method
        public void ProcessCommitsInWeek(string filePath, int week, DateTime startDate, DateTime endDate, List<WeekData> weekDatas, List<string> invalidCommits)
        {
            var lines = File.ReadAllLines(filePath);
            var weekData = new WeekData
            {
                WeekNumber = week,
                DayDataList = new List<DayData>()
            };

            var invalidCommitsThisWeek = new List<string>();
            ParseCommitLines(lines, startDate, endDate, weekData, invalidCommitsThisWeek);

            weekDatas.Add(weekData);
            invalidCommits.AddRange(invalidCommitsThisWeek);
        }

        public void InitializeDataGridView(DataGridView dataGridViewCommits)
        {
            // Đặt chế độ tự động điều chỉnh cột để chiếm 100% không gian
            dataGridViewCommits.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Định dạng các cột
            foreach (DataGridViewColumn column in dataGridViewCommits.Columns)
            {
                column.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            }
            dataGridViewCommits.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
        }

    }
}
