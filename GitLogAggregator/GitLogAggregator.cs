using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Drawing;
using GitLogAggregator.BusinessLogic;
using ET;
using BUS;
using System.Data;
using ClosedXML.Excel;
using GitLogAggregator.Utilities;
using System.Globalization;
using System.Data.SqlClient;
using DocumentFormat.OpenXml.Vml;
using Path = System.IO.Path;
using DocumentFormat.OpenXml.Drawing.Charts;
using Size = System.Drawing.Size;
using DataTable = System.Data.DataTable;
using System.Text;


namespace GitLogAggregator
{
    /// <summary>
    /// Ứng dụng Windows Forms (C#) này tổng hợp và phân tích dữ liệu commit từ Git dự án. 
    /// Nó tạo báo cáo commit theo tuần từ ngày bắt đầu dự án cho đến hết 8 tuần thực tập.
    /// </summary>
    public partial class GitLogAggregator : Form
    {
        // đường dẫn thư mục dự án
        private string projectDirectory = string.Empty;
        private string txtDirectoryProjectPath = string.Empty;
        private string txtFolderInternshipPath = string.Empty;
        private string desktopPath = string.Empty;

        // Biến cờ để kiểm tra trạng thái chạy
        private bool isProcessing = false;

        // Git log BUS
        private readonly GitLogUIBUS gitgui_bus = new GitLogUIBUS();

        // Git commit BUS
        private readonly GitLogFormatBUS gitformat_bus = new GitLogFormatBUS();

        // git check commit
        private readonly GitLogCheckCommitBUS gitlogcheckcommit_bus = new GitLogCheckCommitBUS();

        // git load config file
        private readonly ConfigFileBUS configBus = new ConfigFileBUS();

        private readonly InternshipDirectoryBUS internshipDirectoryBUS = new InternshipDirectoryBUS();

        // save week and commit to db
        private readonly ProjectWeekBUS projectWeeksBUS = new ProjectWeekBUS();

        // thong tin commit theo tuan
        private readonly CommitBUS commitBUS = new CommitBUS();

        private readonly RemoveBUS removeBUS = new RemoveBUS();

        // commit group member
        private readonly CommitGroupMemberBUS commitGroupMembersBUS = new CommitGroupMemberBUS();

        private readonly CommitGroupBUS commitGroupsBUS = new CommitGroupBUS();

        private readonly ChatbotSummaryBUS chatbotSummariesBUS = new ChatbotSummaryBUS();


        // Simplified collection initialization for weekDatas and invalidCommits
        List<WeekData> weekDatas = new();
        /// <summary>
        /// DS commit không hợp lệ
        /// </summary>
        List<string> invalidCommits = new();


        public GitLogAggregator()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Vô hiệu hóa các nút và dropdown cho đến khi người dùng chọn thư mục chứa dự án Git.
        /// Tự động tải danh sách tác giả commit sau khi chọn thư mục.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GitLogAggregator_Load(object sender, EventArgs e)
        {
            // Tải dữ liệu từ thư mục `internship_week` và hiển thị lên form
            desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            txtDirectoryProjectPath = Path.Combine(desktopPath, "GitAggregator");

            // Tải danh sách các thư mục thực tập từ cơ sở dữ liệu vào ComboBox
            cboThuMucThucTap.DataSource = internshipDirectoryBUS.GetAll();
            cboThuMucThucTap.ValueMember = "ID"; // Thiết lập trường sẽ làm giá trị
            cboThuMucThucTap.DisplayMember = "InternshipWeekFolder"; // Thiết lập trường sẽ hiển thị trên combobox

            // Lấy đường dẫn thư mục thực tập đã được chọn hoặc mặc định nếu không có
            txtFolderInternshipPath = GetLatestInternshipFolderPath();
            if (string.IsNullOrEmpty(txtFolderInternshipPath))
            {
                txtFolderInternshipPath = Path.Combine(txtDirectoryProjectPath, "internship_week");
            }

            DisableControls();

            txtInternshipStartDate.Enabled = true;// open

            // Cài đặt và hiển thị danh sách dự án listview project
            InitializeProjectListView(listViewProjects);

            SetupFileListView();

            LoadProjectListView();

            // Xây dựng danh sách các tuần và tệp
            BuildWeekFileListView(txtFolderInternshipPath);

            dgvReportCommits.DataSource = commitBUS.GetAll();

        }



        private string GetLatestInternshipFolderPath()
        {
            var directories = internshipDirectoryBUS.GetAll();
            if (directories.Count > 0)
            {
                return directories.First().InternshipWeekFolder;
            }
            return string.Empty;
        }

        /// <summary>
        /// Người dùng chọn thư mục qua hộp thoại.
        /// Load danh sách tác giả commit từ Git bằng lệnh
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <summary> 
        private void BtnAddProject_Click(object sender, EventArgs e)
        {
            btnOpenGitFolder.Enabled = false; // close
            if (folderBrowserDialog.ShowDialog() != DialogResult.OK)
            {
                btnOpenGitFolder.Enabled = true; // open
                return;
            }
            projectDirectory = folderBrowserDialog.SelectedPath;

            if (!IsValidGitRepository(projectDirectory))
            {
                btnOpenGitFolder.Enabled = true; // open

                AppendTextWithScroll("Lỗi: Thư mục được chọn không chứa repository Git hợp lệ. Vui lòng chọn lại.\n");
                return;
            }

            if (!HasCommitsInRepository(projectDirectory))
            {
                btnOpenGitFolder.Enabled = true; // open
                AppendTextWithScroll("Lỗi: Repository Git này không chứa bất kỳ commit nào. Vui lòng chọn một repository khác hoặc tạo commit đầu tiên.\n");
                return;
            }

            AppendTextWithScroll("File hướng dẫn đã có trong thư mục cài đặt tên là ManualUsage.docx.\n");

            // Kiểm tra xem dự án đã tồn tại trong cơ sở dữ liệu chưa
            if (configBus.GetAll().Any(cf => cf.ProjectDirectory == projectDirectory))
            {
                AppendTextWithScroll("Lỗi: Dự án đã tồn tại trong cơ sở dữ liệu.\n");
                return;
            }

            // Lấy thông tin commit
            DateTime firstCommitDate = gitgui_bus.GetFirstCommitDate(projectDirectory);

            // Kiểm tra xem ngày thực tập có hợp lệ không (ngày thực tập phải < ngày commit đầu tiên)
            if (txtInternshipStartDate.Value >= firstCommitDate)
            {
                AppendTextWithScroll("Lỗi: Ngày bắt đầu thực tập phải nhỏ hơn ngày commit đầu tiên.\n");
                return;
            }

            SetMaxDateForDateTimePicker(txtInternshipStartDate, firstCommitDate);

            txtFolderInternshipPath = GetLatestInternshipFolderPath();

            // Xác định và tạo thư mục internship_week trên Desktop
            if (!Directory.Exists(txtFolderInternshipPath))
            {
                Directory.CreateDirectory(txtFolderInternshipPath);
            }

            // Tạo đối tượng ConfigFileET và lưu vào cơ sở dữ liệu
            ConfigFileET configFile = new ConfigFileET
            {
                ProjectDirectory = projectDirectory,
                Author = gitgui_bus.GetFirstCommitAuthor(projectDirectory),
                InternshipStartDate = txtInternshipStartDate.Value,
                InternshipEndDate = txtInternshipEndDate.Value,
                Weeks = (int)txtNumericsWeek.Value,
                FirstCommitDate = firstCommitDate,
                InternshipDirectoryId = (int)cboThuMucThucTap.SelectedValue
            };

            InternshipDirectoryET internshipDirectory = internshipDirectoryBUS.GetByID(configFile.InternshipDirectoryId);
            internshipDirectory.InternshipWeekFolder = txtFolderInternshipPath;

            // Cập nhật giao diện khi chọn thư mục dự án
            UpdateControls(configFile);

            // Thêm thông tin cấu hình dự án
            configBus.Add(configFile);

            // Load lại dữ liệu lên ListView
            LoadProjectListView();

            AppendTextWithScroll("Dự án và thông tin cấu hình đã được thêm vào cơ sở dữ liệu thành công.\n");

            btnOpenGitFolder.Enabled = true; // open
            btnAggregator.Enabled = true; // open
        }




