using System;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Drawing;
using GitLogAggregator.BusinessLogic;
using ET;
using BUS;
using System.Data;
using GitLogAggregator.Utilities;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Spreadsheet;
using Control = System.Windows.Forms.Control;

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

        // Phân trang
        private int currentPage = 1; // Trang hiện tại
        private const int pageSize = 20; // Số bản ghi mỗi trang
        private List<SearchResult> allSearchResults = new List<SearchResult>(); // Lưu trữ tất cả kết quả tìm kiếm

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

        private readonly CommitPeriodBUS commitPeriodBUS = new CommitPeriodBUS();

        private readonly ChatbotSummaryBUS chatbotSummariesBUS = new ChatbotSummaryBUS();

        // FirstCommitAuthor BUS
        private readonly AuthorBUS authorBUS = new AuthorBUS();

        // ConfigAuthor BUS
        private readonly ConfigAuthorBUS configAuthorBUS = new ConfigAuthorBUS();

        private readonly SearchBUS searchBUS = new SearchBUS();

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
            try
            {
                // Tải danh sách các thư mục thực tập từ cơ sở dữ liệu vào ComboBox
                LoadConfigsIntoCombobox();

                // Tải danh sách tác giả 0: getall
                LoadAuthorsIntoComboBox(0);

                // Lấy đường dẫn thư mục thực tập đã được chọn hoặc mặc định nếu không có
                txtFolderInternshipPath = GetLatestInternshipFolderPath();
                // tải danh sách thư mục thực tập vào combobox
                cboInternshipFolder.DataSource = internshipDirectoryBUS.GetAll();
                cboInternshipFolder.ValueMember = "ID";
                cboInternshipFolder.DisplayMember = "InternshipWeekFolder";

                // Xây dựng danh sách các tuần và tệp
                BuildWeekFileListView(txtFolderInternshipPath);

                // Tải ngày bắt đầu thực tập mặc định
                int configID = (int)cboConfigFiles.SelectedValue;
                LoadProjectDetails(0);

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
                cboSearchByWeek.ValueMember = "ProjectWeekId";
                cboSearchByWeek.DisplayMember = "ProjectWeekName";

                // Danh sách các nút crud và icon tương ứng
                var buttonsToConfigure = new Dictionary<Button, ButtonImage>
            {
                { btnSaveGit, ButtonImage.AddIcon },
                { btnRemoveAll, ButtonImage.DeleteIcon },
                { btnCreateWeek, ButtonImage.AddIcon },
                { btnSearchReport, ButtonImage.SearchIcon } // Ví dụ thêm nút chỉnh sửa
            };

                // Cấu hình các nút icon crud
                ConfigureButtons(buttonsToConfigure);

                // biến tất cả combobox về dạng dropdownlist
                SetAllComboBoxesToDropDownList(this);

                // Hiển thị hint cho các control
                SetupHoverEventsForControls(txtResultMouseEvents);
                //
                // Cấu hình trạng thái của CheckBox và ComboBox
                //
                // Nếu CheckBox được chọn, thực hiện tìm kiếm tất cả các tuần
                chkSearchAllWeeks.Checked = true;

                ConfigureSearchControls();

                if (chkSearchAllWeeks.Checked)
                {
                    SearchCommitWhenLoadForm();
                }

                DisableControls();
            }
            catch (Exception ex)
            {
                AppendTextWithScroll($"Lỗi: {ex.Message}");
            }
        }

        private void LoadProjectDetails(int configId)
        {
            // Lấy ngày bắt đầu thực tập
            DateTime? startDate = configBus.GetInternshipStartDate(configId);

            if (startDate == null)
            {
                // Hiển thị thông báo nếu không có ngày bắt đầu thực tập
                MessageBox.Show("Vui lòng chọn ngày bắt đầu thực tập.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                // Thiết lập giá trị cho DateTimePicker
                txtInternshipStartDate.Value = startDate.Value;
            }
        }
        private void LoadConfigsIntoCombobox()
        {

            // Lấy danh sách các dự án từ cơ sở dữ liệu
            var configFiles = configBus.GetAll();

            // Thêm một mục tùy chọn "Tất cả dự án" vào danh sách
            var allProjectsOption = new ConfigFileET
            {
                ConfigID = 0, // Sử dụng ConfigID đặc biệt (ví dụ: 0) để phân biệt "Tất cả dự án"
                ProjectDirectory = "Tất cả dự án" // Hiển thị "Tất cả dự án"
            };

            // làm trống danh sách trước khi thêm
            cboConfigFiles.DataSource = null;

            // Chèn "Tất cả dự án" vào đầu danh sách
            configFiles.Insert(0, allProjectsOption);

            // Gán danh sách làm nguồn dữ liệu cho ComboBox
            cboConfigFiles.DataSource = configFiles;
            cboConfigFiles.ValueMember = "ConfigID";
            cboConfigFiles.DisplayMember = "ProjectDirectory";
        }
        private void LoadAuthorsIntoComboBox(int configID)
        {
            // Lấy danh sách tác giả theo ConfigID
            List<AuthorET> authors;

            if (configID == 0)
            {
                // Lấy tất cả tác giả từ bảng Authors
                authors = authorBUS.GetAll();
            }
            else
            {
                // Lấy danh sách AuthorID theo ConfigID
                List<int> authorIDs = configAuthorBUS.GetAuthorIDsByConfigID(configID);

                // Lấy thông tin chi tiết của từng tác giả
                authors = authorIDs
                   .Select(authorID => authorBUS.GetByID(authorID)) // Lấy thông tin tác giả theo AuthorID
                   .ToList();
            }

            // Hiển thị danh sách tác giả lên ComboBox
            UpdateAuthorList(authors);
        }

        private void SearchCommitWhenLoadForm()
        {
            try
            {
                // Hiển thị thông báo đang tải dữ liệu
                AppendTextWithScroll("Đang tải dữ liệu...\n");

                // Tải dữ liệu
                SearchCommitsAndUpdateUI();
            }
            catch (Exception ex)
            {
                AppendTextWithScroll($"Lỗi khi tải dữ liệu ban đầu: {ex.Message}\n");
            }

            // Ghi thông báo kết quả
            if (allSearchResults != null && allSearchResults.Any())
            {
                AppendTextWithScroll($"Hiển thị tất cả dữ liệu. Tìm thấy {allSearchResults.Count} kết quả.\n");
            }
            else
            {
                AppendTextWithScroll("Không có dữ liệu nào để hiển thị.\n");
            }
        }
        private void ConfigureSearchControls()
        {
            if (chkSearchAllWeeks.Checked)
            {
                cboSearchByWeek.Enabled = false; // Khóa ComboBox
                chkSearchAllWeeks.Text = "Tìm kiếm tất cả các tuần"; // Thay đổi text
            }
            else
            {
                cboSearchByWeek.Enabled = true; // Mở khóa ComboBox
                chkSearchAllWeeks.Text = "Tìm kiếm theo tuần cụ thể"; // Thay đổi text
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
        private void ConfigureButtons(Dictionary<Button, ButtonImage> buttonsToConfigure)
        {
            foreach (var item in buttonsToConfigure)
            {
                Button button = item.Key;
                ButtonImage buttonImage = item.Value;

                // Lấy imageKey từ từ điển dựa trên giá trị ButtonImage
                if (ButtonImageMap.ImageKeys.TryGetValue(buttonImage, out string imageKey))
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
                { btnExportExcel, "Xuất excel commit" },
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
                { chkSearchAllWeeks, "Tìm kiếm tất cả các tuần của commit trong datagridview" },
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

                projectDirectory = folderBrowserDialog.SelectedPath;

                // Kiểm tra thư mục có phải là Git repository hợp lệ không
                if (!IsValidGitRepository(projectDirectory))
                {
                    btnOpenGitFolder.Enabled = true; // Bật lại nút mở thư mục
                    AppendTextWithScroll("Lỗi: Thư mục được chọn không chứa repository Git hợp lệ. Vui lòng chọn lại.\n");
                    return;
                }

                // Kiểm tra repository có chứa commit nào không
                if (!HasCommitsInRepository(projectDirectory))
                {
                    btnOpenGitFolder.Enabled = true; // Bật lại nút mở thư mục
                    AppendTextWithScroll("Lỗi: Repository Git này không chứa bất kỳ commit nào. Vui lòng chọn một repository khác hoặc tạo commit đầu tiên.\n");
                    return;
                }

                // Kiểm tra xem dự án đã tồn tại trong cơ sở dữ liệu chưa
                if (configBus.GetAll().Any(cf => cf.ProjectDirectory == projectDirectory))
                {
                    AppendTextWithScroll("Lỗi: Dự án đã tồn tại trong cơ sở dữ liệu.\n");
                    btnOpenGitFolder.Enabled = true; // Bật lại nút mở thư mục
                    return;
                }

                // Lấy thông tin commit đầu tiên
                DateTime firstCommitDate = gitgui_bus.GetFirstCommitDate(projectDirectory);

                // Kiểm tra ngày bắt đầu thực tập có hợp lệ không
                if (txtInternshipStartDate.Value >= firstCommitDate)
                {
                    AppendTextWithScroll("Lỗi: Ngày bắt đầu thực tập phải nhỏ hơn ngày commit đầu tiên.\n");
                    btnOpenGitFolder.Enabled = true; // Bật lại nút mở thư mục
                    return;
                }

                // Lấy danh sách tác giả và email từ lịch sử commit của repository
                var authors = gitgui_bus.GetAuthorsFromRepository(projectDirectory);

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
                ConfigFileET configFile = new ConfigFileET
                {
                    ProjectDirectory = projectDirectory,
                    Weeks = (int)txtNumericsWeek.Value,
                    InternshipDirectoryId = (int)cboInternshipFolder.SelectedValue,
                    InternshipStartDate = txtInternshipStartDate.Value,
                    InternshipEndDate = txtInternshipEndDate.Value,
                    FirstCommitDate = firstCommitDate,
                    FirstCommitAuthor = gitgui_bus.GetFirstCommitAuthor(projectDirectory),
                };

                configBus.Add(configFile);

                // Bước 3: Thêm mối quan hệ giữa dự án và tác giả vào bảng ConfigAuthors
                ConfigFileET lastAddedConfigFile = configBus.GetLastAddedConfigFile();

                if (lastAddedConfigFile == null)
                {
                    AppendTextWithScroll("Lỗi: Không thể lấy thông tin dự án vừa thêm.\n");
                    return;
                }

                // Chú ý: authors là danh sach tác giả tham gia dự án thực tập mà hàm này đang chọn projectDirectory

                foreach (var (authorName, authorEmail) in authors)
                {
                    // Lấy thông tin tác giả từ cơ sở dữ liệu dựa trên email
                    AuthorET author = authorBUS.GetByEmail(authorEmail);

                    if (author == null)
                    {
                        AppendTextWithScroll($"Lỗi: Không tìm thấy tác giả với email {authorEmail} trong cơ sở dữ liệu.\n");
                        continue;
                    }

                    try
                    {
                        // Kiểm tra xem mối quan hệ giữa dự án và tác giả đã tồn tại chưa
                        bool relationshipExists = configAuthorBUS.Exists(lastAddedConfigFile.ConfigID, author.AuthorID);

                        if (!relationshipExists)
                        {
                            // Thêm mối quan hệ giữa dự án và tác giả nếu chưa tồn tại
                            configAuthorBUS.Add(new ConfigAuthorET
                            {
                                ConfigID = lastAddedConfigFile.ConfigID,
                                AuthorID = author.AuthorID
                            });

                            AppendTextWithScroll($"Đã thêm mối quan hệ giữa dự án và tác giả {author.AuthorName} (ID: {author.AuthorID}).\n");
                        }
                        else
                        {
                            AppendTextWithScroll($"Mối quan hệ giữa dự án và tác giả {author.AuthorName} (ID: {author.AuthorID}) đã tồn tại.\n");
                        }
                    }
                    catch (Exception ex)
                    {
                        AppendTextWithScroll($"Lỗi khi thêm mối quan hệ giữa dự án và tác giả {author.AuthorName} (ID: {author.AuthorID}): {ex.Message}\n");
                    }
                }

                AppendTextWithScroll("Thêm mối quan hệ giữa dự án và tác giả hoàn tất.\n");

                // Cập nhật giao diện khi chọn thư mục dự án
                UpdateControls(configFile);

                // Tải danh sách tác giả của dự án mới vào ComboBox
                LoadAuthorsIntoComboBox(configFile.ConfigID);

                // Load lại dữ liệu lên combobox
                LoadConfigsIntoCombobox();

                AppendTextWithScroll("Dự án và thông tin cấu hình đã được thêm vào cơ sở dữ liệu thành công.\n");

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
        private void UpdateControls(ConfigFileET configInfo)
        {
            txtInternshipStartDate.Value = configInfo.InternshipStartDate;
            txtInternshipEndDate.Value = configInfo.InternshipEndDate;
            txtNumericsWeek.Value = configInfo.Weeks;
            txtFirstCommitDate.Value = configInfo.FirstCommitDate;
        }

        private void SetMaxDateForDateTimePicker(DateTimePicker dateTimePicker, DateTime maxDate)
        {
            dateTimePicker.MaxDate = maxDate;
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
            cboAuthorCommit.Enabled = true;
            cboInternshipFolder.Enabled = true;
            txtNumericsWeek.Enabled = true;
            txtInternshipStartDate.Enabled = true;
            txtInternshipEndDate.Enabled = true;
            txtFirstCommitDate.Enabled = true;
            // Thêm các điều khiển khác nếu cần
        }
        private void DisableControls()
        {
            cboAuthorCommit.Enabled = false;
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
        private void BtnAggregateCommits_Click(object sender, EventArgs e)
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
        private string GetPeriodAbbreviation(string period)
        {
            switch (period)
            {
                case "Sáng":
                    return "S";
                case "Chiều":
                    return "C";
                case "Tối":
                    return "T";
                default:
                    throw new ArgumentException("Giá trị period không hợp lệ.");
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
            // Lấy thông tin dự án từ cơ sở dữ liệu
            List<ConfigFileET> configFiles = configBus.GetAll();

            foreach (ConfigFileET configFile in configFiles)
            {
                if (configFile != null)
                {
                    string projectDirectory = configFile.ProjectDirectory;

                    // Tạo thư mục theo tuần > Thứ > Buổi
                    var iswf = internshipDirectoryBUS.GetByID(configFile.InternshipDirectoryId);
                    var internshipWeekFolder = iswf.InternshipWeekFolder;
                    var internshipStartDate = configFile.InternshipStartDate;

                    try
                    {
                        // Tạo thư mục tổng hợp Combined nếu chưa tồn tại
                        string combinedFolder = Path.Combine(internshipWeekFolder, "Combined");
                        if (!Directory.Exists(combinedFolder))
                        {
                            Directory.CreateDirectory(combinedFolder);
                        }

                        // Lấy danh sách các tuần từ CSDL
                        List<ProjectWeekET> weeks = projectWeeksBUS.GetAll()
                            .Where(w => w.InternshipDirectoryId == configFile.InternshipDirectoryId)
                            .ToList();

                        foreach (var week in weeks)
                        {
                            DateTime weekStartDate = week.WeekStartDate;
                            DateTime weekEndDate = week.WeekEndDate;
                            int currentWeek = weeks.IndexOf(week) + 1;
                            string weekFolder = Path.Combine(internshipWeekFolder, $"Week_{currentWeek}");
                            string combinedFile = Path.Combine(combinedFolder, $"combined_week_{currentWeek}.txt");
                            bool hasCommitsInWeek = false;

                            // Tạo thư mục tuần nếu chưa tồn tại
                            if (!Directory.Exists(weekFolder))
                            {
                                Directory.CreateDirectory(weekFolder);
                            }

                            // Lấy danh sách các commit trong tuần từ CSDL
                            var weekCommits = commitBUS.GetAll()
                                .Where(c => c.CommitDate >= weekStartDate && c.CommitDate <= weekEndDate)
                                .ToList();

                            for (int dayOffset = 0; dayOffset < 7; dayOffset++)
                            {
                                DateTime currentDate = weekStartDate.AddDays(dayOffset);
                                string dayFolder = weekFolder;
                                //bool hasCommitsInDay = false;

                                // Tạo thư mục ngày nếu chưa tồn tại
                                if (!Directory.Exists(dayFolder))
                                {
                                    Directory.CreateDirectory(dayFolder);
                                }

                                foreach (var period in new[] { "Sáng", "Chiều", "Tối" })
                                {
                                    // Chuyển đổi period sang dạng viết tắt
                                    var periodAbb = GetPeriodAbbreviation(period);

                                    // Tạo đường dẫn file
                                    string dailyFile = Path.Combine(dayFolder, $"{currentDate:yyyy-MM-dd}_{periodAbb}_commits.txt");

                                    // Lấy danh sách các commit trong buổi từ CSDL
                                    var periodCommits = weekCommits
                                        .Where(c => c.Date == currentDate.Date && c.Period == periodAbb)
                                        .ToList();

                                    if (periodCommits.Any())
                                    {
                                        foreach (var commit in periodCommits)
                                        {
                                            string commitInfo = $"[{commit.CommitHash}] {commit.CommitMessage} - {commit.CommitDate} - {commit.Author}\n";
                                            AppendToFile(dailyFile, commitInfo); // Ghi vào file hàng ngày
                                            AppendToFile(combinedFile, $"[{currentDate:yyyy-MM-dd} {periodAbb}] {commitInfo}"); // Ghi vào file tổng hợp

                                            //hasCommitsInDay = true;
                                            hasCommitsInWeek = true;
                                        }
                                    }
                                    else
                                    {
                                        // Tạo file trống nếu không có commit
                                        File.WriteAllText(dailyFile, string.Empty);
                                    }
                                }
                            }

                            // Kiểm tra và xóa thư mục/file trống sau khi xử lý xong tuần
                            DeleteEmptyFoldersAndFiles(weekFolder);

                            if (hasCommitsInWeek)
                            {
                                AppendTextWithScroll($"Week {currentWeek} commits đã tổng hợp vào: {combinedFile}");
                            }
                        }

                        // Kiểm tra và xóa thư mục Combined nếu trống
                        DeleteEmptyFoldersAndFiles(combinedFolder);
                    }
                    catch (Exception ex)
                    {
                        AppendTextWithScroll($"Lỗi: {ex.Message}\n");
                    }

                    AppendTextWithScroll($"Đã tổng hợp commit cho dự án: {projectDirectory}.\n");
                }
            }
        }

        private bool IsGitRepository(string directory)
        {
            string gitFolder = Path.Combine(directory, ".git");
            return Directory.Exists(gitFolder);
        }

        private (string since, string until, string periodName) GetTimeRange(DateTime date, string period)
        {
            string since, until, periodName;
            switch (period)
            {
                case "Sáng":
                    since = $"{date:yyyy-MM-dd} 06:00:00"; // Buổi sáng bắt đầu từ 6:00
                    until = $"{date:yyyy-MM-dd} 12:00:00"; // Buổi sáng kết thúc lúc 12:00
                    periodName = "morning";
                    break;
                case "Chiều":
                    since = $"{date:yyyy-MM-dd} 12:00:00"; // Buổi chiều bắt đầu từ 12:00
                    until = $"{date:yyyy-MM-dd} 18:00:00"; // Buổi chiều kết thúc lúc 18:00
                    periodName = "afternoon";
                    break;
                case "Tối":
                    since = $"{date:yyyy-MM-dd} 18:00:00"; // Buổi tối bắt đầu từ 18:00
                    until = $"{date:yyyy-MM-dd} 23:59:59"; // Buổi tối kết thúc lúc 23:59
                    periodName = "evening";
                    break;
                default:
                    throw new ArgumentException("Invalid timePeriod");
            }
            return (since, until, periodName);
        }
        /// <summary>
        /// Chuyển đổi từ DayOfWeek sang tên tiếng Việt
        /// </summary>
        private string GetDayOfWeekName(DayOfWeek dayOfWeek)
        {
            return dayOfWeek switch
            {
                DayOfWeek.Monday => "Thứ Hai",
                DayOfWeek.Tuesday => "Thứ Ba",
                DayOfWeek.Wednesday => "Thứ Tư",
                DayOfWeek.Thursday => "Thứ Năm",
                DayOfWeek.Friday => "Thứ Sáu",
                DayOfWeek.Saturday => "Thứ Bảy",
                DayOfWeek.Sunday => "Chủ Nhật",
                _ => throw new ArgumentException("Invalid day of week")
            };
        }

        private void AppendToFile(string filePath, string content)
        {
            File.AppendAllText(filePath, content + Environment.NewLine);
        }
        private void WriteToFile(string filePath, string content)
        {
            try
            {
                // Tạo thư mục cha nếu chưa tồn tại
                string directoryPath = Path.GetDirectoryName(filePath);
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                // Ghi nội dung vào file (tạo mới hoặc ghi đè)
                File.WriteAllText(filePath, content + Environment.NewLine);
            }
            catch (Exception ex)
            {
                // Xử lý ngoại lệ (ví dụ: ghi log hoặc hiển thị thông báo)
                AppendTextWithScroll($"Lỗi khi ghi file {filePath}: {ex.Message}\n");
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

                // Làm mới đường dẫn thư mục thực tập
                txtFolderInternshipPath = GetLatestInternshipFolderPath();

                // Kiểm tra sự tồn tại của thư mục internship_week
                if (!Directory.Exists(txtFolderInternshipPath))
                {
                    AppendTextWithScroll("Thư mục thực tập không tồn tại. Vui lòng kiểm tra lại.\n");
                    return; // Dừng nếu thư mục không tồn tại
                }

                // Làm mới combobox thư mục thực tập
                cboInternshipFolder.DataSource = null;
                cboInternshipFolder.DataSource = internshipDirectoryBUS.GetAll();
                cboInternshipFolder.ValueMember = "ID";
                cboInternshipFolder.DisplayMember = "InternshipWeekFolder";
                AppendTextWithScroll("Danh sách thư mục thực tập đã được làm mới.\n");

                // Làm mới dữ liệu trong combobox cboConfigFiles danh sách dự án thực tập
                LoadConfigsIntoCombobox();
                AppendTextWithScroll("Danh sách dự án thực tập đã được làm mới.\n");

                // Làm mới combobox tác giả commit
                LoadAuthorsIntoComboBox(0);
                AppendTextWithScroll("Danh sách tác giả đã được làm mới.\n");

                // Làm mới danh sách thư mục và file trong ListView
                BuildWeekFileListView(txtDirectoryProjectPath);
                AppendTextWithScroll("Danh sách commit file text đã được làm mới.\n");

                // Làm mới dữ liệu trong DataGridView
                dgvReportCommits.DataSource = commitBUS.GetAll();
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
            // Khi ngày thực tập thay đổi, kiểm tra lại tính hợp lệ của ngày
            if (chkConfirmInternshipDate.Checked)
            {
                ValidateInternshipDate();
            }
            else
            {
                DateTime startDate = txtInternshipStartDate.Value;
                int weeks = (int)txtNumericsWeek.Value;
                DateTime endDate = gitgui_bus.CalculateEndDate(startDate, weeks);

                // Hiển thị ngày kết thúc
                txtInternshipEndDate.Value = endDate;
                btnOpenGitFolder.Enabled = true;// chọn tuần thực tập xong mới được thêm dự án
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
            using (var folderBrowserDialog = new FolderBrowserDialog())
            {
                folderBrowserDialog.Description = "Chọn thư mục thực tập";
                folderBrowserDialog.RootFolder = Environment.SpecialFolder.Desktop;

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
                                "Thư mục thực tập đã tồn tại. Bạn có muốn xóa thư mục này không?", // Nội dung thông báo
                                "Xác nhận xóa thư mục",                                           // Tiêu đề hộp thoại
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
                        // Thư mục rỗng, thiết lập thao tác cài đặt mới
                        InternshipDirectoryET internshipDirectory = new InternshipDirectoryET
                        {
                            InternshipWeekFolder = txtFolderInternshipPath,
                            DateModified = DateTime.Now
                        };

                        // Lưu đường dẫn thư mục mới vào cơ sở dữ liệu
                        internshipDirectoryBUS.Add(internshipDirectory);
                    }

                    // Tải danh sách các thư mục thực tập từ cơ sở dữ liệu vào ComboBox
                    cboInternshipFolder.DataSource = internshipDirectoryBUS.GetAll();
                    cboInternshipFolder.ValueMember = "ID"; // Thiết lập trường sẽ làm giá trị
                    cboInternshipFolder.DisplayMember = "InternshipWeekFolder"; // Thiết lập trường sẽ hiển thị trên combobox

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

            StringBuilder outputBuilder = new StringBuilder();
            StringBuilder errorBuilder = new StringBuilder();

            process.OutputDataReceived += (sender, args) => outputBuilder.AppendLine(args.Data);
            process.ErrorDataReceived += (sender, args) => errorBuilder.AppendLine(args.Data);

            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
            process.WaitForExit();

            if (process.ExitCode != 0)
            {
                throw new Exception($"Git command failed with exit code {process.ExitCode}. Error: {errorBuilder.ToString()}");
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

            StringBuilder outputBuilder = new StringBuilder();
            StringBuilder errorBuilder = new StringBuilder();

            process.OutputDataReceived += (sender, args) => outputBuilder.AppendLine(args.Data);
            process.ErrorDataReceived += (sender, args) => errorBuilder.AppendLine(args.Data);

            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
            process.WaitForExit();

            if (process.ExitCode != 0)
            {
                throw new Exception($"Git command failed with exit code {process.ExitCode}. Error: {errorBuilder.ToString()}");
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
        private void AggregateCommits(ConfigFileET config)
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
                if (!Directory.Exists(config.ProjectDirectory))
                {
                    throw new DirectoryNotFoundException($"Thư mục dự án không tồn tại: {config.ProjectDirectory}");
                }

                // Xác định thư mục dự án Git và chạy lệnh git thông qua file batch để lấy thông tin commit
                string gitLogCommand = $"log --reverse --pretty=format:\"%H|%s|%ci|%an|%ae\"";
                string logOutput = RunGitCommandViaBatch(batchFilePath, gitLogCommand, config.ProjectDirectory); // Hàm chạy lệnh git qua batch
                var commits = ParseGitLog(logOutput); // Process the log output

                // Lấy danh sách các tuần từ database
                List<ProjectWeekET> weeks = projectWeeksBUS.GetAll();

                // Danh sách tạm để lưu các commit đã thêm vào
                List<CommitET> insertedCommits = new List<CommitET>();

                // Bước 1: Thêm tất cả các commit vào database
                foreach (var commit in commits)
                {
                    try
                    {
                        // Kiểm tra xem commit đã tồn tại chưa
                        var existingCommit = commitBUS.GetAll().FirstOrDefault(c => c.CommitHash == commit.CommitHash);

                        if (existingCommit == null)
                        {
                            // Xác định period từ giờ commit
                            string period = DeterminePeriod(commit.CommitDate.Hour);

                            // Tìm ProjectWeekId tương ứng với ngày của commit
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
                                CommitMessage = commit.CommitMessage,
                                CommitDate = commit.CommitDate,
                                Author = commit.Author,
                                AuthorEmail = commit.AuthorEmail,
                                ProjectWeekId = projectWeek.ProjectWeekId, // Gán ProjectWeekId chính xác
                                Date = commit.CommitDate.Date, // Ngày của commit
                                Period = period, // Period được tính từ CommitDate
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

                // Bước 2: Tạo CommitGroupMembers sau khi đã thêm tất cả các commit
                foreach (var week in weeks)
                {
                    // Lấy nhóm commit tương ứng với tuần hiện tại
                    var commitPeriod = commitPeriodBUS.GetAll()
                        .FirstOrDefault(g => g.PeriodName == week.ProjectWeekName);

                    if (commitPeriod == null)
                    {
                        AppendTextWithScroll($"Không tìm thấy nhóm commit cho tuần {week.ProjectWeekName}.\n");
                        continue;
                    }

                    // Lấy các commit thuộc tuần hiện tại từ danh sách đã thêm vào
                    var weekCommits = insertedCommits
                        .Where(c => c.CommitDate >= week.WeekStartDate && c.CommitDate <= week.WeekEndDate)
                        .ToList();

                    foreach (var commit in weekCommits)
                    {
                        try
                        {
                            // Thêm commit vào nhóm commit của tuần hiện tại
                            var commitGroupMember = new CommitGroupMemberET
                            {
                                PeriodID = commitPeriod.PeriodID,
                                CommitId = commit.CommitId,
                                AddedAt = DateTime.Now
                            };

                            // Thêm commit vào nhóm
                            commitGroupMembersBUS.Add(commitGroupMember);
                        }
                        catch (Exception ex)
                        {
                            AppendTextWithScroll($"Lỗi khi thêm commit {commit.CommitHash} vào nhóm: {ex.Message}\n");
                        }
                    }

                    AppendTextWithScroll($"Week {week.ProjectWeekName} commits đã tổng hợp vào db.\n");
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

        // Helper method to determine the timePeriod of the day
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
                if (parts.Length == 5) // Đảm bảo có đủ 5 phần: CommitHash, CommitMessage, CommitDate, FirstCommitAuthor, FirstCommitAuthor Email
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
                            Author = parts[3].Trim(),
                            AuthorEmail = parts[4].Trim(),
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
            try
            {
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
                    // Hiển thị dữ liệu đã thêm vào csdl lên dgv
                    //
                    // Cấu hình trạng thái của CheckBox và ComboBox
                    //
                    // Nếu CheckBox được chọn, thực hiện tìm kiếm tất cả các tuần
                    SearchCommitsAndUpdateUI();

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
                AppendTextWithScroll($"Lỗi: {ex.Message}");
            }
        }
        private void SearchCommitsAndUpdateUI()
        {
            try
            {
                // Lấy các tiêu chí từ giao diện
                string keyword = txtSearchReport.Text;
                bool searchAllWeeks = chkSearchAllWeeks.Checked;
                int projectWeekId = searchAllWeeks ? 0 : Convert.ToInt32(cboSearchByWeek.SelectedValue);
                DateTime? minDate = DateTime.TryParse(txtFirstCommitDate.Text, out var parsedDate) ? parsedDate : (DateTime?)null;
                DateTime maxDate = DateTime.Now;
                string author = chkSearchAllAuthors.Checked ? null : cboSearchByAuthor.SelectedValue?.ToString();

                // Gọi phương thức tìm kiếm từ lớp BUS/DAL
                allSearchResults = searchBUS.SearchCommits(
                    keyword: keyword,
                    projectWeekId: projectWeekId,
                    searchAllWeeks: searchAllWeeks,
                    minDate: minDate,
                    maxDate: maxDate,
                    author: author
                );

                // Cập nhật giao diện
                currentPage = 1; // Reset về trang đầu tiên
                DisplaySearchResults(); // Hiển thị kết quả
                txtSearchReport.Clear();
            }
            catch (Exception ex)
            {
                txtSearchReport.Clear();
                AppendTextWithScroll($"Lỗi: {ex.Message}\n");
            }
        }
        private void BtnSearchReport_Click(object sender, EventArgs e)
        {
            SearchCommitsAndUpdateUI();
        }

        private void DisplaySearchResults()
        {
            if (allSearchResults != null && allSearchResults.Any())
            {
                // Tính toán phân trang
                int pageSize = 10;
                var pagedResults = allSearchResults
                    .Skip((currentPage - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                // Hiển thị lên DataGridView
                dgvReportCommits.DataSource = pagedResults;

                // Thông báo trạng thái
                AppendTextWithScroll($"Trang {currentPage}/{Math.Ceiling((double)allSearchResults.Count / pageSize)} | Hiển thị {pagedResults.Count} kết quả.\n");
            }
            else
            {
                dgvReportCommits.DataSource = null;
                AppendTextWithScroll("Không tìm thấy kết quả phù hợp.\n");
            }
        }
        private void BtnPreviousReport_Click(object sender, EventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--; // Giảm số trang hiện tại
                DisplaySearchResults(); // Hiển thị kết quả theo trang mới
            }
            else
            {
                AppendTextWithScroll("Bạn đang ở trang đầu tiên.\n");
            }
        }
        private void BtnNextReport_Click(object sender, EventArgs e)
        {
            int totalPages = (int)Math.Ceiling((double)allSearchResults.Count / pageSize);

            if (currentPage < totalPages)
            {
                currentPage++; // Tăng số trang hiện tại
                DisplaySearchResults(); // Hiển thị kết quả theo trang mới
            }
            else
            {
                AppendTextWithScroll("Bạn đang ở trang cuối cùng.\n");
            }
        }
        private void btnRemoveAll_Click(object sender, EventArgs e)
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
        private void btnCreateWeek_Click(object sender, EventArgs e)
        {


            if (cboInternshipFolder.SelectedValue == null)
            {
                AppendTextWithScroll("Vui lòng chọn thư mục thực tập trước khi tạo tuần thực tập.\n");
                return;
            }
            int internshipDirectoryId = int.Parse(cboInternshipFolder.SelectedValue.ToString());

            DateTime internshipStartDate = txtInternshipStartDate.Value.Date;
            if (internshipStartDate == null)
            {
                AppendTextWithScroll("Vui lòng chọn ngày bắt đầu thực tập.\n");
                return;
            }
            DateTime internshipEndDate = txtInternshipEndDate.Value.Date;

            int weeks = (int)txtNumericsWeek.Value;
            if (weeks <= 0)
            {
                AppendTextWithScroll("Số tuần phải lớn hơn 0.\n");
                return;
            }

            var configs = configBus.GetAll();
            if (configs.Count == 0)
            {
                AppendTextWithScroll("Vui lòng chọn dự án trước khi tạo tuần thực tập.\n");
                return;
            }

            // Kiểm tra ngày commit đầu tiên của các project xem nó có nằm trong tuần thực tập không
            var check = false;
            foreach (var c in configs)
            {
                if (internshipStartDate <= c.FirstCommitDate.Date && c.FirstCommitDate.Date <= internshipEndDate)
                {
                    check = true;
                }
            }
            if (!check)
            {
                AppendTextWithScroll("Không thể tạo tuần thực tập vì dự án không nằm trong kỳ thực tập.\n");
                check = false;
                return;
            }

            // Kiểm tra xem đã có dữ liệu CommitPeriod và ProjectWeek trong khoảng thời gian thực tập chưa
            var existingCommitGroups = commitPeriodBUS.GetAll()
                .Where(g => g.PeriodStartDate >= internshipStartDate && g.PeriodEndDate <= internshipEndDate)
                .ToList();

            var existingProjectWeeks = projectWeeksBUS.GetAll()
                .Where(w => w.WeekStartDate >= internshipStartDate && w.WeekEndDate <= internshipEndDate)
                .ToList();

            if (existingCommitGroups.Count > 0 || existingProjectWeeks.Count > 0)
            {
                AppendTextWithScroll("Đã có dữ liệu CommitPeriod và ProjectWeek trong khoảng thời gian thực tập, không cần tạo thêm.\n");
                return;
            }

            // Tạo các tuần và CommitGroup mới
            for (int weekOffset = 0; weekOffset < weeks; weekOffset++)
            {
                DateTime weekStartDate = internshipStartDate.AddDays(weekOffset * 7);
                DateTime weekEndDate = weekStartDate.AddDays(6);
                string projectWeekName = $"Tuần {weekOffset + 1}"; // Bắt đầu từ Tuần 1

                // Tạo ProjectWeek
                var projectWeek = new ProjectWeekET
                {
                    InternshipDirectoryId = internshipDirectoryId,
                    ProjectWeekName = projectWeekName,
                    WeekStartDate = weekStartDate.Date,
                    WeekEndDate = weekEndDate.Date,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                };
                projectWeeksBUS.Add(projectWeek);

                // Tạo các CommitGroup (buổi) cho từng ngày trong tuần
                for (int dayOffset = 0; dayOffset < 7; dayOffset++) // 7 ngày trong tuần
                {
                    DateTime currentDate = weekStartDate.AddDays(dayOffset);

                    // Tạo CommitGroup cho từng buổi (sáng, chiều, tối)
                    string[] periods = { "Sáng", "Chiều", "Tối" };
                    foreach (var period in periods)
                    {
                        var (since, until, periodName) = GetTimeRange(currentDate, period);

                        var commitGroup = new CommitPeriodET
                        {
                            PeriodName = $"Buổi {period.ToLower()} {currentDate:dd/MM/yyyy}", // Ví dụ: "Buổi sáng 16/01/2025"
                            PeriodDuration = periodName, // Ví dụ: "morning"
                            PeriodStartDate = DateTime.Parse(since), // Ví dụ: "2025-01-16 06:00:00"
                            PeriodEndDate = DateTime.Parse(until),   // Ví dụ: "2025-01-16 12:00:00"
                            CreatedAt = DateTime.Now,
                            UpdatedAt = DateTime.Now
                        };
                        commitPeriodBUS.Add(commitGroup);
                    }
                }
            }

            AppendTextWithScroll("Tạo xong commitgroup.\n");
            AppendTextWithScroll("Tạo xong projectweek.\n");

            // Cập nhật lại danh sách tuần trên ComboBox
            cboSearchByWeek.DataSource = projectWeeksBUS.GetAll();
            cboSearchByWeek.ValueMember = "ProjectWeekId";
            cboSearchByWeek.DisplayMember = "ProjectWeekName";
        }

        private void chkConfirmInternshipDate_CheckedChanged(object sender, EventArgs e)
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

        private void chkSearchAllWeeks_CheckedChanged(object sender, EventArgs e)
        {
            // Cập nhật text trên checkbox dựa vào trạng thái checked
            if (chkSearchAllWeeks.Checked)
            {
                chkSearchAllWeeks.Text = "Tìm kiếm tất cả các tuần"; // Text khi check
                cboSearchByWeek.Enabled = false; // Khóa ComboBox
                cboSearchByWeek.SelectedIndex = 0; // Đặt giá trị mặc định (ví dụ: "Tất cả")
            }
            else
            {
                chkSearchAllWeeks.Text = "Tìm kiếm theo tuần cụ thể"; // Text khi uncheck
                cboSearchByWeek.Enabled = true; // Mở khóa ComboBox
            }

            // Tùy chọn: Tự động cập nhật kết quả tìm kiếm
            SearchCommitsAndUpdateUI();
        }

        private void chkSearchAllAuthors_CheckedChanged(object sender, EventArgs e)
        {
            if (chkSearchAllAuthors.Checked)
            {
                chkSearchAllAuthors.Text = "Tìm kiếm tất cả tác giả";
                cboSearchByAuthor.Enabled = false;
                cboSearchByAuthor.SelectedIndex = 0;
            }
            else
            {
                chkSearchAllAuthors.Text = "Tìm kiếm theo tác giả cụ thể";
                cboSearchByAuthor.Enabled = true;
            }
            SearchCommitsAndUpdateUI();
        }

        private void cboConfigFiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                // Xác định configID từ ComboBox
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

        private void UpdateAuthorList(List<AuthorET> authors)
        {
            // Thêm mục "Tất cả" vào đầu danh sách
            authors.Insert(0, new AuthorET { AuthorID = 0, AuthorName = "Tất cả" });

            // Gán danh sách tác giả vào ComboBox (sử dụng DataSource)
            cboSearchByAuthor.DataSource = authors; // <-- Sửa ở đây
            cboSearchByAuthor.DisplayMember = "AuthorName"; // Hiển thị tên
            cboSearchByAuthor.ValueMember = "AuthorID"; // Giá trị thực là ID

            // Đồng bộ với cboAuthorCommit (nếu cần)
            cboAuthorCommit.DataSource = authors;
            cboAuthorCommit.DisplayMember = "AuthorName";
            cboAuthorCommit.ValueMember = "AuthorID";

            // Mặc định chọn "Tất cả"
            cboSearchByAuthor.SelectedIndex = 0;
            cboAuthorCommit.SelectedIndex = 0;
        }

        private void cboSearchByAuthor_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cboAuthorCommit_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void chkDeleteAllProject_CheckedChanged(object sender, EventArgs e)
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
    }
}

