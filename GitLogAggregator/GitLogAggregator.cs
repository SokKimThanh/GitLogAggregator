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
        // Biến cờ để kiểm tra trạng thái chạy
        private bool isProcessing = false;

        // Git log BUS
        private readonly GitlogBUS gitgui_bus = new GitlogBUS();

        // Git commit BUS
        private readonly GitLogFormatBUS gitformat_bus = new GitLogFormatBUS();

        // git check commit
        private readonly GitLogCheckCommitBUS gitlogcheckcommit_bus = new GitLogCheckCommitBUS();

        // git load config file
        private readonly ConfigFileBUS gitconfig_bus = new ConfigFileBUS();


        // danh sách tổng hợp commit hợp lệ và không hợp lệ
        /// <summary>
        /// DS commit hợp lệ
        /// </summary>
        List<WeekData> weekDatas = new List<WeekData>();
        /// <summary>
        /// DS commit không hợp lệ
        /// </summary>
        List<string> invalidCommits = new List<string>();


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
            DisableControls();
            InitializeProjectListView(listViewProjects);
            LoadProjectListView();
            txtInternshipStartDate.Enabled = true;// open
        }

        /// <summary>
        /// Người dùng chọn thư mục qua hộp thoại.
        /// Load danh sách tác giả commit từ Git bằng lệnh
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <summary>
        /// Người dùng chọn thư mục qua hộp thoại.
        /// Load danh sách tác giả commit từ Git bằng lệnh
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnAddProject_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                projectDirectory = folderBrowserDialog1.SelectedPath;

                if (!IsValidGitRepository(projectDirectory))
                {
                    AppendTextWithScroll("Lỗi: Thư mục được chọn không chứa repository Git hợp lệ. Vui lòng chọn lại.\n");
                    return;
                }

                if (!HasCommitsInRepository(projectDirectory))
                {
                    AppendTextWithScroll("Lỗi: Repository Git này không chứa bất kỳ commit nào. Vui lòng chọn một repository khác hoặc tạo commit đầu tiên.\n");
                    return;
                }

                AppendTextWithScroll("File hướng dẫn đã có trong thư mục cài đặt tên là ManualUsage.docx.\n");

                // Kiểm tra xem dự án đã tồn tại trong cơ sở dữ liệu chưa
                if (gitconfig_bus.GetAllConfigFiles().Any(cf => cf.ProjectDirectory == projectDirectory))
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

                // Tạo đối tượng ConfigFileET và lưu vào cơ sở dữ liệu
                ConfigFileET configFile = new ConfigFileET
                {
                    ProjectDirectory = projectDirectory,
                    InternshipWeekFolder = Path.Combine(projectDirectory, "internship_week"),
                    Author = gitgui_bus.GetFirstCommitAuthor(projectDirectory),
                    StartDate = txtInternshipStartDate.Value,
                    EndDate = txtInternshipEndDate.Value,
                    Weeks = (int)txtNumericsWeek.Value,
                    FirstCommitDate = firstCommitDate,
                };

                // Cập nhật giao diện khi chọn thư mục dự án
                UpdateControls(configFile);

                // Thêm thông tin cấu hình dự án
                gitconfig_bus.AddConfigFile(configFile);

                // Load lại dữ liệu lên ListView
                LoadProjectListView();

                /// Sau mỗi lần chọn thêm thì tổng hợp thêm thư mục tuần và commit trong dự án
                List<string> directories = new List<string>();

                foreach (var cf in gitconfig_bus.GetAllConfigFiles())
                {
                    directories.Add(configFile.ProjectDirectory);
                }

                DisplayWeekAndCommitInListView(directories);


                AppendTextWithScroll("Dự án và thông tin cấu hình đã được thêm vào cơ sở dữ liệu thành công.\n");
            }
        }



        /// <summary>
        /// Cập nhật giao diện với thông tin cấu hình
        /// </summary>
        /// <param name="configInfo">Đối tượng ConfigFile chứa thông tin cấu hình</param>
        private void UpdateControls(ConfigFileET configInfo)
        {
            txtDirectoryProjectPath = configInfo.ProjectDirectory;
            txtFolderInternshipPath = configInfo.InternshipWeekFolder;

            // load danh sách tác giả
            cboAuthorCommit.DataSource = gitgui_bus.LoadAuthorsCombobox(configInfo.ProjectDirectory);
            // hiển thị tác giả commit đầu tiên
            cboAuthorCommit.SelectedItem = configInfo.Author;

            txtInternshipStartDate.Value = configInfo.StartDate;
            txtInternshipEndDate.Value = configInfo.EndDate;
            txtNumericsWeek.Value = configInfo.Weeks;

            txtFirstCommitDate.Value = configInfo.FirstCommitDate;
        }

        private void ListViewProjects_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (e.IsSelected)
            {
                ConfigFileET config = gitconfig_bus.GetConfigFileById(int.Parse(e.Item.Text));
                UpdateControls(config);
                string internshipWeekFolder = Path.Combine(config.ProjectDirectory, "internship_week");

                // Kiểm tra nếu thư mục internship_week tồn tại thì mở thêm thư mục (file config.txt nằm trong thư mục này)
                if (Directory.Exists(internshipWeekFolder))
                {
                    // Hiển thị dữ liệu thư mục và commit
                    LoadWeekAndCommitFileListView(config.ProjectDirectory);
                    // Hiển thị ngày thực tập tối đa có thể chọn.
                    SetMaxDateForDateTimePicker(txtInternshipStartDate, config.FirstCommitDate);
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


        private void LoadCommitDatagridview()
        {
            // Xóa dữ liệu cũ trên DataGridView trước khi thêm dữ liệu mới
            dataGridViewCommits.DataSource = null;

            // thông tin giao diện thời gian thực tập
            DateTime internshipStartDate = txtInternshipStartDate.Value;
            int weeks = (int)txtNumericsWeek.Value;
            DateTime internshipEndDate = gitgui_bus.CalculateEndDate(internshipStartDate, weeks);

            // Lấy tất cả commit
            var allCommits = gitgui_bus.GetCommits(projectDirectory, cboAuthorCommit.SelectedItem.ToString(), internshipStartDate, internshipEndDate);

            // Chuyển đổi danh sách `DayData` thành `DataTable`
            DataTable dataTable = gitgui_bus.ConvertDayDataListToDataTable(allCommits);

            // Hiển thị dữ liệu lên DataGridView
            dataGridViewCommits.DataSource = dataTable;

            // Đặt chế độ tự động điều chỉnh cột để chiếm 100% không gian
            dataGridViewCommits.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
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
        /// Load thông tin list view trống 
        /// </summary>
        /// <param name="internshipWeekFolder"></param>

        public void LoadProjectListView()
        {
            var configFiles = gitconfig_bus.GetAllConfigFiles();
            listViewProjects.Items.Clear(); // Xóa dữ liệu cũ trước khi bắt đầu thêm mới 
            foreach (var config in configFiles)
            {
                // Hiển thị thông tin đường dẫn dự án
                ListViewItem item = new ListViewItem(config.Id.ToString());         // ID
                item.SubItems.Add(config.ProjectDirectory);                         // Đường dẫn dự án
                item.SubItems.Add(config.Author);                                   // Tác giả thực hiện commit đầu tiên
                item.SubItems.Add(config.StartDate.ToString("yyyy-MM-dd"));         // Ngày bắt đầu thực tập
                item.SubItems.Add(config.EndDate.ToString("yyyy-MM-dd"));           // Ngày kết thúc thực tập
                item.SubItems.Add(config.Weeks.ToString());                         // Số tuần thực tập
                item.SubItems.Add(config.FirstCommitDate.ToString("yyyy-MM-dd"));   // Ngày commit đầu tiên
                item.SubItems.Add(config.InternshipWeekFolder);                     // Thư mục thực tập
                listViewProjects.Items.Add(item);
            }
        }

        /// <summary>
        /// Cập nhật trạng thái giao diện (các nút và control)
        /// </summary>
        private void UpdateControlState(bool isEnabled)
        {
            btnClearDataListView.Enabled = isEnabled;
            btnExcelCommits.Enabled = isEnabled;
            btnReviewCommits.Enabled = isEnabled;
            btnDeleteCommits.Enabled = !isEnabled;
            btnExpanDataGridview.Enabled = !isEnabled;
            txtInternshipEndDate.Enabled = !isEnabled;
            txtFirstCommitDate.Enabled = !isEnabled;
            txtNumericsWeek.Enabled = !isEnabled;
        }

        /// <summary>
        /// Xử lý trường hợp file config bị thiếu
        /// </summary>
        private void HandleMissingConfigFile(string configFile)
        {
            AppendTextWithScroll($"Lỗi: Không tìm thấy file {configFile}");
            DisableControls();
            btnClearDataListView.Enabled = true;
            btnOpenGitFolder.Enabled = true;
        }

        private void EnableControls()
        {
            cboAuthorCommit.Enabled = true;
            txtNumericsWeek.Enabled = true;
            txtInternshipStartDate.Enabled = true;
            txtInternshipEndDate.Enabled = true;
            txtFirstCommitDate.Enabled = true;
            btnAggregator.Enabled = true;
            btnClearDataListView.Enabled = true;
            btnOpenGitFolder.Enabled = true;
            btnExcelCommits.Enabled = true;
            btnReviewCommits.Enabled = true;
            btnDeleteCommits.Enabled = true;
            btnExpanDataGridview.Enabled = true;
            // Thêm các điều khiển khác nếu cần
        }
        private void DisableControls()
        {
            cboAuthorCommit.Enabled = false;
            txtNumericsWeek.Enabled = false;
            txtInternshipStartDate.Enabled = false;
            txtInternshipEndDate.Enabled = false;
            txtFirstCommitDate.Enabled = false;
            btnAggregator.Enabled = false;
            btnClearDataListView.Enabled = false;
            btnOpenGitFolder.Enabled = false;
            btnExcelCommits.Enabled = false;
            btnReviewCommits.Enabled = false;
            btnDeleteCommits.Enabled = false;
            btnExpanDataGridview.Enabled = false;
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
        private void BtnAggregate_Click(object sender, EventArgs e)
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
                // Tìm tất cả các Git repository từ ListView
                List<string> gitRepositories = gitgui_bus.FindGitRepositories(listViewProjects);

                if (gitRepositories.Count == 0)
                {
                    AppendTextWithScroll("Không tìm thấy Git repository hợp lệ trong danh sách dự án.\n");
                    isProcessing = false;
                    EnableControls();
                    return;
                }

                // Lấy thông tin từ UI
                string author = cboAuthorCommit.SelectedItem?.ToString();
                DateTime internshipStartDate = txtInternshipStartDate.Value;
                DateTime internshipEndDate = txtInternshipEndDate.Value;

                List<string> allFolders = new List<string>();

                foreach (string projectDirectory in gitRepositories)
                {
                    // Tạo thư mục tổng hợp riêng cho từng dự án
                    string commonInternshipWeekFolder = Path.Combine(
                        projectDirectory,
                        "internship_week"
                    );

                    // Kiểm tra và xác nhận ghi đè thư mục 'internship_week'
                    if (Directory.Exists(commonInternshipWeekFolder))
                    {
                        var result = MessageBox.Show(
                            $"Thư mục 'internship_week' đã tồn tại trong dự án {projectDirectory}. Bạn có muốn ghi đè dữ liệu không?",
                            "Xác nhận ghi đè",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Question
                        );

                        if (result == DialogResult.No)
                        {
                            AppendTextWithScroll($"Quá trình tổng hợp đã bị hủy cho dự án {projectDirectory}.\n");
                            continue;
                        }
                    }

                    AppendTextWithScroll($"Đang xử lý dự án: {projectDirectory}...\n");

                    List<string> folders = gitgui_bus.AggregateCommits(
                        new List<string> { projectDirectory },
                        author,
                        internshipStartDate,
                        commonInternshipWeekFolder
                    );

                    allFolders.AddRange(folders);

                    AppendTextWithScroll($"Đã tổng hợp commit cho dự án: {projectDirectory}.\n");
                }

                // Lưu thông tin cấu hình
                ConfigFileET configFile = new ConfigFileET
                {
                    Author = author,
                    StartDate = internshipStartDate,
                    EndDate = internshipEndDate,
                    Folders = allFolders,
                    ProjectDirectory = gitRepositories[0], // Lấy thư mục đầu tiên làm thư mục gốc
                    InternshipWeekFolder = allFolders[0], // Lấy thư mục đầu tiên của danh sách tổng hợp
                    FirstCommitDate = gitgui_bus.GetFirstCommitDate(gitRepositories[0]),
                    Weeks = (int)txtNumericsWeek.Value
                };

                AppendTextWithScroll("Đã lưu dữ liệu vào file config.txt.\n");

                // Cập nhật UI
                //LoadWeekAndCommitFileListView();
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


        /// <summary>
        /// Xóa thư mục: Nút xóa sẽ chỉ xóa những thư mục hoặc file không cần thiết
        /// mà không cần phải tạo lại những thư mục con như internship_week hoặc các thư mục tuần.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnClearDataListView_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(projectDirectory))
            {
                AppendTextWithScroll("Vui lòng chọn thư mục dự án trước khi xóa.\n");
                return;
            }

            string internshipWeekFolder = Path.Combine(projectDirectory, "internship_week");

            if (Directory.Exists(internshipWeekFolder))
            {
                var result = MessageBox.Show("Bạn có chắc chắn muốn xóa?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    try
                    {
                        DisableControls();
                        Directory.Delete(internshipWeekFolder, true);  // true để xóa tất cả các file và thư mục con
                        AppendTextWithScroll($"Đã xóa thư mục: {internshipWeekFolder}\n");

                        weekListView.Items.Clear();  // Xóa tất cả mục trong ListView
                        AppendTextWithScroll("Danh sách thư mục đã được làm trống.\n");

                        fileListView.Items.Clear();  // Xóa tất cả mục trong ListView
                        AppendTextWithScroll("Danh sách file đã được làm trống.\n");

                        listViewProjects.Items.Clear(); // xóa danh sách hiển thị đường dẫn
                        AppendTextWithScroll("Danh sách config đã được làm trống.\n");

                        checkedListBoxCommits.Items.Clear();  // Xóa tất cả mục trong checkedListBoxCommits
                        AppendTextWithScroll("Danh sách commit đã được làm trống.\n");

                        dataGridViewCommits.DataSource = null;
                        AppendTextWithScroll("Danh sách công việc đã được làm trống.\n");

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
                        btnDeleteCommits.Enabled = false;
                        btnReviewCommits.Enabled = false;
                        AppendTextWithScroll("Nút Kiểm tra đã bị vô hiệu hóa sau khi xóa thư mục.\n");
                        btnExcelCommits.Enabled = false;
                        txtInternshipEndDate.Enabled = false;
                        txtFirstCommitDate.Enabled = false;
                        txtNumericsWeek.Enabled = false;
                        txtFolderInternshipPath = string.Empty;
                        txtDirectoryProjectPath = string.Empty;
                    }
                }
            }
            else
            {
                AppendTextWithScroll("Thư mục internship_week không tồn tại.\n");
            }
        }

        /// <summary>
        /// Hiển thị thư mục khi load thư mục project
        /// </summary>
        /// <param name="directories"></param>
        private void DisplayWeekAndCommitInListView(List<string> directories)
        {
            weekListView.Items.Clear();
            fileListView.Items.Clear();
            imageList.ImageSize = new Size(32, 32);
            imageList.Images.Add("folder", Properties.Resources.Git_commit_aggregation_tool); // Icon folder

            weekListView.SmallImageList = imageList;

            int totalFiles = 0;
            int emptyDirectories = 0;
            int directoriesWithCommits = 0;

            // Danh sách file để tổng hợp từ tất cả các thư mục
            List<(string FilePath, DateTime CreationTime)> allFiles = new List<(string, DateTime)>();

            foreach (string folder in directories)
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

            // Hiển thị thông báo nếu không có thư mục nào trong internship_week
            if (directories.Count == 0)
            {
                AppendTextWithScroll("Không có thư mục nào trong internship_week.\n");
            }
            else
            {
                AppendTextWithScroll($"Tổng số thư mục: {directories.Count}\n");
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
        /// <summary>
        /// Hiển thị danh sách file từ tất cả các thư mục tuần trong fileListView
        /// </summary>
        private void LoadWeekAndCommitFileListView(string projectDirectory)
        {
            if (string.IsNullOrEmpty(projectDirectory))
            {
                AppendTextWithScroll("Chưa chọn thư mục dự án.\n");
                return;
            }

            // Xác định đường dẫn thư mục internship_week
            string internshipWeekFolder = Path.Combine(projectDirectory, "internship_week");

            // Kiểm tra xem thư mục internship_week có tồn tại không
            if (!Directory.Exists(internshipWeekFolder))
            {
                AppendTextWithScroll("Thư mục internship_week không tồn tại.\n");
                return;
            }
            // hiển thị thông tin folder dự án và thực tập
            txtDirectoryProjectPath = projectDirectory;
            txtFolderInternshipPath = internshipWeekFolder;

            // Khởi tạo ImageList cho weekListView
            imageList.ImageSize = new Size(32, 32);
            imageList.Images.Add("folder", Properties.Resources.Git_commit_aggregation_tool);
            weekListView.SmallImageList = imageList;

            // Lấy danh sách thư mục trong thư mục internship_week
            string[] weekFoldersPath = Directory.GetDirectories(internshipWeekFolder);

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

            // Hiển thị thông báo nếu không có thư mục nào trong internship_week
            if (weekFoldersPath.Length == 0)
            {
                AppendTextWithScroll("Không có thư mục nào trong internship_week.\n");
            }
            else
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
            string helpText = "Hướng Dẫn Sử Dụng Công Cụ Quản Lý Commit Git:\n\n" +
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
                              "3. Tổng Hợp Commit:\n" +
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
                    string directoryPath = selectedItem.Tag.ToString();
                    string folderName = Path.GetFileName(directoryPath); // Lấy tên thư mục
                    AppendTextWithScroll($"Bạn đã chọn thư mục tuần: {folderName}\n");

                    // Hiển thị và sắp xếp các file từ thư mục tuần được chọn
                    DisplayAndSortFilesFromSelectedWeek(directoryPath);
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
            checkedListBoxCommits.Items.Clear();
            foreach (var commit in commits)
            {
                checkedListBoxCommits.Items.Add(commit);
            }
        }

        private void BtnExportExcel_Click(object sender, EventArgs e)
        {
            try
            {
                //LoadCommitDatagridview();

                if (dataGridViewCommits.DataSource == null)
                {
                    AppendTextWithScroll("Không có dữ liệu để xuất.\n");
                    return;
                }
                string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                string filePath = Path.Combine(desktopPath, "commits.xlsx");

                DataTable dataTable = (DataTable)dataGridViewCommits.DataSource;
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
            btnReviewCommits.Enabled = false;
            txtResult.Clear();
            string internshipWeekFolder = Path.Combine(txtDirectoryProjectPath, "internship_week");

            if (!Directory.Exists(internshipWeekFolder))
            {
                AppendTextWithScroll("Thư mục internship_week không tồn tại.\n");
                btnReviewCommits.Enabled = true;
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

            checkedListBoxCommits.Items.Clear();
            foreach (var commit in invalidCommits)
            {
                checkedListBoxCommits.Items.Add(commit);
            }

            DataTable dataTable = gitlogcheckcommit_bus.ConvertToDataTable(weekDatas);
            dataGridViewCommits.DataSource = dataTable;

            int invalidCommitCount = invalidCommits.Count;
            if (invalidCommitCount == 0)
            {
                AppendTextWithScroll("Không có commit không hợp lệ.\n");
            }
            else
            {
                AppendTextWithScroll($"Kiểm tra thành công. Commit không hợp lệ: {invalidCommitCount}\n");
            }
            btnReviewCommits.Enabled = false;
            btnDeleteCommits.Enabled = false;
            btnExpanDataGridview.Enabled = true;
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
            dataGridViewCommits.DataSource = dataTable;

            // Clear the data source of the CheckedListBox
            checkedListBoxCommits.Items.Clear();

            // Inform the user that the process is complete
            btnDeleteCommits.Enabled = false;
            AppendTextWithScroll("Xóa thành công danh sách commits lỗi.\n");
            btnReviewCommits.Enabled = false; // khóa nút kiểm tra sau khi xóa commit
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
            btnDeleteCommits.BackColor = Color.FromArgb(255, 128, 128);
            AppendEventsWithScroll("Xóa commits lỗi.\n");
        }
        private void OnMouseLeave(object sender, EventArgs e)
        {
            btnDeleteCommits.BackColor = Color.FromArgb(255, 192, 192); // Trở về màu nền mặc định 
        }

        private void btnExpanDataGridview_MouseEnter(object sender, EventArgs e)
        {
            btnDeleteCommits.BackColor = Color.FromArgb(255, 128, 128);
            AppendEventsWithScroll("Mở rộng datagridview ở Form khác.\n");
        }

        private void btnExpanDataGridview_MouseLeave(object sender, EventArgs e)
        {
            btnDeleteCommits.BackColor = Color.FromArgb(255, 192, 192); // Trở về màu nền mặc định 
        }

        private void btnReviewCommits_MouseEnter(object sender, EventArgs e)
        {
            AppendEventsWithScroll("Kiểm tra commits lỗi.\n");
        }


        private void btnExcelCommits_MouseEnter(object sender, EventArgs e)
        {
            AppendEventsWithScroll("Xuất Excel Commit trong datagridview.\n");
        }

    }
}