        /// <summary>
        /// Cập nhật giao diện với thông tin cấu hình
        /// </summary>
        /// <param name="configInfo">Đối tượng ConfigFile chứa thông tin cấu hình</param>
        private void UpdateControls(ConfigFileET configInfo)
        {
            // load danh sách tác giả
            cboAuthorCommit.DataSource = gitgui_bus.LoadAuthorsCombobox(configInfo.ProjectDirectory);
            // hiển thị tác giả commit đầu tiên
            cboAuthorCommit.SelectedItem = configInfo.Author;

            txtInternshipStartDate.Value = (DateTime)configInfo.InternshipStartDate;
            txtInternshipEndDate.Value = (DateTime)configInfo.InternshipEndDate;
            txtNumericsWeek.Value = configInfo.Weeks;

            txtFirstCommitDate.Value = (DateTime)configInfo.FirstCommitDate;
        }

        private void ListViewProjects_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (e.IsSelected)
            {
                ConfigFileET config = configBus.GetByID(int.Parse(e.Item.Text));
                UpdateControls(config);

                // Kiểm tra nếu thư mục internship_week tồn tại thì hiển thị week folder thực tập
                if (Directory.Exists(txtFolderInternshipPath))
                {
                    // Hiển thị dữ liệu thư mục và commit
                    BuildWeekFileListView(txtDirectoryProjectPath);
                    // Hiển thị ngày thực tập tối đa có thể chọn.
                    SetMaxDateForDateTimePicker(txtInternshipStartDate, (DateTime)config.FirstCommitDate);
                    UpdateControlState(isEnabled: true);
                }
                else
                {
                    DisableControls();
                    cboAuthorCommit.Enabled = true;
                    txtInternshipStartDate.Enabled = true;
                    btnOpenGitFolder.Enabled = true;
                    btnAggregator.Enabled = true;
                    fileListView.Items.Clear();
                    weekListView.Items.Clear();
                    AppendTextWithScroll("Vui lòng tổng hợp commit mới.\n");
                }
            }
        }



        private void SetMaxDateForDateTimePicker(DateTimePicker dateTimePicker, DateTime maxDate)
        {
            dateTimePicker.MaxDate = maxDate;
        }

        private void InitializeProjectListView(ListView listViewProjects)
        {
            listViewProjects.Columns.Clear();
            listViewProjects.View = View.Details;
            listViewProjects.FullRowSelect = true; // Đảm bảo chọn toàn bộ hàng
            listViewProjects.MultiSelect = false; // Đảm bảo chỉ chọn một hàng tại một thời điểm

            // Thiết lập các cột cho listViewProjects (nếu chưa thêm trước đó)
            if (listViewProjects.Columns.Count == 0)
            {
                listViewProjects.Columns.Add("STT", 50); // Cột số thứ tự 
                listViewProjects.Columns.Add("Đường dẫn dự án", 200); // Đường dẫn dự án
                listViewProjects.Columns.Add("Tác giả", 150); // Tác giả
                listViewProjects.Columns.Add("Ngày bắt đầu", 100); // Ngày bắt đầu
                listViewProjects.Columns.Add("Ngày kết thúc", 100); // Ngày kết thúc
                listViewProjects.Columns.Add("Số tuần", 70); // Số tuần thực tập
                listViewProjects.Columns.Add("Ngày commit đầu tiên", 150); // Ngày commit đầu tiên
                listViewProjects.Columns.Add("Thư mục thực tập", 200); // Thư mục thực tập
            }

            // Tự động điều chỉnh kích thước cột
            fileListView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
        }
        private bool IsValidGitRepository(string directory)
        {
            string gitFolder = Path.Combine(directory, ".git");
            return Directory.Exists(gitFolder);
        }
        private bool HasCommitsInRepository(string directory)
        {
            // Chạy lệnh Git để lấy các commit
            string gitCommand = "log --oneline";
            string logOutput = gitgui_bus.RunGitCommand(gitCommand, directory);

            // Kiểm tra nếu output rỗng
            return !string.IsNullOrEmpty(logOutput);
        }
        /// <summary>
        /// Load thông tin list view trống sau khi chọn thêm dự án
        /// </summary>
        /// <param name="internshipWeekFolder"></param>
        public void LoadProjectListView()
        {
            var configFiles = configBus.GetAll();
            listViewProjects.Items.Clear(); // Xóa dữ liệu cũ trước khi bắt đầu thêm mới 
            foreach (var config in configFiles)
            {
                // Hiển thị thông tin đường dẫn dự án
                ListViewItem item = new ListViewItem(config.ID.ToString());         // ID
                item.SubItems.Add(config.ProjectDirectory);                         // Đường dẫn dự án
                item.SubItems.Add(config.Author);                                   // Tác giả thực hiện commit đầu tiên
                item.SubItems.Add(((DateTime)config.InternshipStartDate).ToString("yyyy-MM-dd"));         // Ngày bắt đầu thực tập
                item.SubItems.Add(((DateTime)config.InternshipEndDate).ToString("yyyy-MM-dd"));           // Ngày kết thúc thực tập
                item.SubItems.Add(config.Weeks.ToString());                         // Số tuần thực tập
                item.SubItems.Add(((DateTime)config.FirstCommitDate).ToString("yyyy-MM-dd"));   // Ngày commit đầu tiên
                item.SubItems.Add(config.InternshipDirectoryId.ToString());                     // Thư mục thực tập
                listViewProjects.Items.Add(item);
            }
        }

        /// <summary>
        /// Cập nhật trạng thái giao diện (các nút và control)
        /// </summary>
        private void UpdateControlState(bool isEnabled)
        {
            btnClearDataListView.Enabled = isEnabled;
            btnExportReportExcelCommits.Enabled = isEnabled;
            btnReviewCommitsError.Enabled = isEnabled;
            btnDeleteCommitsError.Enabled = !isEnabled;
            btnExpandReport.Enabled = !isEnabled;
            txtInternshipEndDate.Enabled = !isEnabled;
            txtFirstCommitDate.Enabled = !isEnabled;
            txtNumericsWeek.Enabled = !isEnabled;
        }

        /// <summary>
        /// Xử lý trường hợp file config bị thiếu
        /// </summary>

