﻿using BUS;
using ET;
using GitLogAggregator.BusinessLogic;
using GitLogAggregator.GUI;
using GitLogAggregator.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GitLogAggregator
{
    public partial class ucMainForm : UserControl
    {// đường dẫn thư mục dự án
        private string configDirectory = string.Empty;
        private string txtFolderInternshipPath = string.Empty;

        // Phân trang
        private int currentPage = 1; // Trang hiện tại
        private const int pageSize = 20; // Số bản ghi mỗi trang
        private List<SearchResult> allSearchResults = []; // Lưu trữ tất cả kết quả tìm kiếm
        private PaginationHelper<SearchResult> paginationHelper;
        private DataGridViewHelper dataGridViewHelper;


        // Biến cờ để kiểm tra trạng thái chạy
        private bool isProcessing = false;

        // Git log BUS
        private readonly GitLogUIBUS gitgui_bus = new();

        // git load config file
        private readonly ConfigFileBUS configBus = new();

        private readonly InternshipDirectoryBUS internshipDirectoryBUS = new();

        // save week and commit to db
        private readonly ProjectWeekBUS projectWeeksBUS = new();

        // thong tin commit theo tuan
        private readonly CommitBUS commitBUS = new();

        private readonly RemoveBUS removeBUS = new();

        private readonly CommitPeriodBUS commitPeriodBUS = new();

        private readonly CommitSummaryBUS commitSummaryBUS = new();

        private readonly SummaryBUS summaryBUS = new();


        // FirstCommitAuthor BUS
        private readonly AuthorBUS authorBUS = new();


        private readonly SearchBUS searchBUS = new();

        public ucMainForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Vô hiệu hóa các nút và dropdown cho đến khi người dùng chọn thư mục chứa dự án Git.
        /// Tự động tải danh sách tác giả commit sau khi chọn thư mục.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_Load(object sender, EventArgs e)
        {
            try
            {
                // Tải danh sách các thư mục thực tập từ cơ sở dữ liệu vào ComboBox
                LoadConfigsIntoCombobox(0);

                // Tải danh sách tác giả 0: getall
                LoadAuthorsIntoComboBox(0);

                // Lấy đường dẫn thư mục thực tập đã được chọn hoặc mặc định nếu không có
                txtFolderInternshipPath = GetLatestInternshipFolderPath();
                // tải danh sách thư mục thực tập vào combobox
                cboInternshipFolder.DataSource = internshipDirectoryBUS.GetAll();
                cboInternshipFolder.ValueMember = "InternshipDirectoryId";
                cboInternshipFolder.DisplayMember = "InternshipWeekFolder";

                // Xây dựng danh sách các tuần và tệp
                BuildWeekFileListView();

                // Tự động điều chỉnh kích thước cột
                fileListView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);

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

                // Cập nhật danh sách tuần trên ComboBox
                cboSearchByWeek.DataSource = projectWeeksBUS.GetAll();
                cboSearchByWeek.ValueMember = "WeekId";
                cboSearchByWeek.DisplayMember = "WeekName";

                // Danh sách các nút crud và icon tương ứng
                var buttonsToConfigure = new Dictionary<Button, BtnIconCrudEnum>
                {
                    { btnSaveGit, BtnIconCrudEnum.AddIcon },
                    { btnRemoveAll, BtnIconCrudEnum.DeleteIcon },
                    { btnCreateWeek, BtnIconCrudEnum.AddIcon },
                    { btnSearchReport, BtnIconCrudEnum.SearchIcon } // Ví dụ thêm nút chỉnh sửa
                };

                // Cấu hình các nút icon crud
                ConfigureButtons(buttonsToConfigure);

                // biến tất cả combobox về dạng dropdownlist
                SetAllComboBoxesToDropDownList(this);

                // Hiển thị hint cho các control
                SetupHoverEventsForControls(txtResultMouseEvents);

                // Thêm các mục mặc định
                chkSearchCriteria.AddItem(SearchCriteria.chkEnablePagination, "Bật phân trang", true);
                chkSearchCriteria.AddItem(SearchCriteria.chkSearchAllWeeks, "Tìm kiếm tất cả tuần", true);
                chkSearchCriteria.AddItem(SearchCriteria.chkSearchAllAuthors, "Tìm kiếm tất cả tác giả", true);
                chkSearchCriteria.AddItem(SearchCriteria.chkIsSimpleView, "Hiển thị đơn giản", true);


                // Cấu hình DataGridView
                this.dataGridViewHelper = new DataGridViewHelper(dgvReportCommits);
                this.dataGridViewHelper.ConfigureDataGridView();

                // Load dữ liệu ban đầu
                SearchCommitsAndUpdateUI();


                DisableControls();
            }
            catch (Exception ex)
            {
                AppendTextWithScroll($"Lỗi: {ex.Message}");
            }

        }

        /// <summary>
        /// Hiển thị thông báo lên TextBox và cuộn xuống dòng cuối cùng
        /// </summary>
        /// <param name="configID"></param>
        private void LoadConfigsIntoCombobox(int configID)
        {
            // Lấy danh sách cấu hình từ BUS
            var configFiles = configBus.GetAll();

            // Thêm mục "Tất cả dự án" vào đầu danh sách
            configFiles.Insert(0, new ConfigET
            {
                ConfigID = 0,
                ConfigDirectory = "Tất cả dự án"
            });

            // Gán dữ liệu cho ComboBox
            cboConfigFiles.DataSource = configFiles;
            cboConfigFiles.ValueMember = "ConfigID";
            cboConfigFiles.DisplayMember = "ConfigDirectory";

            // Xử lý chọn mục theo configID
            if (configID == 0)
            {
                cboConfigFiles.SelectedValue = 0; // Chọn "Tất cả dự án"
            }
            else
            {
                // Kiểm tra configID có tồn tại trong danh sách không
                var exists = configFiles.Any(c => c.ConfigID == configID);

                // Nếu tồn tại thì chọn, không thì mặc định chọn "Tất cả"
                cboConfigFiles.SelectedValue = exists ? configID : 0;
            }
        }
        private void LoadAuthorsIntoComboBox(int authorId)
        {
            // Lấy danh sách tất cả tác giả
            List<AuthorET> authors = authorBUS.GetAll();

            // Thêm mục "Tất cả" vào đầu danh sách
            authors.Insert(0, new AuthorET { AuthorID = 0, AuthorName = "Tất cả" });

            // Gán DataSource chung cho cả hai ComboBox
            cboSearchByAuthor.DataSource = authors;
            cboAuthorCommit.DataSource = authors;

            // Cấu hình hiển thị
            cboSearchByAuthor.DisplayMember = "AuthorName";
            cboSearchByAuthor.ValueMember = "AuthorID";
            cboAuthorCommit.DisplayMember = "AuthorName";
            cboAuthorCommit.ValueMember = "AuthorID";

            // Xử lý chọn mục theo authorId
            if (authorId == 0)
            {
                // Chọn mục "Tất cả"
                cboSearchByAuthor.SelectedValue = 0;
                cboAuthorCommit.SelectedValue = 0;
            }
            else
            {
                // Kiểm tra xem authorId có tồn tại trong danh sách không
                bool exists = authors.Any(a => a.AuthorID == authorId);

                // Nếu tồn tại thì chọn, không thì giữ nguyên mặc định (hoặc chọn "Tất cả")
                if (exists)
                {
                    cboSearchByAuthor.SelectedValue = authorId;
                    cboAuthorCommit.SelectedValue = authorId;
                }
                else
                {
                    // Xử lý trường hợp không tìm thấy authorId
                    // Có thể chọn "Tất cả" hoặc không làm gì
                    cboSearchByAuthor.SelectedValue = 0;
                    cboAuthorCommit.SelectedValue = 0;
                }
            }
        }

        private void SetAllComboBoxesToDropDownList(Control parentControl)
        {
            // Duyệt qua tất cả các control trên form
            foreach (Control control in parentControl.Controls)
            {
                // Nếu control là ComboBox, thiết lập DropDownStyle thành DropDownList
                if (control is ComboBox comboBox)
                {
                    comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
                }

                // Nếu control có chứa các control con (ví dụ: Panel, GroupBox), đệ quy vào các control con
                if (control.HasChildren)
                {
                    SetAllComboBoxesToDropDownList(control);
                }
            }
        }
        private void ConfigureButtons(Dictionary<Button, BtnIconCrudEnum> buttonsToConfigure)
        {
            foreach (var item in buttonsToConfigure)
            {
                Button button = item.Key;
                BtnIconCrudEnum buttonImage = item.Value;

                // Lấy imageKey từ từ điển dựa trên giá trị BtnIconCrudEnum
                if (ButtonImageMap.CrudImageKeys.TryGetValue(buttonImage, out string imageKey))
                {
                    button.ImageKey = imageKey;
                }
                else
                {
                    throw new ArgumentException("Invalid button image value", nameof(buttonImage));
                }

                // Đặt các thuộc tính mặc định
                button.ImageAlign = ContentAlignment.MiddleLeft;
                button.TextAlign = ContentAlignment.MiddleCenter;
                button.TextImageRelation = TextImageRelation.ImageBeforeText;
            }
        }

        private void SetupMouseHoverEvents(Control control, string message, RichTextBox textBox)
        {
            // Sự kiện khi rê chuột vào control
            control.MouseEnter += (sender, e) =>
            {
                textBox.Clear();
                textBox.AppendText(message);
                textBox.ScrollToCaret();
            };

            // Sự kiện khi rê chuột ra khỏi control
            control.MouseLeave += (sender, e) =>
            {
                textBox.Clear();
            };
        }
        private void SetupHoverEventsForControls(RichTextBox textBox)
        {
            // Danh sách các control và thông báo tương ứng
            var controlsAndMessages = new Dictionary<Control, string>
            {
                { btnRemoveAll, "Xóa tất cả bảng trong csdl" },
                { btnSaveGit, "Lưu các commit git log dự án vào csdl" },
                { btnNextReport, "Tiếp theo commit kế" },
                { btnPreviousReport, "Lùi lại commit trước" },
                { btnSearchReport, "Tìm kiếm commit" },
                { btnCreateWeek, "Tạo danh mục tuần thực tập và nhóm commit thực tập theo tuần" },
                { btnOpenGitFolder, "Mở thư mục dự án git" },
                { btnSetupThuMucThucTap, "Thêm thư mục dự án vào csdl" },
                { btnRefreshData, "Làm mới dữ liệu hiển thị của các control" },
                { chkConfirmInternshipDate, "Bật/tắt chức năng sử dụng ngày tháng" },
                { txtInternshipStartDate, "Nhập ngày bắt đầu thực tập" },
                { cboInternshipFolder, "Chọn thư mục thực tập" },
                { helpButton, "Hiển thị hướng dẫn sử dụng" },
                { cboConfigFiles, "Hiển thị danh sách các dự án" },
                { dgvReportCommits, "Hiển thị báo cáo các commit" },
                { txtSearchReport, "Nhập từ khóa tìm kiếm commit" },
                { txtResultMouseEvents, "Hiển thị thông báo khi rê chuột vào các control" },
                { txtResult, "Hiển thị kết quả của các thao tác" },
                { txtFirstCommitDate, "Hiển thị ngày commit đầu tiên" },
                { btnExportTXT, "Xuất ra file combined_commits.txt cho các tuần thực tập" },
            };

            // Thiết lập sự kiện cho từng control
            foreach (var item in controlsAndMessages)
            {
                SetupMouseHoverEvents(item.Key, item.Value, textBox);
            }
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
            try
            {
                // Hiển thị hộp thoại chọn thư mục
                if (folderBrowserDialog.ShowDialog() != DialogResult.OK)
                {
                    btnOpenGitFolder.Enabled = true; // Bật lại nút mở thư mục
                    return;
                }

                configDirectory = folderBrowserDialog.SelectedPath;

                // Kiểm tra thư mục có phải là Git repository hợp lệ không
                if (!IsValidGitRepository(configDirectory))
                {
                    btnOpenGitFolder.Enabled = true; // Bật lại nút mở thư mục
                    AppendTextWithScroll("Lỗi: Thư mục được chọn không chứa repository Git hợp lệ. Vui lòng chọn lại.\n");
                    return;
                }

                // Kiểm tra repository có chứa commit nào không
                if (!HasCommitsInRepository(configDirectory))
                {
                    btnOpenGitFolder.Enabled = true; // Bật lại nút mở thư mục
                    AppendTextWithScroll("Lỗi: Repository Git này không chứa bất kỳ commit nào. Vui lòng chọn một repository khác hoặc tạo commit đầu tiên.\n");
                    return;
                }

                // Kiểm tra xem dự án đã tồn tại trong cơ sở dữ liệu chưa
                if (configBus.GetAll().Any(cf => cf.ConfigDirectory == configDirectory))
                {
                    AppendTextWithScroll("Lỗi: Dự án đã tồn tại trong cơ sở dữ liệu.\n");
                    btnOpenGitFolder.Enabled = true; // Bật lại nút mở thư mục
                    return;
                }

                // Lấy thông tin commit đầu tiên
                DateTime firstCommitDate = gitgui_bus.GetFirstCommitDate(configDirectory);

                // Kiểm tra ngày bắt đầu thực tập có hợp lệ không
                if (txtInternshipStartDate.Value >= firstCommitDate)
                {
                    AppendTextWithScroll("Lỗi: Ngày bắt đầu thực tập phải nhỏ hơn ngày commit đầu tiên.\n");
                    btnOpenGitFolder.Enabled = true; // Bật lại nút mở thư mục
                    return;
                }

                // Lấy danh sách tác giả và email từ lịch sử commit của repository
                var authors = gitgui_bus.GetAuthorsFromRepository(configDirectory);

                // Bước 1: Thêm tác giả vào database

                foreach (var (authorName, authorEmail) in authors)
                {
                    // Kiểm tra xem tác giả đã tồn tại trong database chưa dựa trên AuthorEmail
                    AuthorET author = authorBUS.GetByEmail(authorEmail);

                    if (author == null)
                    {
                        // Nếu chưa tồn tại, thêm tác giả mới vào database
                        author = new AuthorET
                        {
                            AuthorName = authorName,
                            AuthorEmail = authorEmail,
                            CreatedAt = DateTime.Now,
                            UpdatedAt = DateTime.Now
                        };

                        try
                        {
                            authorBUS.Add(author);
                        }
                        catch (Exception ex)
                        {
                            // Xử lý ngoại lệ nếu có lỗi khi thêm tác giả
                            AppendTextWithScroll($"Lỗi khi thêm tác giả {authorName} ({authorEmail}): {ex.Message}\n");
                            continue; // Bỏ qua tác giả này và tiếp tục vòng lặp
                        }
                    }
                }

                // Bước 2: Thêm dự án vào database
                ConfigET configFile = new()
                {
                    ConfigDirectory = configDirectory,
                    ConfigWeeks = (int)txtNumericsWeek.Value,
                    FirstCommitDate = firstCommitDate,
                    FirstCommitAuthor = gitgui_bus.GetFirstCommitAuthor(configDirectory),
                    InternshipDirectoryId = (int)cboInternshipFolder.SelectedValue,
                };
                configBus.Add(configFile);

                AppendTextWithScroll("Thêm dự án vào database hoàn tất.\n");

                // Cập nhật giao diện khi chọn thư mục dự án
                UpdateControlInternshipConfig(configFile);

                // Tải danh sách tác giả của dự án mới vào ComboBox
                LoadAuthorsIntoComboBox(0);

                // Load lại dữ liệu lên combobox
                LoadConfigsIntoCombobox(0);

                AppendTextWithScroll("Danh sách tác giả và cấu hình tải vào combobo thành công.\n");

                btnOpenGitFolder.Enabled = true; // Bật lại nút mở thư mục
            }
            catch (Exception ex)
            {
                AppendTextWithScroll($"Lỗi: {ex.Message}\n");
            }
        }



        /// <summary>
        /// Cập nhật giao diện với thông tin cấu hình
        /// </summary>
        /// <param name="configInfo">Đối tượng ConfigFile chứa thông tin cấu hình</param>
        private void UpdateControlInternshipConfig(ConfigET configInfo)
        {
            var internship = internshipDirectoryBUS.GetByID((int)cboInternshipFolder.SelectedValue);

            txtInternshipStartDate.Value = internship.InternshipStartDate;
            txtInternshipEndDate.Value = internship.InternshipEndDate;
            txtNumericsWeek.Value = configInfo.ConfigWeeks;
            txtFirstCommitDate.Value = configInfo.FirstCommitDate;
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
        /// Xử lý trường hợp file config bị thiếu
        /// </summary> 
        private void EnableControls()
        {

            cboInternshipFolder.Enabled = true;
            txtNumericsWeek.Enabled = true;
            txtInternshipStartDate.Enabled = true;
            txtInternshipEndDate.Enabled = true;
            txtFirstCommitDate.Enabled = true;
            // Thêm các điều khiển khác nếu cần
        }
        private void DisableControls()
        {
            cboInternshipFolder.Enabled = false;
            txtNumericsWeek.Enabled = false;
            txtInternshipStartDate.Enabled = false;
            txtInternshipEndDate.Enabled = false;
            txtFirstCommitDate.Enabled = false;
            btnCreateWeek.Enabled = false;
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
        private void BtnExportTXT_Click(object sender, EventArgs e)
        {
            try
            {
                if (configBus.GetAll().Count == 0)
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

                // Lấy thông tin từ UI
                string author = cboAuthorCommit.SelectedItem?.ToString();
                DateTime internshipStartDate = txtInternshipStartDate.Value;
                DateTime internshipEndDate = txtInternshipEndDate.Value;

                // Tổng hợp commit
                AggregateCommitsFileAndFolder();

                // Kiểm tra nếu thư mục internship_week tồn tại thì hiển thị dữ liệu lên listview
                if (Directory.Exists(txtFolderInternshipPath))
                {
                    // Hiển thị dữ liệu thư mục và commit
                    BuildWeekFileListView();
                }
            }
            catch (Exception ex)
            {
                AppendTextWithScroll($"Lỗi: {ex.Message}\n");
            }
            finally
            {
                isProcessing = false;
                EnableControls();
            }
        }

        private void DeleteEmptyFoldersAndFiles(string folderPath)
        {
            try
            {
                // Kiểm tra và xóa các file trống trong thư mục
                foreach (var file in Directory.GetFiles(folderPath))
                {
                    if (new FileInfo(file).Length == 0) // File trống nếu kích thước = 0
                    {
                        File.Delete(file);
                    }
                }

                // Kiểm tra và xóa các thư mục con trống
                foreach (var subFolder in Directory.GetDirectories(folderPath))
                {
                    DeleteEmptyFoldersAndFiles(subFolder); // Đệ quy để xóa thư mục con trống
                }

                // Xóa thư mục cha nếu nó trống
                if (!Directory.EnumerateFileSystemEntries(folderPath).Any())
                {
                    Directory.Delete(folderPath, true);
                }
            }
            catch (Exception ex)
            {
                // Xử lý ngoại lệ (ví dụ: ghi log hoặc hiển thị thông báo)
                AppendTextWithScroll($"Lỗi khi xóa thư mục/file trống: {ex.Message}\n");
            }
        }
        public void AggregateCommitsFileAndFolder()
        {
            var configFiles = configBus.GetAll();
            foreach (var config in configFiles)
            {
                if (config == null) continue;

                // Lấy thông tin thư mục từ database
                var internshipDir = internshipDirectoryBUS.GetByID(config.InternshipDirectoryId);
                if (internshipDir == null)
                {
                    AppendTextWithScroll($"Không tìm thấy thư mục thực tập cho ConfigID {config.ConfigID}");
                    continue;
                }

                string basePath = internshipDir.InternshipWeekFolder;
                string combinedFolder = Path.Combine(basePath, "Combined");

                try
                {
                    Directory.CreateDirectory(combinedFolder);

                    // Lấy danh sách tuần theo ConfigID
                    var weeks = projectWeeksBUS.GetAll();

                    foreach (var week in weeks)
                    {
                        string weekFolder = Path.Combine(basePath, $"{week.WeekName}");
                        string combinedFile = Path.Combine(combinedFolder, $"{week.WeekName}.txt");

                        // Lấy commit theo WeekID
                        var commits = commitBUS.GetByWeekId(week.WeekId)
                            .OrderBy(c => c.CommitDate)
                            .ToList();

                        if (!commits.Any()) continue;

                        // Tạo thư mục tuần
                        Directory.CreateDirectory(weekFolder);

                        // Nhóm commit theo ngày và period
                        var dailyGroups = commits
                            .GroupBy(c => new
                            {
                                c.CommitDate,
                                c.PeriodID
                            });

                        foreach (var group in dailyGroups)
                        {
                            var period = commitPeriodBUS.GetByID(group.Key.PeriodID);
                            if (period == null) continue;

                            string dayFolder = Path.Combine(weekFolder, group.Key.CommitDate.ToString("yyyy-MM-dd"));
                            Directory.CreateDirectory(dayFolder);

                            string periodFile = Path.Combine(dayFolder, $"{period.PeriodName}.txt");
                            var periodContent = string.Join("\n", group.Select(c =>
                                $"[{c.CommitHash}] {c.CommitMessages}\n" +
                                $"Tác giả: {GetAuthorName(c.AuthorID)}\n" +
                                $"Thời gian: {c.CommitDate:f}\n" +
                                new string('-', 50)));

                            File.WriteAllText(periodFile, periodContent);
                            File.AppendAllText(combinedFile, periodContent + "\n\n");
                        }

                        AppendTextWithScroll($"Đã tổng hợp {commits.Count} commit cho {week.WeekName}");
                    }
                }
                catch (Exception ex)
                {
                    AppendTextWithScroll($"Lỗi khi xử lý config {config.ConfigID}: {ex.Message}");
                }
                finally
                {
                    DeleteEmptyFolders(basePath);
                }
            }
        }

        // Các hàm hỗ trợ
        private string GetAuthorName(int authorId)
        {
            return authorBUS.GetByID(authorId)?.AuthorName ?? "Không xác định";
        }


        private void DeleteEmptyFolders(string path)
        {
            foreach (var directory in Directory.GetDirectories(path))
            {
                DeleteEmptyFolders(directory);
                if (!Directory.EnumerateFileSystemEntries(directory).Any())
                {
                    Directory.Delete(directory);
                }
            }
        }




        /// <summary>
        /// Làm mới dữ liệu: Tải lại dữ liệu từ nguồn và cập nhật các control trên giao diện.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnRefreshData_Click(object sender, EventArgs e)
        {
            try
            {
                DisableControls(); // Vô hiệu hóa các control trong quá trình làm mới

                // Làm mới combobox thư mục thực tập
                cboInternshipFolder.DataSource = internshipDirectoryBUS.GetAll();
                AppendTextWithScroll("Danh sách thư mục thực tập đã được làm mới.\n");

                // Làm mới dữ liệu trong combobox cboConfigFiles danh sách dự án thực tập
                LoadConfigsIntoCombobox(0);
                AppendTextWithScroll("Danh sách dự án thực tập đã được làm mới.\n");

                // Làm mới combobox tác giả commit
                LoadAuthorsIntoComboBox(0);
                AppendTextWithScroll("Danh sách tác giả đã được làm mới.\n");

                // Làm mới danh sách thư mục và file trong ListView
                BuildWeekFileListView();
                AppendTextWithScroll("Danh sách commit file text đã được làm mới.\n");

                // Làm mới dữ liệu trong DataGridView
                SearchCommitsAndUpdateUI();
                AppendTextWithScroll("Danh sách công việc đã được làm mới.\n");

                AppendTextWithScroll("Làm mới dữ liệu hoàn tất.\n");
            }
            catch (Exception ex)
            {
                AppendTextWithScroll($"Lỗi: {ex.Message}\n");
            }
            finally
            {
                //EnableControls(); // Kích hoạt lại các control sau khi hoàn tất                
                AppendTextWithScroll("Các control đã được kích hoạt lại.\n");
            }
        }


        /// <summary>
        /// Hiển thị danh sách file từ tất cả các thư mục tuần trong fileListView
        /// </summary>
        private void BuildWeekFileListView()
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
            List<(string FilePath, DateTime CreationTime)> allFiles = [];

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
                ListViewItem item = new(Path.GetFileName(folder))
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
            // Kiểm tra nếu text không chứa bất kỳ ký tự xuống dòng nào
            if (!text.Contains("\n"))
            {
                // Thêm ký tự xuống dòng vào cuối text
                text += "\n";
            }

            // Thêm text vào txtResult
            txtResult.AppendText(text);

            // Cuộn xuống dòng mới nhất
            txtResult.ScrollToCaret();
        }
        /// <summary>
        /// xem hướng dẫn thực hiện
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnHelpButton_Click(object sender, EventArgs e)
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
                if (selectedItem?.Tag != null)
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

                ListViewItem combinedFileItem = new(index++.ToString());
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

                ListViewItem fileItem = new(index++.ToString());
                fileItem.SubItems.Add(fileName); // Tên file
                fileItem.SubItems.Add(fileCreationTime); // Ngày tạo
                fileItem.Tag = file; // Lưu đường dẫn file
                fileListView.Items.Add(fileItem);
            }

            AppendTextWithScroll($"Tổng số file trong tuần: {files.Length}\n");
        }



        private void TxtNumericWeeks_ValueChanged(object sender, EventArgs e)
        {
            // Khi ngày thực tập thay đổi, kiểm tra lại tính hợp lệ của ngày
            if (!chkConfirmInternshipDate.Checked)
            {
                DateTime startDate = txtInternshipStartDate.Value;
                int weeks = (int)txtNumericsWeek.Value;
                DateTime endDate = gitgui_bus.CalculateEndDate(startDate, weeks);

                // Hiển thị ngày kết thúc
                txtInternshipEndDate.Value = endDate;
                btnOpenGitFolder.Enabled = true;// chọn tuần thực tập xong mới được thêm dự án
            }
            else
            {
                ValidateInternshipDate();
            }
        }


        /// <summary>
        /// xu ly giao dien chuc nang cap nhat duong dan
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSetupThuMucThucTap_Click(object sender, EventArgs e)
        {
            // Mở hộp thoại chọn thư mục
            using var folderBrowserDialog = new FolderBrowserDialog();
            folderBrowserDialog.Description = "Chọn thư mục thực tập";
            folderBrowserDialog.RootFolder = Environment.SpecialFolder.Desktop;
            InternshipDirectoryET internshipDirectory = new();
            DialogResult result = folderBrowserDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                // Lấy đường dẫn thư mục
                txtFolderInternshipPath = folderBrowserDialog.SelectedPath;

                // Kiểm tra nếu là thư mục git
                if (Directory.Exists(Path.Combine(txtFolderInternshipPath, ".git")))
                {
                    MessageBox.Show("Thư mục đã chọn là thư mục git. Vui lòng chọn thư mục khác.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Kiểm tra xem ngày thực tập đã được xác nhận chưa
                if (!chkConfirmInternshipDate.Checked)
                {
                    MessageBox.Show("Vui lòng xác nhận ngày thực tập trước khi tạo thư mục.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else
                {
                    internshipDirectory.InternshipWeekFolder = txtFolderInternshipPath;
                    internshipDirectory.InternshipStartDate = txtInternshipStartDate.Value;
                    internshipDirectory.InternshipEndDate = txtInternshipEndDate.Value;
                    internshipDirectory.CreatedAt = DateTime.Now;
                    internshipDirectory.UpdatedAt = DateTime.Now;
                }
                // Kiểm tra nếu là thư mục thông thường đã có dữ liệu
                if (Directory.EnumerateFileSystemEntries(txtFolderInternshipPath).Any())
                {
                    // Kiểm tra nếu là thư mục đã lưu trong cơ sở dữ liệu
                    var existingDirectory = internshipDirectoryBUS.GetByPath(txtFolderInternshipPath);
                    if (existingDirectory != null)
                    {
                        // Tải lại các thiết lập từ thư mục đã lưu
                        // Lấy đường dẫn thư mục thực tập đã được chọn hoặc mặc định nếu không có
                        txtFolderInternshipPath = existingDirectory.InternshipWeekFolder;
                    }
                    else
                    {
                        // Hiển thị thông báo yêu cầu làm trống thư mục
                        DialogResult resultDeleteFolderInternship = MessageBox.Show(
                            "Thư mục thực tập đang có dữ liệu. Bạn có muốn xóa trống thư mục này không?", // Nội dung thông báo
                            "Xác nhận xóa trống thư mục",                                           // Tiêu đề hộp thoại
                            MessageBoxButtons.OKCancel,                                        // Nút OK và Cancel
                            MessageBoxIcon.Warning                                             // Biểu tượng cảnh báo
                        );

                        // Kiểm tra kết quả từ hộp thoại
                        if (resultDeleteFolderInternship == DialogResult.OK)
                        {
                            try
                            {
                                // Xóa thư mục nếu người dùng chọn OK
                                Directory.Delete(txtFolderInternshipPath, true); // true để xóa cả thư mục con và file bên trong
                                AppendTextWithScroll($"Thư mục {txtFolderInternshipPath} đã được xóa thành công.\n");

                                // Tạo lại thư mục trống
                                Directory.CreateDirectory(txtFolderInternshipPath);


                                // Lưu đường dẫn thư mục mới vào cơ sở dữ liệu
                                internshipDirectoryBUS.Add(internshipDirectory);
                                AppendTextWithScroll($"Thư mục {txtFolderInternshipPath} đã được tạo lại.\n");
                            }
                            catch (Exception ex)
                            {
                                // Xử lý lỗi nếu có
                                AppendTextWithScroll($"Lỗi khi xóa thư mục: {ex.Message}\n");
                                return;
                            }
                        }
                        else
                        {
                            // Nếu người dùng chọn Cancel, không làm gì cả và thoát hàm
                            AppendTextWithScroll("Thư mục không được xóa. Vui lòng làm trống thư mục thực tập trước khi xử lý dữ liệu.\n");
                            return;
                        }
                    }
                }
                else
                {
                    // Lưu đường dẫn thư mục mới vào cơ sở dữ liệu
                    internshipDirectoryBUS.Add(internshipDirectory);
                }

                // Tải danh sách các thư mục thực tập từ cơ sở dữ liệu vào ComboBox
                cboInternshipFolder.DataSource = internshipDirectoryBUS.GetAll();

                // Lấy đường dẫn thư mục thực tập đã được chọn hoặc mặc định nếu không có
                txtFolderInternshipPath = GetLatestInternshipFolderPath();

                // Cập nhật đường dẫn thư mục thực tập trên giao diện
                AppendTextWithScroll($"Đã cập nhật đường dẫn thư mục thực tập: {txtFolderInternshipPath}\n");
            }
        }

        private void CboThuMucThucTap_SelectedIndexChanged(object sender, EventArgs e)
        {

        }


        public string RunGitCommand(string command, string workingDirectory)
        {
            var processStartInfo = new ProcessStartInfo
            {
                FileName = "git",
                Arguments = command,
                WorkingDirectory = workingDirectory,
                RedirectStandardOutput = true,
                RedirectStandardError = true, // Đọc cả lỗi từ quá trình
                UseShellExecute = false,
                CreateNoWindow = true,
                StandardOutputEncoding = Encoding.UTF8,
                StandardErrorEncoding = Encoding.UTF8
            };

            using var process = new Process
            {
                StartInfo = processStartInfo
            };

            StringBuilder outputBuilder = new();
            StringBuilder errorBuilder = new();

            process.OutputDataReceived += (sender, args) => outputBuilder.AppendLine(args.Data);
            process.ErrorDataReceived += (sender, args) => errorBuilder.AppendLine(args.Data);

            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
            process.WaitForExit();

            if (process.ExitCode != 0)
            {
                throw new Exception($"Git command failed with exit code {process.ExitCode}. Error: {errorBuilder}");
            }

            return outputBuilder.ToString();
        }

        // Hàm chạy lệnh Git thông qua file batch
        private string RunGitCommandViaBatch(string batchFilePath, string gitCommand, string workingDirectory)
        {
            var processStartInfo = new ProcessStartInfo
            {
                FileName = batchFilePath, // Đường dẫn đến file batch
                Arguments = gitCommand,   // Lệnh Git sẽ được truyền vào file batch
                WorkingDirectory = workingDirectory, // Thư mục làm việc
                RedirectStandardOutput = true,
                RedirectStandardError = true, // Đọc cả lỗi từ quá trình
                UseShellExecute = false,
                CreateNoWindow = true,
                StandardOutputEncoding = Encoding.UTF8,
                StandardErrorEncoding = Encoding.UTF8
            };

            using var process = new Process
            {
                StartInfo = processStartInfo
            };

            StringBuilder outputBuilder = new();
            StringBuilder errorBuilder = new();

            process.OutputDataReceived += (sender, args) => outputBuilder.AppendLine(args.Data);
            process.ErrorDataReceived += (sender, args) => errorBuilder.AppendLine(args.Data);

            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
            process.WaitForExit();

            if (process.ExitCode != 0)
            {
                throw new Exception($"Git command failed with exit code {process.ExitCode}. Error: {errorBuilder}");
            }

            return outputBuilder.ToString();
        }
        private void CreateBatchFile(string batchFilePath)
        {
            // Nội dung của file batch
            string batchContent = @"
@echo off
REM Đảm bảo rằng Git đã được cài đặt và có trong PATH
git %*
";

            // Ghi nội dung vào file batch
            File.WriteAllText(batchFilePath, batchContent);
        }
        /// <summary>
        /// Phân tích đầu ra từ lệnh git log thành danh sách các commit với các trường thông tin cần thiết (hash, message, date, author)
        /// </summary>
        /// <param name="config"></param>
        private void AggregateCommits(ConfigET config)
        {
            var internshipWeekFolder = internshipDirectoryBUS.GetByID(config.InternshipDirectoryId).InternshipWeekFolder;
            try
            {
                // Kiểm tra và tạo thư mục internshipWeekFolder nếu nó không tồn tại
                if (!Directory.Exists(internshipWeekFolder))
                {
                    Directory.CreateDirectory(internshipWeekFolder); // Tạo thư mục
                    AppendTextWithScroll($"Thư mục đã được tạo tại: {internshipWeekFolder}\n");
                }

                // Đường dẫn đến file batch
                string batchFilePath = Path.Combine(internshipWeekFolder, "run_git_command.bat");

                // Kiểm tra và tạo file batch nếu nó không tồn tại
                if (!File.Exists(batchFilePath))
                {
                    CreateBatchFile(batchFilePath); // Tạo file batch
                    AppendTextWithScroll($"File batch đã được tạo tại: {batchFilePath}\n");
                }

                // Kiểm tra thư mục dự án có tồn tại không
                if (!Directory.Exists(config.ConfigDirectory))
                {
                    throw new DirectoryNotFoundException($"Thư mục dự án không tồn tại: {config.ConfigDirectory}");
                }

                // Xác định thư mục dự án Git và chạy lệnh git thông qua file batch để lấy thông tin commit
                string gitLogCommand = $"log --reverse --pretty=format:\"%H|%s|%ci|%an|%ae\"";
                string logOutput = RunGitCommandViaBatch(batchFilePath, gitLogCommand, config.ConfigDirectory); // Hàm chạy lệnh git qua batch
                var commits = ParseGitLog(logOutput, config); // Process the log output

                // Lấy danh sách các tuần từ database
                List<WeekET> weeks = projectWeeksBUS.GetAll();

                // Danh sách tạm để lưu các commit đã thêm vào
                List<CommitET> insertedCommits = [];

                // Bước 1: Thêm tất cả các commit vào database
                foreach (var commit in commits)
                {
                    try
                    {
                        // Kiểm tra xem commit đã tồn tại chưa
                        var existingCommit = commitBUS.GetAll().FirstOrDefault(c => c.CommitHash == commit.CommitHash);

                        if (existingCommit == null)
                        {
                            // Tìm WeekId tương ứng với ngày của commit
                            var projectWeek = weeks.FirstOrDefault(w =>
                                commit.CommitDate >= w.WeekStartDate && commit.CommitDate <= w.WeekEndDate);

                            if (projectWeek == null)
                            {
                                AppendTextWithScroll($"Không tìm thấy tuần phù hợp cho commit {commit.CommitHash}.\n");
                                continue;
                            }
                            // Tạo thông tin commit để lưu vào DB
                            var commitInfo = new CommitET
                            {
                                CommitHash = commit.CommitHash,
                                CommitMessages = commit.CommitMessages,
                                CommitDate = commit.CommitDate,
                                ConfigID = commit.ConfigID,
                                AuthorID = commit.AuthorID,
                                WeekId = projectWeek.WeekId, // Gán WeekId chính xác
                                PeriodID = commit.PeriodID, // Gán PeriodID chính xác
                                CreatedAt = DateTime.Now,
                                UpdatedAt = DateTime.Now
                            };

                            // Thêm commit vào DB
                            commitBUS.Add(commitInfo);

                            // Lấy commit vừa thêm vào và lưu vào danh sách tạm
                            var lastInsertedCommit = commitBUS.GetLastInserted();
                            insertedCommits.Add(lastInsertedCommit);
                        }
                        else
                        {
                            AppendTextWithScroll($"Commit {commit.CommitHash} đã tồn tại.\n");
                        }
                    }
                    catch (Exception ex)
                    {
                        AppendTextWithScroll($"Lỗi khi thêm commit {commit.CommitHash}: {ex.Message}\n");
                    }
                }

                // Bước 2: Phân loại commit vào CommitGroupMembers dựa trên thời gian trong ngày (buổi)
                foreach (var commit in insertedCommits)
                {
                    try
                    {
                        // 1. Lấy thời gian trong ngày của commit (bỏ qua ngày/tháng/năm)
                        TimeSpan commitTime = commit.CommitDate.TimeOfDay;
                        // 2. Tìm period phù hợp dựa trên thời gian commit
                        var commitPeriod = commitPeriodBUS.GetAll()
                            .FirstOrDefault(p => commitTime >= p.PeriodStartTime
                                              && commitTime <= p.PeriodEndTime);

                        if (commitPeriod == null)
                        {
                            AppendTextWithScroll($"Không tìm thấy period phù hợp cho commit {commit.CommitHash}.\n");
                            continue;
                        }

                    }
                    catch (Exception ex)
                    {
                        AppendTextWithScroll($"Lỗi khi xử lý commit {commit.CommitHash}: {ex.Message}\n");
                    }
                }
                // -----------------------------
                // Bổ sung: Bước 3 & 4 - Lưu dữ liệu SummaryET và CommitSummaryET theo logic của SaveSummary
                // -----------------------------
                try
                {
                    // Lấy danh sách các CommitPeriodET từ database (sử dụng commitPeriodBUS đã có)
                    var allPeriods = commitPeriodBUS.GetAll();

                    // Lấy danh sách các ngày (Date) từ các commit đã thêm (insertedCommits)
                    var distinctDates = insertedCommits.Select(c => c.CommitDate.Date).Distinct();

                    // Với mỗi ngày và mỗi period, xử lý lưu summary nếu có commit thuộc phạm vi đó
                    foreach (var date in distinctDates)
                    {
                        foreach (var period in allPeriods)
                        {
                            // Sử dụng commitsBUS.GetByDateAndPeriod(date, period.PeriodStartTime, period.PeriodEndTime)
                            // để lấy danh sách commit thuộc ngày và buổi hiện hành.
                            var commitsForDatePeriod = commitBUS.GetByDateAndPeriod(date, period.PeriodStartTime, period.PeriodEndTime);

                            // Nếu có commit trong khoảng này, tiến hành lưu Summary
                            if (commitsForDatePeriod != null && commitsForDatePeriod.Count > 0)
                            {
                                // Xây dựng chuỗi contentResults dựa trên danh sách commit.
                                // Ví dụ: tổng hợp các commit messages hoặc thống kê số lượng commit theo từ khóa.
                                // Bạn có thể sử dụng hàm BuildContentResults nếu đã có, hoặc viết logic riêng.
                                string contentResults = BuildContentResults(commitsForDatePeriod);

                                // Gọi hàm SaveSummary theo logic đã định nghĩa:
                                // - Nếu Summary cho (date, period) chưa có, sẽ tạo mới và lưu.
                                // - Nếu đã tồn tại, sẽ cập nhật lại ContentResults.
                                // - Sau đó, liên kết các commit của (date, period) vào CommitSummaryET nếu chưa tồn tại.
                                SaveSummary(date, period, contentResults);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    AppendTextWithScroll($"Lỗi khi tạo Summary/CommitSummary: {ex.Message}\n");
                }


            }
            catch (FileNotFoundException ex)
            {
                AppendTextWithScroll($"Lỗi: File batch không tồn tại. Chi tiết: {ex.Message}\n");
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
            var commits = commitBUS.GetByDateAndPeriod(date, period.PeriodStartTime, period.PeriodEndTime);

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
        /// <summary>
        /// Xây dựng chuỗi ContentResults dựa trên danh sách CommitET.
        /// Ví dụ: "4 commits: 2 Fixes, 1 Feature, 1 Refactor"
        /// </summary>
        private string BuildContentResults(List<CommitET> commits)
        {
            var keywordCounts = new Dictionary<string, int>
            {
                { "fix", 0 },
                { "add", 0 },
                { "refactor", 0 }
            };

            foreach (var keyword in keywordCounts.Keys.ToList())
            {
                keywordCounts[keyword] = commits.Count(c => !string.IsNullOrEmpty(c.CommitMessages) &&
                                                            c.CommitMessages.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0);
            }

            var details = keywordCounts
                .Where(kvp => kvp.Value > 0)
                .Select(kvp => $"{kvp.Value} {kvp.Key}{(kvp.Value > 1 ? "es" : "")}")
                .ToList();

            string result = $"{commits.Count} commit{(commits.Count > 1 ? "s" : "")}";
            if (details.Any())
            {
                result += ": " + string.Join(", ", details);
            }
            return result;
        }
        // Helper method to parse git log output with encoding handling
        private List<CommitET> ParseGitLog(string logOutput, ConfigET config)
        {
            var commits = new List<CommitET>();

            var lines = logOutput.Split(['\n'], StringSplitOptions.RemoveEmptyEntries); // Loại bỏ các dòng trống
                                                                                        // Xác định period từ giờ commit
            foreach (var line in lines)
            {
                var parts = line.Split('|');
                if (parts.Length == 5) // Đảm bảo có đủ 5 phần: CommitHash, CommitMessages, CommitDate, FirstCommitAuthor, FirstCommitAuthor Email
                {
                    try
                    {
                        var commitDate = DateTime.ParseExact(
                            parts[2].Trim(), // Loại bỏ khoảng trắng thừa
                            "yyyy-MM-dd HH:mm:ss zzz", // Định dạng thời gian từ Git
                            CultureInfo.InvariantCulture // Sử dụng CultureInfo mặc định
                        );
                        var period = commitPeriodBUS.GetAll().FirstOrDefault(p => commitDate.TimeOfDay >= p.PeriodStartTime && commitDate.TimeOfDay <= p.PeriodEndTime);
                        var week = projectWeeksBUS.GetAll().FirstOrDefault(w => commitDate.Date >= w.WeekStartDate.Value.Date && commitDate.Date <= w.WeekEndDate.Value.Date);
                        if (week == null)
                        {
                            throw new InvalidOperationException("Week not found for the given commit date.");
                        }
                        // Thêm commit vào danh sách
                        commits.Add(new CommitET
                        {
                            CommitHash = parts[0].Trim(),
                            CommitMessages = parts[1].Trim(), // Không cần chuyển đổi encoding nếu đầu vào đã là UTF-8
                            CommitDate = commitDate,
                            ConfigID = config.ConfigID,
                            WeekId = week.WeekId,
                            AuthorID = authorBUS.GetByEmail(parts[4].Trim()).AuthorID,
                            PeriodID = period.PeriodID,
                        });
                    }

                    catch (FormatException ex)
                    {
                        AppendTextWithScroll($"Lỗi phân tích ngày tháng: {ex.Message}");
                    }
                    catch (InvalidOperationException ex)
                    {
                        AppendTextWithScroll($"Week not found error: {ex.Message}");
                    }
                    catch (Exception ex)
                    {
                        AppendTextWithScroll($"Lỗi không xác định: {ex.Message}");
                    }
                }
            }

            return commits;
        }
        private void BtnSaveGit_Click(object sender, EventArgs e)
        {
            try
            {
                // Kiểm tra xem danh sách tuần thực tập đã được tạo chưa
                if (!projectWeeksBUS.IsInternshipWeekListCreated())
                {
                    MessageBox.Show(
                        "Vui lòng tạo danh sách tuần thực tập trước khi lưu thông tin cấu hình.", // Nội dung thông báo
                        "Lỗi",                                                                     // Tiêu đề hộp thoại
                        MessageBoxButtons.OK,                                                      // Nút OK
                        MessageBoxIcon.Error                                                       // Biểu tượng lỗi
                    );
                    return; // Dừng hàm nếu danh sách tuần thực tập chưa được tạo
                }

                // Kiểm tra xem ngày thực tập đã được xác nhận chưa
                if (!chkConfirmInternshipDate.Checked)
                {
                    MessageBox.Show(
                        "Vui lòng xác nhận ngày thực tập trước khi lưu thông tin cấu hình.", // Nội dung thông báo
                        "Lỗi",                                                               // Tiêu đề hộp thoại
                        MessageBoxButtons.OK,                                                // Nút OK
                        MessageBoxIcon.Error                                                 // Biểu tượng lỗi
                    );
                    return; // Dừng hàm nếu ngày thực tập chưa được xác nhận
                }

                // Hiển thị hộp thoại xác nhận
                DialogResult result = MessageBox.Show(
                    "Bạn có chắc chắn muốn lưu thông tin cấu hình không?", // Nội dung thông báo
                    "Xác nhận lưu",                                        // Tiêu đề hộp thoại
                    MessageBoxButtons.YesNo,                               // Nút Yes và No
                    MessageBoxIcon.Question                                // Biểu tượng câu hỏi
                );

                // Kiểm tra kết quả từ hộp thoại
                if (result == DialogResult.Yes)
                {
                    // Nếu người dùng chọn "Yes", thực hiện lưu
                    List<ConfigET> configs = configBus.GetAll();

                    if (configs == null || configs.Count == 0)
                    {
                        AppendTextWithScroll("Không có cấu hình nào được tìm thấy.\n");
                        return;
                    }

                    // Lưu thông tin cấu hình vào cơ sở dữ liệu
                    foreach (ConfigET config in configs)
                    {
                        AggregateCommits(config); // Hàm này xử lý việc lưu thông tin commit vào cơ sở dữ liệu
                    }

                    // Cập nhật giao diện người dùng (UI)
                    UpdateUIAfterSave();

                    // Hiển thị thông báo thành công
                    AppendTextWithScroll("Lưu thông tin cấu hình thành công.\n");
                }
                else
                {
                    // Nếu người dùng chọn "No", hiển thị thông báo hủy
                    AppendTextWithScroll("Đã hủy lưu thông tin cấu hình.\n");
                }
            }
            catch (Exception ex)
            {
                // Xử lý ngoại lệ và hiển thị thông báo lỗi
                AppendTextWithScroll($"Lỗi: {ex.Message}\n");
            }
        }



        private void UpdateUIAfterSave()
        {
            // Cập nhật DataGridView với dữ liệu mới từ cơ sở dữ liệu
            dgvReportCommits.DataSource = commitBUS.GetAll();

            // Cập nhật trạng thái của CheckBox và ComboBox
            chkConfirmInternshipDate.Checked = false; // Đặt lại trạng thái của CheckBox
            cboInternshipFolder.SelectedIndex = -1;  // Đặt lại ComboBox về trạng thái không chọn

            // Nếu CheckBox được chọn, thực hiện tìm kiếm tất cả các tuần
            if (chkSearchCriteria.GetItemChecked(0))
            {
                // Logic tìm kiếm commit và cập nhật UI
                SearchCommitsAndUpdateUI();
                AppendTextWithScroll("Đã tìm kiếm và cập nhật dữ liệu từ tất cả các tuần.\n");
            }
        }
        private void SearchCommitsAndUpdateUI()
        {
            try
            {
                // Kiểm tra ComboBox đã được bind dữ liệu chưa
                if (cboSearchByWeek.DataSource == null || cboAuthorCommit.DataSource == null)
                {
                    AppendTextWithScroll("Vui lòng tải dữ liệu tuần/tác giả trước!\n");
                    return;
                }

                // Lấy các tiêu chí từ CheckedListBox
                bool enablePagination = CheckedListBoxHelper.IsItemChecked(chkSearchCriteria, SearchCriteria.chkEnablePagination);
                bool searchAllWeeks = CheckedListBoxHelper.IsItemChecked(chkSearchCriteria, SearchCriteria.chkSearchAllWeeks);
                bool searchAllAuthors = CheckedListBoxHelper.IsItemChecked(chkSearchCriteria, SearchCriteria.chkSearchAllAuthors);
                bool isSimpleView = CheckedListBoxHelper.IsItemChecked(chkSearchCriteria, SearchCriteria.chkIsSimpleView);

                // Lấy giá trị từ ComboBox (nếu không chọn "Tất cả")
                int? selectedWeekId = searchAllWeeks ? null : (int?)cboSearchByWeek.SelectedValue;
                int? selectedAuthorId = searchAllAuthors ? null : (int?)cboAuthorCommit.SelectedValue;

                // Gọi phương thức tìm kiếm từ BUS
                allSearchResults = searchBUS.SearchCommits(
                    keyword: txtSearchReport.Text,
                    projectWeekId: selectedWeekId,
                    searchAllWeeks: searchAllWeeks,
                    searchAllAuthors: searchAllAuthors,
                    authorId: selectedAuthorId
                );

                // Hiển thị kết quả
                currentPage = 1;
                DisplaySearchResults(enablePagination, isSimpleView);
            }
            catch (Exception ex)
            {
                AppendTextWithScroll($"Lỗi: {ex.Message}\n");
            }
        }

        private void BtnSearchReport_Click(object sender, EventArgs e)
        {
            SearchCommitsAndUpdateUI();
        }

        private void DisplaySearchResults(bool enablePagination, bool isSimpleView)
        {
            try
            {
                if (allSearchResults != null && allSearchResults.Any())
                {
                    if (enablePagination)
                    {
                        // Khởi tạo lại paginationHelper với dữ liệu mới
                        paginationHelper = new PaginationHelper<SearchResult>(allSearchResults, pageSize);
                        paginationHelper.SetCurrentPage(currentPage); // Đặt trang hiện tại

                        // Lấy dữ liệu cho trang hiện tại
                        var pagedResults = paginationHelper.GetCurrentPageData();
                        dgvReportCommits.DataSource = pagedResults;

                        // Hiển thị thông tin phân trang
                        AppendTextWithScroll($"Trang {paginationHelper.GetCurrentPage()}/{paginationHelper.GetTotalPages()} | Hiển thị {pagedResults.Count} kết quả.\n");
                        UpdatePaginationControls(paginationHelper.GetTotalPages());
                    }
                    else
                    {
                        // Hiển thị tất cả kết quả
                        dgvReportCommits.DataSource = allSearchResults;
                        AppendTextWithScroll($"Hiển thị tất cả {allSearchResults.Count} kết quả.\n");
                        btnPreviousReport.Enabled = false;
                        btnNextReport.Enabled = false;
                    }

                    // Cấu hình hiển thị đơn giản hoặc đầy đủ
                    if (isSimpleView)
                    {
                        dataGridViewHelper.ConfigureSimpleView();
                    }
                    else
                    {
                        dataGridViewHelper.ConfigureFullView();
                    }
                }
                else
                {
                    dgvReportCommits.DataSource = null;
                    btnPreviousReport.Enabled = false;
                    btnNextReport.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                AppendTextWithScroll($"Lỗi khi hiển thị kết quả: {ex.Message}\n");
            }
        }

        private void UpdatePaginationControls(int totalPages)
        {
            btnPreviousReport.Enabled = paginationHelper.GetCurrentPage() > 1;
            btnNextReport.Enabled = paginationHelper.GetCurrentPage() < totalPages;
        }

        // Trong sự kiện nút phân trang
        private void BtnNextReport_Click(object sender, EventArgs e)
        {
            if (paginationHelper != null)
            {
                paginationHelper.NextPage();
                currentPage = paginationHelper.GetCurrentPage(); // Cập nhật currentPage
                bool chkSimpleView = chkSearchCriteria.IsItemChecked(SearchCriteria.chkIsSimpleView);
                DisplaySearchResults(true, chkSimpleView);
            }
        }

        private void BtnPreviousReport_Click(object sender, EventArgs e)
        {
            if (paginationHelper != null)
            {
                paginationHelper.PreviousPage();
                currentPage = paginationHelper.GetCurrentPage(); // Cập nhật currentPage
                bool chkSimpleView = chkSearchCriteria.IsItemChecked(SearchCriteria.chkIsSimpleView);
                DisplaySearchResults(true, chkSimpleView);
            }
        }
        // Trong phương thức EnablePagination
        private void EnablePagination(bool enable)
        {
            btnPreviousReport.Visible = enable;
            btnNextReport.Visible = enable;

            if (!enable)
            {
                paginationHelper = null; // Reset helper khi tắt phân trang
                bool chkSimpleView = chkSearchCriteria.IsItemChecked(SearchCriteria.chkIsSimpleView);
                DisplaySearchResults(false, chkSimpleView);
            }
        }
        private void BtnRemoveAll_Click(object sender, EventArgs e)
        {
            // Hiển thị hộp thoại xác nhận
            DialogResult result = MessageBox.Show(
                "Bạn có chắc chắn muốn xóa tất cả dữ liệu không?", // Nội dung thông báo
                "Xác nhận xóa",                                    // Tiêu đề hộp thoại
                MessageBoxButtons.YesNo,                           // Nút Yes và No
                MessageBoxIcon.Warning                             // Biểu tượng cảnh báo
            );

            // Kiểm tra kết quả từ hộp thoại
            if (result == DialogResult.Yes)
            {
                try
                {
                    // Kiểm tra xem CheckBox "Xóa tất cả dự án" có được chọn hay không
                    if (chkDeleteAllProject.Checked)
                    {
                        // Nếu được chọn, xóa tất cả dự án
                        removeBUS.ClearAllProjects();
                        AppendTextWithScroll("Tất cả dự án đã được xóa.\n");
                    }
                    else
                    {
                        // Nếu không được chọn, xóa toàn bộ dữ liệu
                        removeBUS.ClearAllTables();
                        AppendTextWithScroll("Tất cả dữ liệu đã được xóa.\n");
                    }

                    // Làm mới giao diện sau khi xóa
                    BtnRefreshData_Click(sender, e); // Gọi hàm làm mới để cập nhật giao diện
                }
                catch (Exception ex)
                {
                    AppendTextWithScroll($"Lỗi khi xóa dữ liệu: {ex.Message}\n");
                }
            }
            // Nếu người dùng chọn "No", không làm gì cả
        }
        /// <summary>
        /// kiểm tra ngày commit đầu tiên của các project xem nó có nằm trong tuần thực tập thì mới được tạo tuần và commit group
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnCreateWeekAndPeriod_Click(object sender, EventArgs e)
        {
            // Kiểm tra đầu vào
            if (cboInternshipFolder.SelectedValue == null)
            {
                AppendTextWithScroll("[Lỗi] Vui lòng chọn thư mục thực tập trước khi tạo tuần thực tập.\n");
                return;
            }

            int internshipDirectoryId = int.Parse(cboInternshipFolder.SelectedValue.ToString());
            DateTime internshipStartDate = txtInternshipStartDate.Value.Date;
            DateTime internshipEndDate = txtInternshipEndDate.Value.Date;

            int weeks = (int)txtNumericsWeek.Value;
            if (weeks <= 0)
            {
                AppendTextWithScroll("[Lỗi] Số tuần phải lớn hơn 0.\n");
                return;
            }

            var configs = configBus.GetAll();
            if (configs.Count == 0)
            {
                AppendTextWithScroll("[Lỗi] Vui lòng thêm dự án trước khi tạo tuần thực tập.\n");
                return;
            }

            // Kiểm tra ngày commit đầu tiên của các project
            bool isWithinInternship = configs.Any(c =>
                c.FirstCommitDate.Date >= internshipStartDate &&
                c.FirstCommitDate.Date <= internshipEndDate
            );
            if (!isWithinInternship)
            {
                AppendTextWithScroll("[Lỗi] Không thể tạo tuần thực tập vì dự án không nằm trong kỳ thực tập.\n");
                return;
            }

            // Kiểm tra ProjectWeek đã tồn tại
            var existingProjectWeeks = projectWeeksBUS.GetAll()
                .Where(w => w.WeekStartDate >= internshipStartDate && w.WeekEndDate <= internshipEndDate)
                .ToList();

            if (existingProjectWeeks.Count == 0)
            {
                // Tạo ProjectWeek
                for (int weekOffset = 0; weekOffset < weeks; weekOffset++)
                {
                    DateTime weekStartDate = internshipStartDate.AddDays(weekOffset * 7);
                    DateTime weekEndDate = weekStartDate.AddDays(6);
                    string WeekName = $"Tuần {weekOffset + 1}";

                    var projectWeek = new WeekET
                    {
                        WeekName = WeekName,
                        WeekStartDate = weekStartDate.Date,
                        WeekEndDate = weekEndDate.Date,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now,
                    };
                    projectWeeksBUS.Add(projectWeek);
                }

                // Kiểm tra dữ liệu đã được thêm thành công
                var newProjectWeeks = projectWeeksBUS.GetAll()
                    .Where(w => w.WeekStartDate >= internshipStartDate && w.WeekEndDate <= internshipEndDate)
                    .ToList();

                if (newProjectWeeks.Count == weeks)
                {
                    AppendTextWithScroll("[Thành công] Đã tạo và lưu ProjectWeek vào database.\n");
                }
                else
                {
                    AppendTextWithScroll("[Lỗi] Không thể xác nhận ProjectWeek được lưu trong database.\n");
                    return;
                }
            }
            else
            {
                AppendTextWithScroll("[Thông báo] Dữ liệu ProjectWeek đã tồn tại, không tạo mới.\n");
            }

            // Kiểm tra CommitPeriod đã tồn tại
            var existingCommitPeriods = commitPeriodBUS.GetAll();

            if (existingCommitPeriods.Count == 0)
            {
                string[] periods = ["Sáng", "Chiều", "Tối", "Khuya"];
                foreach (var period in periods)
                {
                    var (since, until, _) = GetTimeRange(period);
                    var commitPeriod = new CommitPeriodET
                    {
                        PeriodName = $"Buổi {period.ToLower()}",
                        PeriodDuration = $"{since} - {until}",
                        PeriodStartTime = TimeSpan.Parse(since),
                        PeriodEndTime = TimeSpan.Parse(until),
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now
                    };
                    commitPeriodBUS.Add(commitPeriod);
                }

                // Kiểm tra dữ liệu đã được thêm thành công
                var newCommitPeriods = commitPeriodBUS.GetAll();
                if (newCommitPeriods.Count == 3) // Vì có 3 buổi: Sáng, Chiều, Tối
                {
                    AppendTextWithScroll("[Thành công] Đã tạo và lưu CommitPeriod vào database.\n");
                }
                else
                {
                    AppendTextWithScroll("[Lỗi] Không thể xác nhận CommitPeriod được lưu trong database.\n");
                    return;
                }
            }
            else
            {
                AppendTextWithScroll("[Thông báo] Dữ liệu CommitPeriod đã tồn tại, không tạo mới.\n");
            }

            AppendTextWithScroll("[Hoàn tất] Tạo ProjectWeek và CommitPeriod thành công.\n");

            // Cập nhật ComboBox sau khi hoàn thành
            cboSearchByWeek.DataSource = projectWeeksBUS.GetAll();
        }


        private (string since, string until, string periodName) GetTimeRange(string period)
        {
            string since, until, periodName;
            switch (period)
            {
                case "Sáng":
                    since = "06:00:00";
                    until = "12:00:00";
                    periodName = "morning";
                    break;
                case "Chiều":
                    since = "12:00:00";
                    until = "18:00:00";
                    periodName = "afternoon";
                    break;
                case "Tối":
                    since = "18:00:00";
                    until = "23:59:59";
                    periodName = "evening";
                    break;
                case "Khuya":
                    since = "00:00:00";
                    until = "05:59:59";
                    periodName = "evening";
                    break;
                default:
                    throw new ArgumentException("Invalid period");
            }
            return (since, until, periodName);
        }
        private void ChkConfirmInternshipDate_CheckedChanged(object sender, EventArgs e)
        {
            // Khi CheckBox được chọn, kích hoạt DateTimePicker
            txtInternshipStartDate.Enabled = chkConfirmInternshipDate.Checked;

            // Khi CheckBox không được chọn, vô hiệu hóa DateTimePicker và các nút liên quan
            if (!chkConfirmInternshipDate.Checked)
            {
                txtInternshipStartDate.Value = DateTime.Now; // Đặt lại giá trị mặc định (tùy chọn)
                btnCreateWeek.Enabled = false; // Vô hiệu hóa nút Tạo tuần thực tập
                btnOpenGitFolder.Enabled = false; // Vô hiệu hóa nút Thêm dự án
                txtInternshipStartDate.Enabled = true;// reset cho nhập lại.
            }
            else
            {
                // Khi CheckBox được chọn, kiểm tra ngày thực tập hợp lệ
                ValidateInternshipDate();
            }
        }
        private void ValidateInternshipDate()
        {
            // Kiểm tra ngày thực tập hợp lệ (ví dụ: ngày bắt đầu không được lớn hơn ngày hiện tại)
            if (txtInternshipStartDate.Value > DateTime.Now)
            {
                ShowError("Ngày bắt đầu thực tập không được lớn hơn ngày hiện tại.");
                btnCreateWeek.Enabled = false; // Vô hiệu hóa nút Tạo tuần thực tập
                btnOpenGitFolder.Enabled = false; // Vô hiệu hóa nút Thêm dự án 
                chkConfirmInternshipDate.Checked = false;
            }
            else
            // xác nhận thành công
            {
                btnCreateWeek.Enabled = true; // Kích hoạt nút Tạo tuần thực tập
                btnOpenGitFolder.Enabled = true; // Kích hoạt nút Thêm dự án
                txtInternshipEndDate.Enabled = false; // khóa ngày kết thúc
                txtNumericsWeek.Enabled = false; // khóa số tuần
            }
        }


        private void CboConfigFiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                // Xác định authorId từ ComboBox
                int configID = (cboConfigFiles.SelectedValue is int selectedID) ? selectedID : 0;

                // Cập nhật ngày commit đầu tiên và danh sách tác giả
                UpdateFirstCommitDateAndAuthors(configID);

                // Tải lại kết quả tìm kiếm
                SearchCommitsAndUpdateUI();
            }
            catch (Exception ex)
            {
                ShowError($"Lỗi khi xử lý chọn dự án: {ex.Message}");
            }
        }

        // Phương thức phụ trợ: Cập nhật ngày commit và danh sách tác giả
        private void UpdateFirstCommitDateAndAuthors(int configID)
        {
            DateTime? firstCommitDate = (configID != 0)
                ? searchBUS.GetFirstCommitDateByProject(configID)
                : null;

            txtFirstCommitDate.Text = firstCommitDate?.ToString("dd/MM/yyyy") ?? string.Empty;
            LoadAuthorsIntoComboBox(configID);
        }

        // Phương thức phụ trợ: Hiển thị lỗi
        private void ShowError(string message)
        {
            MessageBox.Show(message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }


        private void ChkDeleteAllProject_CheckedChanged(object sender, EventArgs e)
        {
            // Kiểm tra xem CheckBox có được chọn hay không
            if (chkDeleteAllProject.Checked)
            {
                // Nếu được chọn, đổi tên nút xóa thành "Xóa tất cả dự án"
                btnRemoveAll.Text = "Xóa tất cả dự án";
            }
            else
            {
                // Nếu không được chọn, đổi tên nút xóa về giá trị mặc định (ví dụ: "Xóa")
                btnRemoveAll.Text = "Xóa tất cả";
            }
        }

        private void ChkSearchCriteria_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            // Đợi sự kiện check hoàn tất trước khi xử lý
            this.BeginInvoke((MethodInvoker)delegate
            {
                // Lấy trạng thái mới của item (Checked/Unchecked)
                bool isChecked = (e.NewValue == CheckState.Checked);
                SearchCriteria criteria = (SearchCriteria)e.Index;

                // Xử lý logic dựa trên item được check
                switch (criteria)
                {
                    case SearchCriteria.chkEnablePagination:
                        // Cập nhật trạng thái phân trang
                        EnablePagination(isChecked);
                        UpdateCheckedItemText(criteria, isChecked);
                        break;

                    case SearchCriteria.chkSearchAllWeeks:
                        // Khóa/Mở khóa ComboBox chọn tuần
                        cboSearchByWeek.Enabled = !isChecked;
                        if (!isChecked && cboSearchByWeek.Items.Count > 0)
                        {
                            cboSearchByWeek.SelectedIndex = 0;
                        }
                        UpdateCheckedItemText(criteria, isChecked);
                        break;

                    case SearchCriteria.chkSearchAllAuthors:
                        // Khóa/Mở khóa ComboBox chọn tác giả
                        cboAuthorCommit.Enabled = !isChecked;
                        cboSearchByAuthor.Enabled = !isChecked;
                        if (!isChecked)
                        {
                            if (cboAuthorCommit.Items.Count > 0)
                            {
                                cboAuthorCommit.SelectedIndex = 0;
                            }
                            if (cboSearchByAuthor.Items.Count > 0)
                            {
                                cboSearchByAuthor.SelectedIndex = 0;
                            }
                        }
                        UpdateCheckedItemText(criteria, isChecked);
                        break;

                    case SearchCriteria.chkIsSimpleView:
                        // Cập nhật trạng thái hiển thị
                        UpdateCheckedItemText(criteria, isChecked);
                        break;
                }

                // Thực hiện tìm kiếm ngay lập tức
                SearchCommitsAndUpdateUI();
            });
        }

        // Cập nhật text của item dựa trên trạng thái
        private void UpdateCheckedItemText(SearchCriteria criteria, bool isChecked)
        {
            string newText = GetDisplayText(criteria, isChecked);
            chkSearchCriteria.Items[(int)criteria] = newText;
        } // Ánh xạ Enum sang text tương ứng
        private string GetDisplayText(SearchCriteria criteria, bool isChecked)
        {
            return criteria switch
            {
                SearchCriteria.chkEnablePagination => isChecked ? "Đã bật phân trang" : "Bật phân trang",
                SearchCriteria.chkSearchAllWeeks => isChecked ? "Đã bật tìm tất cả tuần" : "Tìm kiếm tất cả tuần",
                SearchCriteria.chkSearchAllAuthors => isChecked ? "Đã bật tìm tất cả tác giả" : "Tìm kiếm tất cả tác giả",
                SearchCriteria.chkIsSimpleView => isChecked ? "Đã bật hiển thị đơn giản" : "Hiển thị đơn giản",
                _ => throw new ArgumentOutOfRangeException(nameof(criteria), "Tiêu chí không hợp lệ")
            };
        }
        private void CboSearchByWeek_SelectedIndexChanged(object sender, EventArgs e)
        {
            SearchCommitsAndUpdateUI();
        }

        private void CboSearchByAuthor_SelectedIndexChanged(object sender, EventArgs e)
        {
            SearchCommitsAndUpdateUI();
        }

        private void TxtSearchReport_TextChanged(object sender, EventArgs e)
        {
            SearchCommitsAndUpdateUI();
        }
    }
}
