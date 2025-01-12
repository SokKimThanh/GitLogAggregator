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
        private bool isError = false;

        // Git log BUS
        private readonly GitlogBUS gitlogui_bus = new GitlogBUS();

        // Git commit BUS
        private readonly GitLogFormatBUS gitlogformat_bus = new GitLogFormatBUS();

        // git check commit
        private readonly GitLogCheckCommitBUS gitlogcheckcommit_bus = new GitLogCheckCommitBUS();

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
            btnOpenGitFolder.Enabled = true;
        }
        /// <summary>
        /// Hiển thị thông tin file config lên ListView với các cột: STT, Đường dẫn, Phân cấp và Mô tả.
        /// </summary>
        /// <param name="configFile">Đối tượng ConfigFile chứa thông tin cấu hình</param>
        /// <summary>
        /// Hiển thị thông tin file config lên ListView với các cột: STT, Đường dẫn, Phân cấp và Mô tả.
        /// </summary>
        /// <param name="configFile">Đối tượng ConfigFile chứa thông tin cấu hình</param>
        public void DisplayConfigInListView(ConfigFile configFile)
        {
            configListView.Items.Clear(); // Xóa dữ liệu cũ trong ListView

            int stt = 1;

            // Hiển thị thông tin đường dẫn dự án
            ListViewItem projectItem = new ListViewItem(stt.ToString()); // STT
            projectItem.SubItems.Add(configFile.ProjectDirectory);       // Đường dẫn dự án
            projectItem.SubItems.Add("Đường dẫn dự án");                 // Mô tả
            projectItem.SubItems.Add("1");                               // Phân cấp
            configListView.Items.Add(projectItem);
            stt++;

            // Hiển thị thông tin thư mục thực tập
            ListViewItem internshipItem = new ListViewItem(stt.ToString()); // STT
            internshipItem.SubItems.Add(configFile.InternshipWeekFolder);   // Thư mục thực tập
            internshipItem.SubItems.Add("Thư mục thực tập");                // Mô tả
            internshipItem.SubItems.Add("1");                               // Phân cấp
            configListView.Items.Add(internshipItem);
            stt++;

            // Hiển thị danh sách thư mục tuần, sắp xếp theo Phân cấp giảm dần
            var folderInfo = configFile.Folders
                .Select((path, index) => new
                {
                    STT = stt + index,                                    // Số thứ tự
                    Path = path,                                          // Đường dẫn
                    Level = path.Count(c => c == '\\' || c == '/'),       // Phân cấp
                    Description = $"Thư mục cấp {path.Count(c => c == '\\' || c == '/')}" // Mô tả
                })
                .OrderByDescending(info => info.Level)                   // Sắp xếp giảm dần theo Level
                .ToList();

            foreach (var info in folderInfo)
            {
                ListViewItem folderItem = new ListViewItem(info.STT.ToString()); // STT
                folderItem.SubItems.Add(info.Path);                              // Đường dẫn
                folderItem.SubItems.Add(info.Description);                       // Mô tả
                folderItem.SubItems.Add(info.Level.ToString());                  // Phân cấp (Level Path Folder)
                configListView.Items.Add(folderItem);
            }
        }



        /// <summary>
        /// Tải file config.txt và hiển thị thông tin lên ListView
        /// </summary>
        /// <param name="txtInternshipFolderPath">Đường dẫn thư mục thực tập</param>
        private void LoadConfigFileListView(string txtInternshipFolderPath = "")
        {
            string configPath = Path.Combine(txtInternshipFolderPath, "config.txt");
            if (File.Exists(configPath))
            {
                ConfigFile configFile = gitlogui_bus.LoadConfigFile(configPath);
                DisplayConfigInListView(configFile); // Gọi hàm hiển thị đã cập nhật
                AppendTextWithScroll("Đã tải dữ liệu từ file configFile.txt.\n");
            }
            else
            {
                AppendTextWithScroll("File configFile.txt không tồn tại.\n");
            }
        }



        /// <summary>
        /// Thêm một dòng vào ListView
        /// </summary>
        /// <param name="key">Tên thông tin</param>
        /// <param name="value">Giá trị thông tin</param>
        public void AddListViewItem(string key, string value)
        {
            ListViewItem item = new ListViewItem(key);
            item.SubItems.Add(value);
            configListView.Items.Add(item);
        }
        /// <summary>
        /// Người dùng chọn thư mục qua hộp thoại.
        /// Load danh sách tác giả commit từ Git bằng lệnh
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSelectGitFolder_Click(object sender, EventArgs e)
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

                string internshipWeekFolder = Path.Combine(projectDirectory, "internship_week");

                // Mở tên folder và tác giả
                txtDirectoryProjectPath = projectDirectory;
                cboAuthorCommit.DataSource = gitlogui_bus.LoadAuthorsCombobox(projectDirectory);

                // Lấy ngày commit đầu tiên và hiển thị lên giao diện
                DateTime firstCommitDate = gitlogui_bus.GetProjectStartDate(projectDirectory);
                txtFirstCommitDate.Value = firstCommitDate;

                // Thiết lập giới hạn ngày cho DateTimePicker
                SetMaxDateForDateTimePicker(txtInternshipStartDate, firstCommitDate);

                // Kiểm tra nếu thư mục internship_week tồn tại thì mở thêm thư mục (file config.txt nằm trong thư mục này)
                if (Directory.Exists(internshipWeekFolder))
                {
                    LoadAndDisplayConfigInfo(internshipWeekFolder);
                }
                else
                {
                    DisableControls();
                    cboAuthorCommit.Enabled = true;
                    txtInternshipStartDate.Enabled = true;
                    btnOpenGitFolder.Enabled = true;
                    btnAggregator.Enabled = true;
                    AppendTextWithScroll("Ngày thực tập của bạn bắt đầu khi nào?\n");
                }
            }
        }

        private void SetMaxDateForDateTimePicker(DateTimePicker dateTimePicker, DateTime maxDate)
        {
            dateTimePicker.MaxDate = maxDate;
        }

        private void InitializeFileListView()
        {
            fileListView.Columns.Clear();
            fileListView.View = View.Details;
            fileListView.FullRowSelect = true; // Đảm bảo chọn toàn bộ hàng
            fileListView.MultiSelect = false; // Đảm bảo chỉ chọn một hàng tại một thời điểm

            // Thiết lập các cột cho fileListView (nếu chưa thêm trước đó)
            if (fileListView.Columns.Count == 0)
            {
                fileListView.Columns.Add("STT", 50); // Cột số thứ tự
                fileListView.Columns.Add("Tên file", 120); // Cột tên file
                fileListView.Columns.Add("Ngày tạo", 100); // Cột ngày tạo file
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
            DateTime internshipEndDate = gitlogui_bus.CalculateEndDate(internshipStartDate, weeks);

            // Lấy tất cả commit
            var allCommits = gitlogui_bus.GetCommits(projectDirectory, cboAuthorCommit.SelectedItem.ToString(), internshipStartDate, internshipEndDate);

            // Chuyển đổi danh sách `DayData` thành `DataTable`
            DataTable dataTable = gitlogui_bus.ConvertDayDataListToDataTable(allCommits);

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
            string logOutput = gitlogui_bus.RunGitCommand(gitCommand, directory);

            // Kiểm tra nếu output rỗng
            return !string.IsNullOrEmpty(logOutput);
        }
        /// <summary>
        /// Load thông tin list view trống 
        /// </summary>
        /// <param name="internshipWeekFolder"></param>
        private void LoadAndDisplayConfigInfo(string internshipWeekFolder)
        {
            string configFile = Path.Combine(internshipWeekFolder, "config.txt");
            try
            {
                if (File.Exists(configFile))
                {
                    //
                    // Hiển thị dữ liệu config list view
                    //
                    ConfigFile configInfo = gitlogui_bus.LoadConfigFile(configFile);
                    DisplayConfigInListView(configInfo);
                    cboAuthorCommit.SelectedItem = configInfo.Author;
                    txtInternshipStartDate.Value = configInfo.StartDate;
                    txtInternshipEndDate.Value = configInfo.EndDate;
                    txtNumericsWeek.Value = configInfo.Weeks;
                    txtDirectoryProjectPath = configInfo.ProjectDirectory;
                    txtFolderInternshipPath = configInfo.InternshipWeekFolder;


                    DisplayFoldersInListView(configInfo.Folders);
                    AppendTextWithScroll($"Tải dữ liệu tổng hợp trước đó:\nTác giả: {configInfo.Author}\nNgày bắt đầu: {configInfo.StartDate:dd/MM/yyyy}\n");

                    // Load và hiển thị các commits từ các thư mục
                    //LoadCommitDatagridview();

                    //Đảm bảo rằng fileListView đã được cấu hình để hiển thị cột "STT" và tên file.
                    InitializeFileListView();

                    EnableControls();
                    btnDelete.Enabled = true;// open
                    btnExport.Enabled = true;  // open
                    txtInternshipEndDate.Enabled = false;
                    txtFirstCommitDate.Enabled = false;
                    txtNumericsWeek.Enabled = false;
                    btnCompleteReview.Enabled = false;
                }
                else
                {
                    DisableControls();
                    btnDelete.Enabled = false;
                }
            }
            catch
            {
                AppendTextWithScroll($"Lỗi: Không tìm thấy file {configFile}");
            }
        }
        private void EnableControls()
        {
            cboAuthorCommit.Enabled = true;
            txtNumericsWeek.Enabled = true;
            txtInternshipStartDate.Enabled = true;
            txtInternshipEndDate.Enabled = true;
            txtFirstCommitDate.Enabled = true;
            btnAggregator.Enabled = true;
            btnDelete.Enabled = true;
            btnOpenGitFolder.Enabled = true;
            btnExport.Enabled = true;
            btnReviewCommits.Enabled = true;
            btnCompleteReview.Enabled = true;
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
            btnDelete.Enabled = false;
            btnOpenGitFolder.Enabled = false;
            btnExport.Enabled = false;
            btnReviewCommits.Enabled = false;
            btnCompleteReview.Enabled = false;

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
            if (string.IsNullOrEmpty(projectDirectory))
            {
                AppendTextWithScroll("Vui lòng chọn thư mục dự án trước khi tổng hợp commit.\n");
                return;
            }

            if (isProcessing)
            {
                AppendTextWithScroll("Chương trình đang xử lý, vui lòng chờ...\n");
                return;
            }

            try
            {
                isProcessing = true;
                DisableControls();

                string internshipWeekFolder = Path.Combine(projectDirectory, "internship_week");
                if (Directory.Exists(internshipWeekFolder))
                {
                    var result = MessageBox.Show("Thư mục 'internship_week' đã tồn tại. Bạn có muốn ghi đè dữ liệu không?", "Xác nhận ghi đè", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == DialogResult.No)
                    {
                        AppendTextWithScroll("Quá trình tổng hợp đã bị hủy.\n");
                        isProcessing = false;
                        EnableControls();
                        return;
                    }
                }

                string author = cboAuthorCommit.SelectedItem.ToString();
                DateTime internshipStartDate = txtInternshipStartDate.Value;
                DateTime internshipEndDate = txtInternshipEndDate.Value;

                DateTime firstCommitDate = gitlogui_bus.GetProjectStartDate(projectDirectory);

                if (internshipStartDate > firstCommitDate)
                {
                    AppendTextWithScroll("Lỗi: Ngày thực tập phải diễn ra trước ngày commit đầu tiên.\n");
                    isProcessing = false;
                    isError = true;// đang có lỗi 
                    return;
                }

                //Dữ liệu được tổng hợp theo chức năng cũ gitlogui_bus.
                List<string> folders = gitlogui_bus.AggregateCommits(projectDirectory, author, internshipStartDate, internshipWeekFolder);
                ConfigFile aggregateInfo = new ConfigFile
                {
                    Author = author,
                    StartDate = internshipStartDate,
                    EndDate = internshipEndDate,
                    Folders = folders,
                    ProjectDirectory = projectDirectory,
                    InternshipWeekFolder = internshipWeekFolder,
                    FirstCommitDate = txtFirstCommitDate.Value,
                    Weeks = (int)txtNumericsWeek.Value
                };

                // Lưu thông tin vào file text
                gitlogui_bus.SaveConfigFile(aggregateInfo);
                AppendTextWithScroll("Đã lưu dữ liệu vào file config.txt.\n");

                // Hiển thị lên list view
                DisplayDirectoriesInListView();

                // load commit
                //LoadCommitDatagridview();

                // thông báo
                AppendTextWithScroll("Đã tổng hợp tất cả commit.\n");
            }
            catch (Exception ex)
            {
                AppendTextWithScroll($"Lỗi: {ex.Message}\n");
            }
            finally
            {
                AppendLogMessages(); // Hiển thị các thông báo
                isProcessing = false;
                // Tạm khóa dữ liệu cho phép chọn dự án khác hoặc mới.
                if (isError == true)
                {
                    isError = false;// reset flag 
                    DisableControls();
                    cboAuthorCommit.Enabled = true;
                    txtInternshipStartDate.Enabled = true;
                    btnOpenGitFolder.Enabled = true;
                    btnAggregator.Enabled = true;
                }
                else
                {
                    EnableControls();
                    txtInternshipEndDate.Enabled = false;
                    txtFirstCommitDate.Enabled = false;
                    txtNumericsWeek.Enabled = false;
                }
            }
        }
        /// <summary>
        /// Xóa thư mục: Nút xóa sẽ chỉ xóa những thư mục hoặc file không cần thiết
        /// mà không cần phải tạo lại những thư mục con như internship_week hoặc các thư mục tuần.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnDeleteFolderInternship_Click(object sender, EventArgs e)
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

                        LoadConfigFileListView(internshipWeekFolder);

                        btnDelete.Enabled = false;  // Vô hiệu hóa nút xóa
                        AppendTextWithScroll("Nút xóa đã bị vô hiệu hóa sau khi xóa thư mục.\n");

                        weekListView.Items.Clear();  // Xóa tất cả mục trong ListView
                        AppendTextWithScroll("Danh sách thư mục đã được làm trống.\n");

                        fileListView.Items.Clear();  // Xóa tất cả mục trong ListView
                        AppendTextWithScroll("Danh sách file đã được làm trống.\n");

                        checkedListBoxCommits.Items.Clear();  // Xóa tất cả mục trong checkedListBox1
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
                        btnDelete.Enabled = false;
                        btnExport.Enabled = false;
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
        private void DisplayFoldersInListView(List<string> directories)
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
        private void DisplayDirectoriesInListView()
        {
            if (string.IsNullOrEmpty(projectDirectory))
            {
                AppendTextWithScroll("Chưa chọn thư mục dự án.\n");
                return;
            }

            // Xác định đường dẫn thư mục internship_week
            string internshipWeekFolder = Path.Combine(txtDirectoryProjectPath, "internship_week");

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
            string[] directories = Directory.GetDirectories(internshipWeekFolder);

            // Xóa các mục hiện có trong ListView
            weekListView.Items.Clear();
            fileListView.Items.Clear();

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
            if (directories.Length == 0)
            {
                AppendTextWithScroll("Không có thư mục nào trong internship_week.\n");
            }
            else
            {
                AppendTextWithScroll($"Tổng số thư mục: {directories.Length}\n");
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
        private void AppendLogMessages()
        {
            foreach (var message in gitlogformat_bus.LogMessages)
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
            List<string> commits = gitlogui_bus.ReadCommitsFromFile(filePath);

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
                LoadCommitDatagridview();

                if (dataGridViewCommits.DataSource == null)
                {
                    AppendTextWithScroll("Không có dữ liệu để xuất.\n");
                    return;
                }
                string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                string filePath = Path.Combine(desktopPath, "commits.xlsx");

                DataTable dataTable = (DataTable)dataGridViewCommits.DataSource;
                List<DayData> dayDataList = gitlogformat_bus.ConvertDataTableToDayDataList(dataTable);

                // Chuyển đổi `List<DayData>` thành `List<WeekData>`
                List<WeekData> weekDataList = gitlogformat_bus.ConvertDayDataListToWeekDataList(dayDataList, txtInternshipStartDate.Value, txtInternshipEndDate.Value);

                gitlogformat_bus.CreateExcelFile(filePath, weekDataList, txtInternshipEndDate.Value);

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
            DateTime endDate = gitlogui_bus.CalculateEndDate(startDate, weeks);

            // Hiển thị ngày kết thúc
            txtInternshipEndDate.Value = endDate;
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

            AppendTextWithScroll("Hoàn thành tất cả các tuần!\n");
            btnReviewCommits.Enabled = true;
            btnCompleteReview.Enabled = true;
        }
        private void BtnCompleteReview_Click(object sender, EventArgs e)
        {
            if (invalidCommits.Count == 0)
            {
                AppendTextWithScroll("Vui lòng tổng hợp và kiểm tra commit lỗi trước khi xóa");
                return;
            }

            // Step 1: Confirm deletion with the user
            DialogResult result = MessageBox.Show("Are you sure you want to delete the invalid commits?", "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
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
            if (checkedListBoxCommits.DataSource != null)
            {
                // Assuming the DataSource is a List<string>
                var dataSource = checkedListBoxCommits.DataSource as List<string>;
                if (dataSource != null)
                {
                    dataSource.Clear(); // Clear the underlying data source
                }
                checkedListBoxCommits.DataSource = null; // Unbind the DataSource
            }

            // Optionally, rebind the CheckedListBox to an empty list
            checkedListBoxCommits.DataSource = new List<string>();

            // Inform the user that the process is complete
            AppendTextWithScroll("Invalid commits have been deleted.\n");
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
    }
}