        private void EnableControls()
        {
            cboAuthorCommit.Enabled = true;
            cboThuMucThucTap.Enabled = true;
            txtNumericsWeek.Enabled = true;
            txtInternshipStartDate.Enabled = true;
            txtInternshipEndDate.Enabled = true;
            txtFirstCommitDate.Enabled = true;
            btnAggregator.Enabled = true;
            btnClearDataListView.Enabled = true;
            btnOpenGitFolder.Enabled = true;
            btnExportReportExcelCommits.Enabled = true;
            btnReviewCommitsError.Enabled = true;
            btnDeleteCommitsError.Enabled = true;
            btnExpandReport.Enabled = true;
            // Thêm các điều khiển khác nếu cần
        }
        private void DisableControls()
        {
            cboAuthorCommit.Enabled = false;
            cboThuMucThucTap.Enabled = false;
            txtNumericsWeek.Enabled = false;
            txtInternshipStartDate.Enabled = false;
            txtInternshipEndDate.Enabled = false;
            txtFirstCommitDate.Enabled = false;
            btnAggregator.Enabled = false;
            btnClearDataListView.Enabled = false;
            btnOpenGitFolder.Enabled = false;
            btnExportReportExcelCommits.Enabled = false;
            btnReviewCommitsError.Enabled = false;
            btnDeleteCommitsError.Enabled = false;
            btnExpandReport.Enabled = false;
            // Thêm các điều khiển khác nếu cần
        }
        /// <summary>
        /// Duyệt qua 8 tuần kể từ ngày bắt đầu thực tập.
        /// Tạo thư mục theo từng tuần và file log commit hằng ngày.
        /// Chạy lệnh Git để lấy commit của từng ngày
        /// Xuất báo cáo commit vào file và hiển thị kết quả trong giao diện
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnAggregateCommits_Click(object sender, EventArgs e)
        {
            if (listViewProjects.Items.Count == 0)
            {
                AppendTextWithScroll("Vui lòng thêm ít nhất một dự án vào danh sách trước khi tổng hợp commit.\n");
                return;
            }

            if (isProcessing)
            {
                AppendTextWithScroll("Chương trình đang xử lý, vui lòng chờ...\n");
                return;
            }

            isProcessing = true;
            DisableControls();

            try
            {

                // Lấy thông tin từ UI
                string author = cboAuthorCommit.SelectedItem?.ToString();
                DateTime internshipStartDate = txtInternshipStartDate.Value;
                DateTime internshipEndDate = txtInternshipEndDate.Value;

                // Tổng hợp commit
                AggregateCommitsForProjects();

                // Kiểm tra nếu thư mục internship_week tồn tại thì hiển thị dữ liệu lên listview
                if (Directory.Exists(txtFolderInternshipPath))
                {
                    // Hiển thị dữ liệu thư mục và commit
                    BuildWeekFileListView(txtFolderInternshipPath);
                }

                AppendTextWithScroll("Đã tổng hợp tất cả commit từ tất cả các dự án.\n");
            }
            catch (Exception ex)
            {
                AppendTextWithScroll($"Lỗi: {ex.Message}\n");
            }
            finally
            {
                AppendLogMessages(); // Hiển thị các thông báo
                isProcessing = false;

                EnableControls();
            }
        }
        private void AggregateCommitsForProjects()
        {
            foreach (ListViewItem item in listViewProjects.Items)
            {
                if (int.TryParse(item.Text, out int configFileId))
                {
                    // Lấy thông tin dự án từ cơ sở dữ liệu
                    ConfigFileET configFile = configBus.GetByID(configFileId);

                    if (configFile != null)
                    {
                        var internshipWeekFolder = internshipDirectoryBUS.GetByID(configFile.InternshipDirectoryId).InternshipWeekFolder;

                        // Tạo thư mục theo tuần
                        gitgui_bus.AggregateCommits(configFile.ProjectDirectory, configFile.Author, configFile.InternshipStartDate, configFile.Weeks, internshipWeekFolder);

                        AppendTextWithScroll($"Đã tổng hợp commit cho dự án: {projectDirectory}.\n");
                    }
                }
            }
        }



