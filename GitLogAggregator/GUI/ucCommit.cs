using BUS;
using ET;
using System;
using System.Linq;
using System.Windows.Forms;

namespace GitLogAggregator.GUI
{
    public partial class ucCommit : UserControl
    {
        private readonly ProjectWeekBUS weeksBUS = new();
        private readonly SummaryBUS summaryBUS = new();
        private readonly CommitSummaryBUS commitSummaryBUS = new();
        private readonly CommitBUS commitsBUS = new();
        private readonly CommitPeriodBUS commitPeriodBUS = new();
        private readonly AuthorBUS authorBUS = new();
        public ucCommit()
        {
            InitializeComponent();
        }

        private void UcCommit_Load(object sender, EventArgs e)
        {
            comboBoxWeeks.DataSource = weeksBUS.GetAll();
            comboBoxWeeks.DisplayMember = "WeekName";
            comboBoxWeeks.ValueMember = "WeekId";
        }

        private void LoadCommitsForPeriod(DateTime date, CommitPeriodET period, RichTextBox rtbAssignedTasks, RichTextBox rtbContentResults)
        {
            // Lấy danh sách các commit thuộc ngày và buổi này
            var commits = commitsBUS.GetByDateAndPeriod(date, period.PeriodStartTime, period.PeriodEndTime);

            // Lấy danh sách các tác giả từ AuthorBUS
            var authors = authorBUS.GetAll().ToDictionary(a => a.AuthorID, a => a.AuthorName);

            // Gắn dữ liệu vào RichTextBox AssignedTasks
            rtbAssignedTasks.Text = string.Join("\n", commits.Select(c =>
            {
                // Lấy AuthorName từ danh sách authors dựa trên AuthorID
                string authorName = authors.ContainsKey(c.AuthorID) ? authors[c.AuthorID] : "Unknown Author";

                // Trả về thông tin commit
                return $"- {c.CommitMessages} ({authorName})";
            }));

            // Lấy danh sách CommitSummary thuộc ngày và buổi này
            var summaries = summaryBUS.GetByDateAndPeriod(date, period.PeriodStartTime, period.PeriodEndTime);

            // Truy vấn đến bảng Summary theo ID từ bảng CommitSummary
            if (summaries != null)
            {
                var summaryId = summaries.SummaryID; // Giả định bạn chỉ muốn lấy dữ liệu từ Summary đầu tiên
                var summary = summaryBUS.GetById(summaryId);

                // Gắn dữ liệu Summary vào RichTextBox ContentResults
                if (summary != null)
                {
                    rtbContentResults.Text = summary.ContentResults;
                }
                else
                {
                    rtbContentResults.Clear();
                }
            }
            else
            {
                rtbContentResults.Clear();
            }
        }




        private void SaveSummary(DateTime date, CommitPeriodET period, string contentResults)
        {
            // Lấy dữ liệu Summary từ database hoặc tạo mới nếu chưa có
            var summary = summaryBUS.GetByDateAndPeriod(date, period.PeriodStartTime, period.PeriodEndTime);
            if (summary == null)
            {
                summary = new SummaryET
                {
                    ContentResults = contentResults,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };

                // Thêm mới Summary và lấy SummaryID vừa tạo
                summaryBUS.Add(summary);
                summary = summaryBUS.GetLastInserted(); // Giả định GetLastInserted trả về Summary vừa thêm
            }
            else
            {
                // Cập nhật nội dung Summary nếu đã tồn tại
                summary.ContentResults = contentResults;
                summary.UpdatedAt = DateTime.Now;
                summaryBUS.Update(summary);
            }

            // Lấy danh sách commit thuộc ngày và buổi này
            var commits = commitsBUS.GetByDateAndPeriod(date, period.PeriodStartTime, period.PeriodEndTime);

            foreach (var commit in commits)
            {
                // Kiểm tra nếu liên kết giữa Commit và Summary chưa tồn tại trong CommitSummary
                if (!commitSummaryBUS.Exists(commit.CommitID, summary.SummaryID))
                {
                    // Tạo liên kết mới trong bảng CommitSummary
                    var commitSummary = new CommitSummaryET
                    {
                        CommitID = commit.CommitID,
                        SummaryID = summary.SummaryID,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now
                    };
                    commitSummaryBUS.Add(commitSummary);
                }
            }
        }



        private void AddPeriodControls(TabPage periodTab, DateTime date, CommitPeriodET period)
        {
            // RichTextBox công việc đã giao (chỉ đọc)
            var rtbAssignedTasks = new RichTextBox { Dock = DockStyle.Top, Height = 150, ReadOnly = true };

            // RichTextBox kết quả đạt được (cho phép nhập)
            var rtbContentResults = new RichTextBox { Dock = DockStyle.Fill, ReadOnly = false };

            // Nút lưu
            var btnSave = new Button { Text = "Lưu", Dock = DockStyle.Bottom };

            // Sự kiện khi tab được chọn
            periodTab.Enter += (sender, e) => LoadCommitsForPeriod(date, period, rtbAssignedTasks, rtbContentResults);

            // Xử lý nút lưu
            btnSave.Click += (sender, e) => SaveSummary(date, period, rtbContentResults.Text);

            // Thêm điều khiển vào tab (đảm bảo thứ tự chính xác)
            periodTab.Controls.AddRange([rtbContentResults, rtbAssignedTasks, btnSave]);
        }


        private void ComboBoxWeeks_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxWeeks.SelectedItem is WeekET selectedWeek)
            {
                panelContainer.Controls.Clear();
                var tabControlDays = new TabControl { Dock = DockStyle.Fill };

                for (DateTime date = selectedWeek.WeekStartDate.Value; date <= selectedWeek.WeekEndDate.Value; date = date.AddDays(1))
                {
                    var dayTab = new TabPage(date.ToString("dd/MM/yyyy"));
                    var tabControlPeriods = new TabControl { Dock = DockStyle.Fill };

                    foreach (var period in commitPeriodBUS.GetAll().OrderBy(p => p.PeriodStartTime))
                    {
                        var periodTab = new TabPage(period.PeriodName);
                        AddPeriodControls(periodTab, date, period);
                        tabControlPeriods.TabPages.Add(periodTab);
                    }
                    dayTab.Controls.Add(tabControlPeriods);
                    tabControlDays.TabPages.Add(dayTab);
                }
                panelContainer.Controls.Add(tabControlDays);
            }
        }
    }
}
