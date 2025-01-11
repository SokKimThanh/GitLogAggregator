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
        // Biến cờ để kiểm tra trạng thái chạy
        private bool isProcessing = false;
        private bool isError = false;

        // Git log BUS
        private readonly GitlogBUS gitlogui_bus = new GitlogBUS();

        // Git commit BUS
        private readonly GitLogFormatBUS gitlogformat_bus = new GitLogFormatBUS();

        // git check commit
        private readonly GitLogCheckCommitBUS gitlogcheckcommit_bus = new GitLogCheckCommitBUS();

        // Biến toàn cục để theo dõi trạng thái kiểm duyệt
        private bool userHasReviewed = false;

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
                txtDirectoryProjectPath.Text = projectDirectory;

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
                txtDirectoryProjectPath.Text = projectDirectory;
                cboAuthorCommit.DataSource = gitlogui_bus.LoadAuthorsCombobox(projectDirectory);

                // Lấy ngày commit đầu tiên và hiển thị lên giao diện
                DateTime firstCommitDate = gitlogui_bus.GetProjectStartDate(projectDirectory);
                txtFirstCommitDate.Value = firstCommitDate;

                // Thiết lập giới hạn ngày cho DateTimePicker
                SetMaxDateForDateTimePicker(txtInternshipStartDate, firstCommitDate);

                // Kiểm tra nếu thư mục internship_week tồn tại thì mở thêm thư mục (file config.txt nằm trong thư mục này)
                if (Directory.Exists(internshipWeekFolder))
                {
                    LoadAndDisplayAggregateInfo(internshipWeekFolder);
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

            // Thêm cột STT và tên file
            fileListView.Columns.Add("STT", 40, HorizontalAlignment.Right);
            fileListView.Columns.Add("Tên file", 300, HorizontalAlignment.Left);
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
        private void LoadAndDisplayAggregateInfo(string internshipWeekFolder)
        {
            string configFile = Path.Combine(internshipWeekFolder, "config.txt");
            try
            {
                if (File.Exists(configFile))
                {
                    AggregateInfo aggregateInfo = gitlogui_bus.LoadAggregateInfo(configFile);

                    cboAuthorCommit.SelectedItem = aggregateInfo.Author;
                    txtInternshipStartDate.Value = aggregateInfo.StartDate;
                    txtInternshipEndDate.Value = aggregateInfo.EndDate;
                    txtNumericsWeek.Value = aggregateInfo.Weeks;
                    txtDirectoryProjectPath.Text = aggregateInfo.ProjectDirectory;
                    txtFolderInternshipPath.Text = internshipWeekFolder;


                    DisplayFoldersInListView(aggregateInfo.Folders);
                    AppendTextWithScroll($"Tải dữ liệu tổng hợp trước đó:\nTác giả: {aggregateInfo.Author}\nNgày bắt đầu: {aggregateInfo.StartDate:dd/MM/yyyy}\n");

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
                AggregateInfo aggregateInfo = new AggregateInfo
                {
                    Author = author,
                    StartDate = internshipStartDate,
                    EndDate = txtInternshipEndDate.Value,
                    Folders = folders,
                    ProjectDirectory = projectDirectory,
                    FirstCommitDate = txtFirstCommitDate.Value,
                    Weeks = (int)txtNumericsWeek.Value
                };

                // Lưu thông tin vào file text
                gitlogui_bus.SaveAggregateInfo(aggregateInfo);

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

                        txtFolderInternshipPath.Text = string.Empty;
                        txtDirectoryProjectPath.Text = projectDirectory;

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
        /// <param name="folders"></param>
        private void DisplayFoldersInListView(List<string> folders)
        {
            weekListView.Items.Clear();
            fileListView.Items.Clear();
            imageList.ImageSize = new Size(32, 32);
            imageList.Images.Add("folder", Properties.Resources.Git_commit_aggregation_tool); // Icon folder

            weekListView.SmallImageList = imageList;

            foreach (var folder in folders)
            {
                // Lấy tên thư mục từ đường dẫn
                string folderName = Path.GetFileName(folder);

                var folderItem = new ListViewItem(folderName)
                {
                    ImageKey = "folder",
                    Tag = folder // Lưu đường dẫn đầy đủ của thư mục vào thuộc tính Tag của ListViewItem
                };
                weekListView.Items.Add(folderItem);
            }
        }
        /// <summary>
        /// Hiển thị danh sách thư mục thực tập kho tổng hợp commit
        /// </summary>
        private void DisplayDirectoriesInListView()
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

            // Khởi tạo ImageList
            imageList.ImageSize = new Size(32, 32);
            imageList.Images.Add("folder", Properties.Resources.Git_commit_aggregation_tool);
            weekListView.SmallImageList = imageList;

            // Lấy danh sách thư mục trong thư mục internship_week
            string[] directories = Directory.GetDirectories(internshipWeekFolder);

            // Thêm các thư mục vào ListView
            weekListView.Items.Clear();  // Xóa tất cả các mục hiện có
            fileListView.Items.Clear();  // Xóa tất cả các file hiện có
            int totalFiles = 0;
            int stt = 1;
            int emptyDirectories = 0;
            int directoriesWithCommits = 0;

            foreach (string folder in directories)
            {
                ListViewItem item = new ListViewItem(Path.GetFileName(folder))
                {
                    ImageKey = "folder",  // Sử dụng icon thư mục
                    Tag = folder // Lưu đường dẫn đầy đủ của thư mục vào thuộc tính Tag của ListViewItem
                };
                weekListView.Items.Add(item);

                // Lấy và thêm các file trong thư mục vào fileListView
                var files = Directory.GetFiles(folder);
                foreach (var file in files)
                {
                    string fileName = Path.GetFileName(file);
                    var fileItem = new ListViewItem(stt++.ToString());
                    fileItem.SubItems.Add(fileName);
                    fileItem.Tag = file; // Lưu đường dẫn đầy đủ của file vào thuộc tính Tag của ListViewItem
                    fileListView.Items.Add(fileItem);
                    totalFiles++;
                }

                // Đếm số lượng commit trong thư mục
                int commitsCount = CountCommitsInFolder(folder);
                if (commitsCount > 0)
                {
                    directoriesWithCommits++;
                    AppendTextWithScroll($"Thư mục: {folder} - Số lượng file: {files.Length} - Số lượng commit: {commitsCount}\n");
                }
                else
                {
                    emptyDirectories++;
                    AppendTextWithScroll($"Thư mục: {folder} - Không có commit.\n");
                }
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
        /// Hiển thị file khi chọn một thư mục
        /// </summary>
        /// <param name="directoryPath"></param>
        private void DisplayFilesInDirectory(string directoryPath)
        {
            if (Directory.Exists(directoryPath))
            {
                fileListView.Items.Clear(); // Xóa tất cả các mục hiện tại trong ListView
                string[] files = Directory.GetFiles(directoryPath);

                // biến đếm để tạo số thứ tự
                int index = 1;
                foreach (var file in files)
                {
                    ListViewItem item = new ListViewItem(index.ToString())
                    {
                        Tag = file // Lưu đường dẫn đầy đủ của file vào thuộc tính Tag của ListViewItem
                    };
                    item.SubItems.Add(Path.GetFileName(file));
                    fileListView.Items.Add(item);
                    index++;
                }
            }
            else
            {
                AppendTextWithScroll("Thư mục không tồn tại.\n");
            }
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
        /// xem danh sách commit trong thư mục
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
                    DisplayFilesInDirectory(directoryPath);

                    // Hiển thị thông báo "chọn thư mục tuần thứ"
                    AppendTextWithScroll($"Bạn đã chọn thư mục tuần: {folderName}\n");
                }
                else
                {
                    AppendTextWithScroll("Đường dẫn thư mục không tồn tại.\n");
                }
            }
        }


        private void FileListView_MouseClick(object sender, MouseEventArgs e)
        {
            if (fileListView.SelectedItems.Count > 0)
            {
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
        }

        private void BtnExportExcel_Click(object sender, EventArgs e)
        {
            try
            {

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
        /// Kiểm tra các commit lỗi, đồng thời xóa nó đi và cho phép preview trên datagridview trước khi xuất excel. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnReviewCommits_Click(object sender, EventArgs e)
        {
            btnReviewCommits.Enabled = false;
            txtResult.Clear(); // Xóa nội dung cũ trong RichTextBox
            InitializeDataGridView(dataGridViewCommits); // Thêm cột trước khi thêm hàng

            string internshipWeekFolder = Path.Combine(projectDirectory, "internship_week");

            if (!Directory.Exists(internshipWeekFolder))
            {
                AppendTextWithScroll("Thư mục internship_week không tồn tại.\n");
                btnReviewCommits.Enabled = true;
                return;
            }

            // Danh sách để lưu trữ tất cả các commit hợp lệ
            List<string> validCommits = new List<string>();

            for (int week = 1; week <= txtNumericsWeek.Value; week++)
            {
                AppendTextWithScroll($"Đang xử lý tuần {week}...\n"); // Hiển thị tuần hiện tại
                string weekFolder = Path.Combine(internshipWeekFolder, $"Week_{week}");
                if (!Directory.Exists(weekFolder))
                {
                    AppendTextWithScroll($"Thư mục {weekFolder} không tồn tại.\n");
                    continue;
                }

                string combinedFilePath = Path.Combine(weekFolder, "combined_commits.txt");
                if (!File.Exists(combinedFilePath))
                {
                    AppendTextWithScroll($"Tuần {week} không có commit nào.\n");
                    continue;
                }

                List<string> commits = gitlogcheckcommit_bus.ReadCommitsFromFile(combinedFilePath);

                if (commits.Count > 0)
                {
                    var groupedCommits = gitlogcheckcommit_bus.GroupErrorCommits(commits);

                    // Xóa ngay các commit lỗi mà không cần xác nhận
                    var commitsToDelete = groupedCommits["ErrorCommits"];
                    if (commitsToDelete.Count > 0)
                    {
                        ConfirmDeleteCommits(commitsToDelete, combinedFilePath, commits);

                        // Xóa các mục trong CheckedListBox sau khi xóa commit thành công
                        checkedListBoxCommits.Items.Clear();
                    }

                    // Thêm các commit hợp lệ vào danh sách validCommits
                    validCommits.AddRange(commits.Except(commitsToDelete));
                }
            }

            // Chuyển đổi danh sách validCommits thành danh sách CommitItem
            List<CommitItem> commitItems = new List<CommitItem>();
            foreach (var commit in validCommits)
            {
                var commitItem = gitlogcheckcommit_bus.ParseCommit(commit);
                commitItems.Add(commitItem);
            }

            // Hiển thị các commit hợp lệ lên DataGridView sau khi hoàn thành tất cả các tuần
            gitlogcheckcommit_bus.DisplayCommits(commitItems, dataGridViewCommits);

            AppendTextWithScroll("Hoàn thành tất cả các tuần!\n");
            MessageBox.Show("Đã hoàn thành kiểm duyệt và xóa commit cho tất cả các tuần.", "Hoàn thành", MessageBoxButtons.OK, MessageBoxIcon.Information);

            btnReviewCommits.Enabled = true;
        }






        /// <summary>
        /// Cài đặt mẫu dữ liệu để hiển thị
        /// </summary>
        public void InitializeDataGridView(DataGridView dataGridViewCommits)
        {
            DataTable dataTable = new DataTable();

            // Thêm các cột vào DataTable
            dataTable.Columns.Add("Tuần", typeof(string));
            dataTable.Columns.Add("Thứ", typeof(string));
            dataTable.Columns.Add("Buổi", typeof(string));
            dataTable.Columns.Add("Điểm danh vắng", typeof(string));
            dataTable.Columns.Add("Tên tệp", typeof(string));
            dataTable.Columns.Add("Nội dung commit", typeof(string));
            dataTable.Columns.Add("Ngày commit", typeof(DateTime));
            dataTable.Columns.Add("Nhận xét", typeof(string));
            dataTable.Columns.Add("Trạng thái", typeof(string));
            dataTable.Columns.Add("Ghi chú", typeof(string));

            // Gán DataTable cho DataGridView
            dataGridViewCommits.DataSource = dataTable;

            // Đặt chế độ tự động điều chỉnh cột để chiếm 100% không gian
            dataGridViewCommits.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Định dạng các cột
            foreach (DataGridViewColumn column in dataGridViewCommits.Columns)
            {
                column.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            }
            dataGridViewCommits.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
        }


        private void WaitUserReview()
        {
            // Tạm dừng cho đến khi người dùng hoàn thành việc kiểm tra các commit
            while (!userHasReviewed)
            {
                Application.DoEvents();
            }
            userHasReviewed = false; // Đặt lại để sẵn sàng cho lần kiểm duyệt tiếp theo
        }

        private void ConfirmDeleteCommits(List<string> commitsToDelete, string combinedFilePath, List<string> commits)
        {
            var result = MessageBox.Show("Bạn có chắc chắn muốn xóa các commit được chọn?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                foreach (var commit in commitsToDelete)
                {
                    commits.Remove(commit);
                }
                // xóa và cập nhật file
                gitlogcheckcommit_bus.UpdateLogFile(combinedFilePath, commits);

                AppendTextWithScroll($"Đã xóa các commit lỗi trong file {combinedFilePath}.\n");
            }
        }


        public void BtnCompleteReview_Click(object sender, EventArgs e)
        {
            // Kiểm tra tất cả các commit trong CheckedListBox
            for (int i = 0; i < checkedListBoxCommits.Items.Count; i++)
            {
                checkedListBoxCommits.SetItemChecked(i, true);
            }

            userHasReviewed = true;
        }
    }
}