        /// <summary>
        /// Xóa thư mục: Nút xóa sẽ chỉ xóa những thư mục hoặc file không cần thiết
        /// mà không cần phải tạo lại những thư mục con như internship_week hoặc các thư mục tuần.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnClearDataListView_Click(object sender, EventArgs e)
        {
            txtFolderInternshipPath = GetLatestInternshipFolderPath();

            // Kiểm tra sự tồn tại của thư mục internship_week
            if (!Directory.Exists(txtFolderInternshipPath))
            {
                AppendTextWithScroll("Thư mục thực tập không tồn tại.\n");
                return; // Dừng nếu thư mục internship_week không tồn tại
            }

            // Hộp thoại xác nhận xóa
            var result = MessageBox.Show("Bạn có chắc chắn muốn xóa?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result != DialogResult.Yes)
            {
                return; // Dừng nếu người dùng không xác nhận xóa
            }

            try
            {
                DisableControls();

                // Xóa toàn bộ thư mục internship_week
                try
                {
                    Directory.Delete(txtFolderInternshipPath, true); // Xóa tất cả các file và thư mục con
                    AppendTextWithScroll($"Đã xóa toàn bộ thư mục thực tập: {txtFolderInternshipPath}\n");
                }
                catch (UnauthorizedAccessException ex)
                {
                    AppendTextWithScroll($"Lỗi: Không có quyền truy cập để xóa thư mục '{txtFolderInternshipPath}'. Chi tiết: {ex.Message}\n");
                    return; // Dừng nếu không thể xóa thư mục do quyền truy cập
                }

                // Xóa tất cả các mục trong bảng listview project
                RemoveSelectedProjects();

                // Xóa tất cả các mục trong ListView
                weekListView.Items.Clear();
                AppendTextWithScroll("Danh sách thư mục đã được làm trống.\n");

                fileListView.Items.Clear();
                AppendTextWithScroll("Danh sách file đã được làm trống.\n");

                listViewProjects.Items.Clear();
                AppendTextWithScroll("Danh sách config đã được làm trống.\n");

                checkedListBoxCommitsError.Items.Clear();
                AppendTextWithScroll("Danh sách commit đã được làm trống.\n");

                dgvReportCommits.DataSource = null;
                AppendTextWithScroll("Danh sách công việc đã được làm trống.\n");

                cboAuthorCommit.DataSource = null;

                cboThuMucThucTap.DataSource = null;



                // Xóa dữ liệu trong các bảng
                removeBUS.ClearAllTables();

                // Tải lại danh sách listViewProjects
                LoadListViewProjects(configBus.GetAll());


                AppendTextWithScroll("Xóa thư mục internship_week và các mục trong bảng ConfigFile hoàn tất.\n");
            }
            catch (Exception ex)
            {
                AppendTextWithScroll($"Lỗi: {ex.Message}\n");
            }
            finally
            {
                EnableControls();
                btnClearDataListView.Enabled = false;  // Vô hiệu hóa nút xóa
                AppendTextWithScroll("Nút xóa đã bị vô hiệu hóa sau khi xóa thư mục.\n");
                btnDeleteCommitsError.Enabled = false;
                btnReviewCommitsError.Enabled = false;
                AppendTextWithScroll("Nút Kiểm tra đã bị vô hiệu hóa sau khi xóa thư mục.\n");
                btnExportReportExcelCommits.Enabled = false;
                txtInternshipEndDate.Enabled = false;
                txtFirstCommitDate.Enabled = false;
                txtNumericsWeek.Enabled = false;
                txtFolderInternshipPath = string.Empty;
                txtDirectoryProjectPath = string.Empty;
            }
        }

        private void RemoveSelectedProjects()
        {
            // Xóa các mục được chọn trong ListViewProjects
            foreach (ListViewItem item in listViewProjects.Items)
            {
                if (int.TryParse(item.Text, out int configFileId))
                {
                    // Xóa mục khỏi cơ sở dữ liệu
                    configBus.Delete(configFileId);

                    // Xóa các mục liên quan trong ListView
                    item.Remove();
                }
            }
            AppendTextWithScroll("Đã xóa các mục được chọn trong bảng ConfigFile.\n");
        }
        // Hàm để tải lại danh sách listViewProjects từ cơ sở dữ liệu
        private void LoadListViewProjects(List<ConfigFileET> configFiles)
        {
            listViewProjects.Items.Clear();
            foreach (var configFile in configFiles)
            {

                var internshipWeekFolder = internshipDirectoryBUS.GetByID(configFile.InternshipDirectoryId).InternshipWeekFolder;

                ListViewItem item = new ListViewItem(configFile.ID.ToString());
                item.SubItems.Add(configFile.ProjectDirectory);
                item.SubItems.Add(configFile.Author);
                item.SubItems.Add(((DateTime)configFile.InternshipStartDate).ToShortDateString());
                item.SubItems.Add(((DateTime)configFile.InternshipEndDate).ToShortDateString());
                item.SubItems.Add(configFile.Weeks.ToString());
                item.SubItems.Add(((DateTime)configFile.FirstCommitDate).ToShortDateString());
                item.SubItems.Add(internshipWeekFolder);
                listViewProjects.Items.Add(item);
            }
        }
        private void SetupFileListView()
        {
            fileListView.View = View.Details;// Hiển thị chế độ chi tiết
            fileListView.FullRowSelect = true; // Đảm bảo chọn toàn bộ hàng
            fileListView.MultiSelect = false; // Đảm bảo chỉ chọn một hàng tại một thời điểm

            // Thiết lập các cột cho listViewProjects (nếu chưa thêm trước đó)
            if (fileListView.Columns.Count == 0)
            {
                // Thêm các cột cho ListView
                fileListView.Columns.Add("STT", 40); // Số thứ tự
                fileListView.Columns.Add("Tên File", 120); // Tên file
                fileListView.Columns.Add("Ngày Tạo", 100); // Ngày tạo
            }

            // Tự động điều chỉnh kích thước cột
            fileListView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
        }



        /// <summary>
        /// Hiển thị danh sách file từ tất cả các thư mục tuần trong fileListView
        /// </summary>
        private void BuildWeekFileListView(string txtDirectoryProjectPath)
        {

            // Xác định đường dẫn thư mục internship_week
            txtFolderInternshipPath = GetLatestInternshipFolderPath();
            // Kiểm tra xem thư mục internship_week có tồn tại không
            if (!Directory.Exists(txtFolderInternshipPath))
            {
                AppendTextWithScroll("Thư mục internship_week không tồn tại.\n");
                return;
            }

            // Khởi tạo ImageList cho weekListView
            imageList.ImageSize = new Size(32, 32);
            imageList.Images.Add("folder", Properties.Resources.Git_commit_aggregation_tool);
            weekListView.SmallImageList = imageList;

            // Lấy danh sách thư mục trong thư mục internship_week
            string[] weekFoldersPath = Directory.GetDirectories(txtFolderInternshipPath);
            if (weekFoldersPath == null)
            {
                AppendTextWithScroll("Thư mục internship_week không được tổng hợp các week commit.\n");
                return;
            }
            // Xóa các mục hiện có trong ListView
            weekListView.Items.Clear();
            fileListView.Items.Clear();

            int totalFiles = 0;
            int emptyDirectories = 0;
            int directoriesWithCommits = 0;

            // Danh sách file để tổng hợp từ tất cả các thư mục
            List<(string FilePath, DateTime CreationTime)> allFiles = new List<(string, DateTime)>();

            foreach (string folder in weekFoldersPath)
            {
                // Lấy danh sách file trong thư mục
                var files = Directory.GetFiles(folder);

                // Kiểm tra thư mục có file commit nào không
                var combinedFile = files.FirstOrDefault(f => Path.GetFileName(f).Equals("combined_commits.txt", StringComparison.OrdinalIgnoreCase));
                var otherFiles = files.Where(f => !Path.GetFileName(f).Equals("combined_commits.txt", StringComparison.OrdinalIgnoreCase))
                                      .OrderBy(f => File.GetCreationTime(f)) // Sắp xếp theo ngày tạo
                                      .ToList();

                if (combinedFile == null && otherFiles.Count == 0)
                {
                    // Nếu thư mục không có file nào, bỏ qua thư mục này
                    emptyDirectories++;
                    AppendTextWithScroll($"Thư mục: {Path.GetFileName(folder)} - Không có commit.\n");
                    continue;
                }

                // Thêm thư mục có file commit vào weekListView
                ListViewItem item = new ListViewItem(Path.GetFileName(folder))
                {
                    ImageKey = "folder", // Sử dụng icon thư mục
                    Tag = folder         // Lưu đường dẫn đầy đủ của thư mục vào thuộc tính Tag của ListViewItem
                };
                weekListView.Items.Add(item);

                // Thêm file combined_commits.txt (nếu có) vào danh sách tổng hợp
                if (combinedFile != null)
                {
                    allFiles.Add((combinedFile, File.GetCreationTime(combinedFile)));
                    totalFiles++;
                }

                // Thêm các file khác vào danh sách tổng hợp
                foreach (var file in otherFiles)
                {
                    allFiles.Add((file, File.GetCreationTime(file)));
                    totalFiles++;
                }

                // Đếm số lượng commit trong thư mục
                int commitsCount = CountCommitsInFolder(folder);
                directoriesWithCommits++;
                AppendTextWithScroll($"Thư mục: {Path.GetFileName(folder)} - Số lượng file: {files.Length} - Số lượng commit: {commitsCount}\n");
            }

            // Hiển thị thông báo nếu có thư mục
            if (weekFoldersPath.Length != 0)
            {
                AppendTextWithScroll($"Tổng số thư mục: {weekFoldersPath.Length}\n");
                AppendTextWithScroll($"Số thư mục trống: {emptyDirectories}\n");
                AppendTextWithScroll($"Số thư mục có commit: {directoriesWithCommits}\n");
                AppendTextWithScroll($"Tổng số file: {totalFiles}\n");
            }

            // Hiển thị tất cả các file từ danh sách tổng hợp lên fileListView, sắp xếp theo ngày tạo
            var sortedFiles = allFiles.OrderBy(f => f.CreationTime).ToList();
            int fileOrder = 1;
            foreach (var (filePath, creationTime) in sortedFiles)
            {
                string fileName = Path.GetFileName(filePath);
                var fileItem = new ListViewItem(fileOrder++.ToString());
                fileItem.SubItems.Add(fileName);
                fileItem.SubItems.Add(creationTime.ToString("dd/MM/yyyy HH:mm:ss")); // Thêm ngày tạo
                fileItem.Tag = filePath; // Lưu đường dẫn đầy đủ của file vào thuộc tính Tag của ListViewItem
                fileListView.Items.Add(fileItem);
            }
        }



        private int CountCommitsInFolder(string folderPath)
        {
            // Giả sử mỗi file commit chứa các dòng ghi log commit
            // Thay thế bằng logic thực tế để đếm số lượng commit
            int commitCount = 0;
            var commitFiles = Directory.GetFiles(folderPath, "*.txt");
            foreach (var file in commitFiles)
            {
                var lines = File.ReadAllLines(file);
                commitCount += lines.Length; // Giả định mỗi dòng là một commit
            }
            return commitCount;
        }




        /// <summary>
        /// Phương thức bổ sung văn bản và cuộn đến cuối
        /// </summary>
        /// <param name="text"></param>
        private void AppendTextWithScroll(string text)
        {
            txtResult.AppendText(text);
            txtResult.ScrollToCaret();
        }
        private void AppendEventsWithScroll(string text)
        {
            txtResultMouseEvents.Clear();
            txtResultMouseEvents.AppendText(text);
            txtResultMouseEvents.ScrollToCaret();
        }

        private void AppendLogMessages()
        {
            foreach (var message in gitformat_bus.LogMessages)
            {
                AppendTextWithScroll(message + "\n");
            }
        }
        /// <summary>
        /// xem hướng dẫn thực hiện
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HelpButton_Click(object sender, EventArgs e)
        {
            string helpText = "Hướng Dẫn Sử Dụng Công Cụ Quản Lý CommitET Git:\n\n" +
                              "1. Chọn Thư Mục Dự Án Git:\n" +
                              "   - Nhấn nút 'Chọn Dự Án'.\n" +
                              "   - Chọn thư mục chứa dự án Git của bạn.\n" +
                              "   - Chương trình sẽ kiểm tra xem thư mục có chứa repository Git hợp lệ và có commit nào không:\n" +
                              "     + Nếu không chứa repository Git, sẽ hiển thị thông báo lỗi: 'Thư mục được chọn không chứa repository Git hợp lệ. Vui lòng chọn lại.'.\n" +
                              "     + Nếu repository Git không có commit nào, sẽ hiển thị thông báo lỗi: 'Repository Git này không chứa bất kỳ commit nào. Vui lòng chọn một repository khác hoặc tạo commit đầu tiên.'.\n" +
                              "     + Nếu hợp lệ, chương trình sẽ:\n" +
                              "        * Tạo thư mục internship_week nếu chưa có.\n" +
                              "        * Hiển thị đường dẫn dự án.\n" +
                              "        * Bật các nút chọn tác giả, ngày bắt đầu và nút tổng hợp commit.\n\n" +
                              "2. Chọn Tác Giả và Ngày Bắt Đầu:\n" +
                              "   - Chọn tác giả từ danh sách 'Tác Giả'.\n" +
                              "   - Chọn ngày bắt đầu thực tập từ 'Ngày Bắt Đầu'.\n\n" +
                              "3. Tổng Hợp CommitET:\n" +
                              "   - Nhấn nút 'Tổng Hợp' để bắt đầu tổng hợp commit.\n" +
                              "   - Quá trình tổng hợp sẽ:\n" +
                              "     + Tạo thư mục tuần bên trong internship_week.\n" +
                              "     + Lấy dữ liệu commit cho từng ngày trong tuần.\n" +
                              "     + Lưu dữ liệu vào các file riêng theo từng tuần.\n" +
                              "   - Sau khi hoàn tất, danh sách tuần sẽ hiển thị trong 'Danh Sách Thư Mục'.\n" +
                              "   - Lưu ý: Trong khi quá trình tổng hợp đang chạy, nút tổng hợp sẽ bị vô hiệu hóa cho đến khi hoàn thành.\n\n" +
                              "4. Xem Danh Sách Thư Mục:\n" +
                              "   - Danh sách các thư mục trong internship_week sẽ được hiển thị ở bảng 'Danh Sách Thư Mục'.\n" +
                              "   - Mỗi thư mục đại diện cho một tuần trong dự án.\n" +
                              "   - Khi click vào một thư mục trong 'Danh Sách Thư Mục', danh sách file trong thư mục đó sẽ hiển thị ở bảng 'Danh Sách File'.\n\n" +
                              "5. Xóa Dữ Liệu:\n" +
                              "   - Nhấn nút 'Xóa' để xóa thư mục internship_week cùng tất cả dữ liệu bên trong.\n" +
                              "   - Sau khi xóa, nút xóa sẽ bị vô hiệu hóa cho đến khi thực hiện tổng hợp lại dữ liệu.\n" +
                              "   - Danh sách trong 'Danh Sách Thư Mục' và 'Danh Sách File' sẽ được làm trống.\n\n" +
                              "Ghi Chú:\n" +
                              "- Nút 'Tổng Hợp' chỉ được kích hoạt sau khi chọn thư mục dự án.\n" +
                              "- Nút 'Xóa' chỉ xuất hiện sau khi đã tổng hợp thành công.\n" +
                              "- Các thông báo lỗi sẽ hiển thị khi:\n" +
                              "   * Thư mục được chọn không chứa repository Git hợp lệ.\n" +
                              "   * Repository Git không chứa bất kỳ commit nào.";

            MessageBox.Show(helpText, "Hướng Dẫn Sử Dụng", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        /// <summary>
        /// Xem nội dung file commit 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FileListView_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (fileListView.SelectedItems.Count > 0)
            {
                ListViewItem selectedItem = fileListView.SelectedItems[0];
                string filePath = selectedItem.Tag.ToString();

                try
                {
                    Process.Start(filePath); // Mở file bằng ứng dụng mặc định
                }
                catch (Exception ex)
                {
                    AppendTextWithScroll($"Không thể mở file: {ex.Message}\n");
                }
            }
        }
        /// <summary>
        /// Xử lý sự kiện click vào thư mục trong weekListView và hiển thị danh sách file trong thư mục được chọn
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WeekListView_MouseClick(object sender, MouseEventArgs e)
        {
            if (weekListView.SelectedItems.Count > 0)
            {
                ListViewItem selectedItem = weekListView.SelectedItems[0];

                // Kiểm tra null trước khi sử dụng thuộc tính Tag
                if (selectedItem.Tag != null)
                {
                    string basePath = txtFolderInternshipPath; // Sử dụng biến txtFolderInternshipPath

                    string folderPath = Path.Combine(basePath, selectedItem.Tag.ToString()); // Lấy đường dẫn đầy đủ của thư mục
                    string folderName = Path.GetFileName(folderPath); // Lấy tên thư mục
                    AppendTextWithScroll($"Bạn đã chọn thư mục tuần: {folderName}\n");

                    // Hiển thị và sắp xếp các file từ thư mục tuần được chọn
                    DisplayAndSortFilesFromSelectedWeek(folderPath);
                }
                else
                {
                    AppendTextWithScroll("Đường dẫn thư mục không tồn tại.\n");
                }
            }
        }



        /// <summary>
        /// Hiển thị và sắp xếp danh sách file từ thư mục tuần được chọn
        /// </summary>
        /// <param name="directoryPath"></param>
        private void DisplayAndSortFilesFromSelectedWeek(string directoryPath)
        {
            if (!Directory.Exists(directoryPath))
            {
                AppendTextWithScroll("Thư mục không tồn tại.\n");
                return;
            }

            // Xóa danh sách file hiện tại trong fileListView
            fileListView.Items.Clear();

            // Lấy danh sách file trong thư mục được chọn
            var files = Directory.GetFiles(directoryPath);

            if (files.Length == 0)
            {
                AppendTextWithScroll("Thư mục không có file nào.\n");
                return;
            }

            // Tách file "combined_commits.txt" ra trước
            var combinedFile = files.FirstOrDefault(f => Path.GetFileName(f).Equals("combined_commits.txt", StringComparison.OrdinalIgnoreCase));
            var otherFiles = files.Where(f => !Path.GetFileName(f).Equals("combined_commits.txt", StringComparison.OrdinalIgnoreCase))
                                  .OrderBy(f => File.GetCreationTime(f)) // Sắp xếp theo ngày tạo
                                  .ToList();

            int index = 1; // Đếm số thứ tự file

            // Thêm file "combined_commits.txt" trước (nếu có)
            if (combinedFile != null)
            {
                var combinedFileName = Path.GetFileName(combinedFile);
                var combinedFileCreationTime = File.GetCreationTime(combinedFile).ToString("dd/MM/yyyy HH:mm:ss");

                ListViewItem combinedFileItem = new ListViewItem(index++.ToString());
                combinedFileItem.SubItems.Add(combinedFileName); // Tên file
                combinedFileItem.SubItems.Add(combinedFileCreationTime); // Ngày tạo
                combinedFileItem.Tag = combinedFile; // Lưu đường dẫn file
                fileListView.Items.Add(combinedFileItem);
            }

            // Thêm các file còn lại
            foreach (var file in otherFiles)
            {
                string fileName = Path.GetFileName(file);
                string fileCreationTime = File.GetCreationTime(file).ToString("dd/MM/yyyy HH:mm:ss");

                ListViewItem fileItem = new ListViewItem(index++.ToString());
                fileItem.SubItems.Add(fileName); // Tên file
                fileItem.SubItems.Add(fileCreationTime); // Ngày tạo
                fileItem.Tag = file; // Lưu đường dẫn file
                fileListView.Items.Add(fileItem);
            }

            AppendTextWithScroll($"Tổng số file trong tuần: {files.Length}\n");
        }



        private void FileListView_MouseClick(object sender, MouseEventArgs e)
        {

            if (fileListView.SelectedItems.Count > 0)
            {
                return;
            }
            // Lấy mục được chọn
            ListViewItem selectedItem = fileListView.SelectedItems[0];

            // Lấy đường dẫn đầy đủ của file từ thuộc tính Tag của ListViewItem
            string filePath = selectedItem.Tag.ToString();

            // Đọc danh sách commit từ file
            List<string> commits = gitgui_bus.ReadCommitsFromFile(filePath);

            // Hiển thị danh sách commit trong checkedListBox1
            checkedListBoxCommitsError.Items.Clear();
            foreach (var commit in commits)
            {
                checkedListBoxCommitsError.Items.Add(commit);
            }
        }

        private void BtnExportExcel_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvReportCommits.DataSource == null)
                {
                    AppendTextWithScroll("Không có dữ liệu để xuất.\n");
                    return;
                }
                string filePath = Path.Combine(desktopPath, "commits.xlsx");

                DataTable dataTable = (System.Data.DataTable)dgvReportCommits.DataSource;
                List<DayData> dayDataList = gitformat_bus.ConvertDataTableToDayDataList(dataTable);

                // Chuyển đổi `List<DayData>` thành `List<WeekData>`
                List<WeekData> weekDataList = gitformat_bus.ConvertDayDataListToWeekDataList(dayDataList, txtInternshipStartDate.Value, txtInternshipEndDate.Value);

                gitformat_bus.CreateExcelFile(filePath, weekDataList, txtInternshipEndDate.Value);

                AppendTextWithScroll("Xuất Excel thành công! File đã được lưu trên Desktop của bạn.\n");
            }
            catch (Exception ex)
            {
                AppendTextWithScroll($"Lỗi: {ex.Message}");
            }
        }
        private void NumericWeeks_ValueChanged(object sender, EventArgs e)
        {
            DateTime startDate = txtInternshipStartDate.Value;
            int weeks = (int)txtNumericsWeek.Value;
            DateTime endDate = gitgui_bus.CalculateEndDate(startDate, weeks);

            // Hiển thị ngày kết thúc
            txtInternshipEndDate.Value = endDate;
            btnOpenGitFolder.Enabled = true;// chọn tuần thực tập xong mới được thêm dự án
        }
        /// <summary>
        /// Kiểm tra commit lỗi được hiển thị lên listcheckbox, commit hợp lệ hiển thị lên datagridview theo mẫu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnReviewCommits_Click(object sender, EventArgs e)
        {
            btnReviewCommitsError.Enabled = false;
            txtResult.Clear();
            string internshipWeekFolder = Path.Combine(txtDirectoryProjectPath, "internship_week");

            if (!Directory.Exists(internshipWeekFolder))
            {
                AppendTextWithScroll("Thư mục internship_week không tồn tại.\n");
                btnReviewCommitsError.Enabled = true;
                return;
            }

            DateTime internshipStartDate = txtInternshipStartDate.Value;
            DateTime internshipEndDate = txtInternshipEndDate.Value;
            int totalWeeks = (int)txtNumericsWeek.Value;

            weekDatas = new List<WeekData>();
            invalidCommits = new List<string>();

            for (int week = 1; week <= totalWeeks; week++)
            {
                AppendTextWithScroll($"Đang xử lý tuần {week}...\n");
                string weekFolder = Path.Combine(internshipWeekFolder, $"Week_{week}");
                string combinedFilePath = Path.Combine(weekFolder, "combined_commits.txt");

                if (!CheckDirectoriesAndFiles(weekFolder, combinedFilePath, week))
                    continue;

                gitlogcheckcommit_bus.ProcessCommitsInWeek(combinedFilePath, week, internshipStartDate, internshipEndDate, weekDatas, invalidCommits);
            }

            checkedListBoxCommitsError.Items.Clear();
            foreach (var commit in invalidCommits)
            {
                checkedListBoxCommitsError.Items.Add(commit);
            }

            DataTable dataTable = gitlogcheckcommit_bus.ConvertToDataTable(weekDatas);
            dgvReportCommits.DataSource = dataTable;

            int invalidCommitCount = invalidCommits.Count;
            if (invalidCommitCount == 0)
            {
                AppendTextWithScroll("Không có commit không hợp lệ.\n");
            }
            else
            {
                AppendTextWithScroll($"Kiểm tra thành công. CommitET không hợp lệ: {invalidCommitCount}\n");
            }
            btnReviewCommitsError.Enabled = false;
            btnDeleteCommitsError.Enabled = false;
            btnExpandReport.Enabled = true;
        }

        private void BtnDeleteCommits_Click(object sender, EventArgs e)
        {
            if (invalidCommits.Count == 0)
            {
                AppendTextWithScroll("Không thể thực hiện. Danh sách commit lỗi đang trống.\n");
                return;
            }

            // Step 1: Confirm deletion with the user
            DialogResult result = MessageBox.Show("Bạn có chắc là muốn xóa trực tiếp các commit trong các file combined_commits.txt?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result != DialogResult.Yes)
                return;

            // Step 2: Remove invalid commits from files
            int totalWeeks = (int)txtNumericsWeek.Value;

            List<string> invalidLines = invalidCommits;
            string internshipWeekFolder = Path.Combine(txtDirectoryProjectPath, "internship_week");

            for (int week = 1; week <= totalWeeks; week++)
            {
                AppendTextWithScroll($"Đang xử lý tuần {week}...\n");
                string weekFolder = Path.Combine(internshipWeekFolder, $"Week_{week}");
                string combinedFilePath = Path.Combine(weekFolder, "combined_commits.txt");

                if (!CheckDirectoriesAndFiles(weekFolder, combinedFilePath, week))
                    continue;

                try
                {
                    List<string> lines = File.ReadAllLines(combinedFilePath).ToList();
                    lines = lines.Where(line => !invalidLines.Contains(line)).ToList();
                    File.WriteAllLines(combinedFilePath, lines);
                    AppendTextWithScroll($"Deleted invalid commits from {combinedFilePath}\n");
                }
                catch (Exception ex)
                {
                    AppendTextWithScroll($"Error deleting invalid commits from {combinedFilePath}: {ex.Message}\n");
                }
            }

            // Step 4: Update the UI
            // Refresh the DataGridView
            DataTable dataTable = gitlogcheckcommit_bus.ConvertToDataTable(weekDatas);
            dgvReportCommits.DataSource = dataTable;

            // Clear the data source of the CheckedListBox
            checkedListBoxCommitsError.Items.Clear();

            // Inform the user that the process is complete
            btnDeleteCommitsError.Enabled = false;
            AppendTextWithScroll("Xóa thành công danh sách commits lỗi.\n");
            btnReviewCommitsError.Enabled = false; // khóa nút kiểm tra sau khi xóa commit
            AppendTextWithScroll("Khóa nút kiểm tra commits lỗi sau khi xóa thành công danh sách commit lỗi.\n");
        }
        /// <summary>
        /// Kiểm tra sự tồn tại của thư mục tuần và file combined_commits.txt
        /// </summary>
        private bool CheckDirectoriesAndFiles(string weekFolder, string combinedFilePath, int week)
        {
            if (!Directory.Exists(weekFolder))
            {
                AppendTextWithScroll($"Thư mục {weekFolder} không tồn tại.\n");
                return false;
            }

            if (!File.Exists(combinedFilePath))
            {
                AppendTextWithScroll($"Tuần {week} không có commit nào.\n");
                return false;
            }

            return true;
        }
        private void OnMouseEnter(object sender, EventArgs e)
        {
            btnDeleteCommitsError.BackColor = Color.FromArgb(255, 128, 128);
            AppendEventsWithScroll("Xóa commits lỗi.\n");
        }
        private void OnMouseLeave(object sender, EventArgs e)
        {
            btnDeleteCommitsError.BackColor = Color.FromArgb(255, 192, 192); // Trở về màu nền mặc định 
        }

        private void BtnExpanDataGridview_MouseEnter(object sender, EventArgs e)
        {
            btnDeleteCommitsError.BackColor = Color.FromArgb(255, 128, 128);
            AppendEventsWithScroll("Mở rộng datagridview ở Form khác.\n");
        }

        private void BtnExpanDataGridview_MouseLeave(object sender, EventArgs e)
        {
            btnDeleteCommitsError.BackColor = Color.FromArgb(255, 192, 192); // Trở về màu nền mặc định 
        }

        private void BtnReviewCommits_MouseEnter(object sender, EventArgs e)
        {
            AppendEventsWithScroll("Kiểm tra commits lỗi.\n");
        }


        private void BtnExcelCommits_MouseEnter(object sender, EventArgs e)
        {
            AppendEventsWithScroll("Xuất Excel CommitET trong datagridview.\n");
        }

        /// <summary>
        /// xu ly giao dien chuc nang cap nhat duong dan
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SetupThuMucThucTap_Click(object sender, EventArgs e)
        {
            // Mở hộp thoại chọn thư mục
            using (var folderBrowserDialog = new FolderBrowserDialog())
            {
                folderBrowserDialog.Description = "Chọn thư mục thực tập";
                folderBrowserDialog.RootFolder = Environment.SpecialFolder.Desktop;

                DialogResult result = folderBrowserDialog.ShowDialog();
                if (result == DialogResult.OK)
                {
                    // Lấy đường dẫn thư mục
                    txtFolderInternshipPath = folderBrowserDialog.SelectedPath;
                    InternshipDirectoryET internshipDirectory = new InternshipDirectoryET
                    {
                        InternshipWeekFolder = txtFolderInternshipPath,
                        DateModified = DateTime.Now
                    };

                    // Lưu đường dẫn thư mục mới vào cơ sở dữ liệu
                    internshipDirectoryBUS.Add(internshipDirectory);

                    // Tải danh sách các thư mục thực tập từ cơ sở dữ liệu vào ComboBox
                    cboThuMucThucTap.DataSource = internshipDirectoryBUS.GetAll();
                    cboThuMucThucTap.ValueMember = "ID"; // Thiết lập trường sẽ làm giá trị
                    cboThuMucThucTap.DisplayMember = "InternshipWeekFolder"; // Thiết lập trường sẽ hiển thị trên combobox

                    // Lấy đường dẫn thư mục thực tập đã được chọn hoặc mặc định nếu không có
                    txtFolderInternshipPath = GetLatestInternshipFolderPath();

                    // Cập nhật đường dẫn thư mục thực tập trên giao diện
                    AppendTextWithScroll($"Đã cập nhật đường dẫn thư mục thực tập: {txtFolderInternshipPath}\n");

                }
            }
        }

        private void CboThuMucThucTap_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Lấy đối tượng ComboBox
            ComboBox comboBox = (ComboBox)sender;

            // Lấy thư mục thực tập được chọn
            InternshipDirectoryET selectedDirectory = (InternshipDirectoryET)comboBox.SelectedItem;

            if (selectedDirectory != null)
            {
                // Cập nhật đường dẫn thư mục thực tập trên giao diện
                txtFolderInternshipPath = selectedDirectory.InternshipWeekFolder;
                AppendTextWithScroll($"Đã chọn đường dẫn thư mục thực tập: {txtFolderInternshipPath}\n");
            }
        }

        private void SetupThuMucThucTap_MouseEnter(object sender, EventArgs e)
        {
            AppendEventsWithScroll("Chọn thư mục thực tập\n");
            txtSetupThuMucThucTap.BorderStyle = BorderStyle.FixedSingle;
            // them màu border khi hover
        }

        private void SetupThuMucThucTap_MouseLeave(object sender, EventArgs e)
        {
            txtSetupThuMucThucTap.BorderStyle = BorderStyle.None;
            txtResultMouseEvents.Clear();
        }
        private string RunGitCommand(string command, string workingDirectory)
        {
            var processStartInfo = new ProcessStartInfo
            {
                FileName = "git",
                Arguments = command,
                WorkingDirectory = workingDirectory,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                StandardOutputEncoding = Encoding.UTF8 // Đảm bảo đọc đầu ra với UTF-8
            };

            using var process = new Process
            {
                StartInfo = processStartInfo
            };
            process.Start();
            string output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            return output;
        }

        /// <summary>
        /// Phân tích đầu ra từ lệnh git log thành danh sách các commit với các trường thông tin cần thiết (hash, message, date, author)
        /// </summary>
        /// <param name="config"></param>
        public void AggregateCommits(ConfigFileET config)
        {
            var internshipWeekFolder = internshipDirectoryBUS.GetByID(config.InternshipDirectoryId).InternshipWeekFolder;
            try
            {
                // Xác định thư mục dự án Git và chuyển đến thư mục đó để chạy lệnh git
                string gitLogCommand = $"log --pretty=format:\"%H|%s|%ci|%an\"";
                string logOutput = RunGitCommand(gitLogCommand, config.ProjectDirectory); // Hàm chạy lệnh git
                var commits = ParseGitLog(logOutput);// Process the log output

                for (int weekOffset = 0; weekOffset < config.Weeks; weekOffset++)
                {
                    // Xác định thư mục cho tuần hiện tại
                    DateTime weekStartDate = config.InternshipStartDate.AddDays(weekOffset * 7);
                    DateTime weekEndDate = weekStartDate.AddDays(6);
                    int currentWeek = weekOffset + 1;
                    bool hasCommits = false;

                    // Tạo bản ghi cho tuần hiện tại
                    var projectWeek = new ProjectWeekET
                    {
                        InternshipDirectoryId = (int)config.InternshipDirectoryId,
                        WeekStartDate = weekStartDate,
                        WeekEndDate = weekEndDate,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now,
                    };
                    projectWeeksBUS.Add(projectWeek);// Thêm tuần vào db
                    var lastInsertedWeek = projectWeeksBUS.GetLastInserted(); // Lấy tuần vừa thêm

                    // Tạo nhóm commit cho tuần hiện tại
                    var commitGroup = new CommitGroupET
                    {
                        GroupName = $"Week {currentWeek}: {weekStartDate:dd/MM/yyyy} - {weekEndDate:dd/MM/yyyy}", // Group name chi tiết
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now
                    };
                    commitGroupsBUS.Add(commitGroup);// Thêm nhóm commit vào db
                    var lastInsertedGroup = commitGroupsBUS.GetLastInserted(); // Lấy nhóm vừa thêm


                    // Duyệt qua từng ngày trong tuần hiện tại
                    for (int dayOffset = 0; dayOffset < 7; dayOffset++)
                    {
                        DateTime currentDate = weekStartDate.AddDays(dayOffset);

                        // Lấy các commit trong ngày hiện tại
                        var dailyCommits = commits.Where(c => c.CommitDate.Date == currentDate.Date).ToList();

                        if (dailyCommits.Any())
                        {
                            foreach (var commit in dailyCommits)
                            {
                                // Xác định period từ giờ commit được thêm vào db cho từng commit trong ngày hiện tại
                                string period = DeterminePeriod(commit.CommitDate.Hour);

                                // Ghi log vào db cho từng commit trong ngày hiện tại
                                var commitInfo = new CommitET
                                {
                                    CommitHash = commit.CommitHash,
                                    CommitMessage = commit.CommitMessage,
                                    CommitDate = commit.CommitDate,
                                    Author = commit.Author,
                                    ProjectWeekId = lastInsertedWeek.ProjectWeekId,
                                    Date = currentDate, // Ngày hiện tại trong vòng lặp
                                    Period = period,    // Period được tính từ CommitDate
                                    CreatedAt = DateTime.Now,
                                    UpdatedAt = DateTime.Now
                                };
                                commitBUS.Add(commitInfo); // Thêm commit vào db
                                var lastInsertedCommit = commitBUS.GetLastInserted(); // Lấy commit vừa thêm

                                // Thêm commit vào nhóm commit của tuần hiện tại
                                var commitGroupMember = new CommitGroupMemberET
                                {
                                    CommitGroupId = lastInsertedGroup.CommitGroupId,
                                    CommitId = lastInsertedCommit.CommitId,
                                    AddedAt = DateTime.Now
                                };
                                commitGroupMembersBUS.Add(commitGroupMember); // Thêm commit vào nhóm
                            }
                            hasCommits = true;
                        }
                    }

                    if (!hasCommits)
                    {
                        // Xóa tuần nếu không có commit nào được thêm vào db cho tuần hiện tại
                        projectWeeksBUS.Delete(lastInsertedWeek.ProjectWeekId);
                    }
                    else
                    {
                        AppendTextWithScroll($"Week {currentWeek} commits đã tổng hợp vào db.\n");
                    }
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                AppendTextWithScroll($"Lỗi: Không có quyền truy cập vào đường dẫn '{internshipWeekFolder}'. Chi tiết: {ex.Message}\n");
            }
            catch (Exception ex)
            {
                AppendTextWithScroll($"Lỗi: Đã xảy ra lỗi khi tổng hợp commits. Chi tiết: {ex.Message}\n");
            }
        }

        // Helper method to determine the period of the day
        private string DeterminePeriod(int hour)
        {
            if (hour >= 6 && hour < 12)
            {
                return "S"; // Sáng: 6 giờ đến trước 12 giờ
            }
            else if (hour >= 12 && hour < 18)
            {
                return "C"; // Chiều: 12 giờ đến trước 18 giờ
            }
            else
            {
                return "T"; // Tối: 18 giờ đến trước 6 giờ sáng hôm sau
            }
        }

        // Helper method to parse git log output with encoding handling
        private List<CommitET> ParseGitLog(string logOutput)
        {
            var commits = new List<CommitET>();
            var lines = logOutput.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries); // Loại bỏ các dòng trống

            foreach (var line in lines)
            {
                var parts = line.Split('|');
                if (parts.Length == 4) // Đảm bảo có đủ 4 phần: CommitHash, CommitMessage, CommitDate, Author
                {
                    try
                    {
                        // Phân tích thời gian commit từ định dạng ISO 8601 kèm theo múi giờ
                        var commitDate = DateTime.ParseExact(
                            parts[2].Trim(), // Loại bỏ khoảng trắng thừa
                            "yyyy-MM-dd HH:mm:ss zzz", // Định dạng thời gian từ Git
                            CultureInfo.InvariantCulture // Sử dụng CultureInfo mặc định
                        );

                        // Thêm commit vào danh sách
                        commits.Add(new CommitET
                        {
                            CommitHash = parts[0].Trim(),
                            CommitMessage = parts[1].Trim(), // Không cần chuyển đổi encoding nếu đầu vào đã là UTF-8
                            CommitDate = commitDate,
                            Author = parts[3].Trim()
                        });
                    }
                    catch (FormatException ex)
                    {
                        Console.WriteLine($"Error parsing date: {ex.Message}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Unexpected error: {ex.Message}");
                    }
                }
            }

            return commits;
        }
        private void BtnSaveGit_Click(object sender, EventArgs e)
        {
            List<ConfigFileET> configs = configBus.GetAll();

            if (configs == null || configs.Count == 0)
            {
                AppendTextWithScroll("Không có cấu hình nào được tìm thấy.\n");
                return;
            }
            // Lưu thông tin cấu hình vào cơ sở dữ liệu
            foreach (ConfigFileET config in configs)
            {
                AggregateCommits(config);
            }
        }

        private void BtnSearchReport_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtSearchReport.Text == "")
                {
                    dgvReportCommits.DataSource = commitBUS.GetAll();
                    AppendTextWithScroll("Vui lòng nhập thông tin tìm kiếm.\n");
                    return;
                }
                Random d = new Random();
                commitBUS.Add(new CommitET()
                {
                    Author = "hoang van mach",
                    CommitDate = DateTime.Now,
                    CommitHash = d.Next(32343).ToString(),
                    CommitMessage = txtSearchReport.Text,
                    Date = DateTime.Now,
                    Period = DeterminePeriod(DateTime.Now.Hour),
                    ProjectWeekId = 35
                });
                dgvReportCommits.DataSource = commitBUS.GetAll();
                txtSearchReport.Clear();
            }
            catch (Exception ex)
            {
                txtSearchReport.Clear();
                AppendTextWithScroll($"Lỗi: {ex.Message}");
            }
        }
    }
}

