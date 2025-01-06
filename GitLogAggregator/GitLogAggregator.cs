﻿using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Globalization;
using System.Drawing;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using GitLogAggregator.BusinessLogic;
using ET;


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
        private readonly GitlogBUS bll = new GitlogBUS();

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
            authorsComboBox.Enabled = false;
            startDatePicker.Enabled = false;
            aggregateButton.Enabled = false;

            // Ẩn nút xóa khi form mới tải
            deleteFolderButton.Enabled = false;

            if (!string.IsNullOrEmpty(projectDirectory))
            {
                LoadAuthorsCombobox(projectDirectory);
                EnableControls();
            }
        }
        /// <summary>
        /// Người dùng chọn thư mục qua hộp thoại.
        /// Load danh sách tác giả commit từ Git bằng lệnh
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void selectGitFolderButton_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                // Lưu đường dẫn thư mục đã chọn
                projectDirectory = folderBrowserDialog1.SelectedPath;
                folderPathLabel.Text = projectDirectory;

                // Kiểm tra xem thư mục có chứa thư mục .git không
                string gitFolder = Path.Combine(projectDirectory, ".git");
                if (!Directory.Exists(gitFolder))
                {
                    AppendTextWithScroll("Lỗi: Thư mục được chọn không chứa repository Git hợp lệ. Vui lòng chọn lại.\n");
                    return;
                }
                // Kiểm tra xem repository có commit nào không
                string logFilePath = Path.Combine(projectDirectory, "git_log_output.txt");
                bll.RunGitCommand("log --oneline", logFilePath, projectDirectory);

                // Đọc nội dung file log để kiểm tra
                string logOutput = File.Exists(logFilePath) ? File.ReadAllText(logFilePath) : string.Empty;
                File.Delete(logFilePath);

                // Xóa file log sau khi đọc
                if (string.IsNullOrEmpty(logOutput))
                {
                    AppendTextWithScroll("Lỗi: Repository Git này không chứa bất kỳ commit nào. Vui lòng chọn một repository khác hoặc tạo commit đầu tiên.\n");
                    return;
                }

                // Thông báo file hướng dẫn trong thư mục Cài đặt
                AppendTextWithScroll("File hướng dẫn đã có trong thư mục cài đặt tên là ManualUsage.docx.\n");

                string internshipWeekFolder = Path.Combine(projectDirectory, "internship_week");
                // Tạo thư mục 'internship_week' nếu chưa tồn tại
                if (!Directory.Exists(internshipWeekFolder))
                {
                    Directory.CreateDirectory(internshipWeekFolder);
                }

                // Gắn đường dẫn lên textbox danh mục 8 tuần
                folderPathInternWeekLabel.Text = internshipWeekFolder;

                // Tải danh sách tác giả
                LoadAuthorsCombobox(projectDirectory);

                // Kiểm tra thông tin tổng hợp trước đó
                string configFile = Path.Combine(internshipWeekFolder, "config.txt");
                try
                {
                    if (File.Exists(configFile))
                    {
                        // Load aggregate information
                        AggregateInfo aggregateInfo = bll.LoadAggregateInfo(configFile);

                        // Update UI controls
                        authorsComboBox.SelectedItem = aggregateInfo.Author;
                        startDatePicker.Value = aggregateInfo.StartDate;
                        folderPathLabel.Text = aggregateInfo.ProjectDirectory;

                        DisplayFoldersInListView(aggregateInfo.Folders);
                        AppendTextWithScroll($"Tải dữ liệu tổng hợp trước đó:\nTác giả: {aggregateInfo.Author}\nNgày bắt đầu: {aggregateInfo.StartDate:dd/MM/yyyy}\n");

                        // Disable/Enable UI elements
                        aggregateButton.Enabled = false;
                        deleteFolderButton.Enabled = true;
                        selectGitFolderButton.Enabled = false;
                    }
                    else
                    {
                        // Enable controls for new aggregation
                        authorsComboBox.Enabled = true;
                        startDatePicker.Enabled = true;
                        aggregateButton.Enabled = true;
                        deleteFolderButton.Enabled = false;
                        selectGitFolderButton.Enabled = false; // Prevent selecting a different folder
                    }
                }
                catch (Exception ex)
                {
                    // Handle exceptions, e.g., show an error message to the user
                    MessageBox.Show($"An error occurred: {ex.Message}");
                }
            }
        }
        /// <summary>
        /// Hiển thị danh sách tác giả trên combobox
        /// </summary>
        private void LoadAuthorsCombobox(string projectDirectory)
        {
            try
            {
                List<string> authors = bll.GetGitAuthors(projectDirectory);
                authorsComboBox.DataSource = authors;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading authors: " + ex.Message);
            }
        }
        /// <summary>
        /// Duyệt qua 8 tuần kể từ ngày bắt đầu thực tập.
        /// Tạo thư mục theo từng tuần và file log commit hằng ngày.
        /// Chạy lệnh Git để lấy commit của từng ngày
        /// Xuất báo cáo commit vào file và hiển thị kết quả trong giao diện
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void aggregateButton_Click(object sender, EventArgs e)
        {
            // Kiểm tra xem thư mục dự án đã được chọn chưa
            if (string.IsNullOrEmpty(projectDirectory))
            {
                AppendTextWithScroll("Vui lòng chọn thư mục dự án trước khi tổng hợp commit.\n");
                return;
            }

            // Kiểm tra trạng thái của chương trình, nếu đang chạy thì không cho nhấn
            if (isProcessing)
            {
                AppendTextWithScroll("Chương trình đang xử lý, vui lòng chờ...\n");
                return;
            }

            try
            {
                // Đánh dấu trạng thái xử lý
                isProcessing = true;

                // Tạo thư mục internship_week nếu chưa tồn tại
                string internshipWeekFolder = Path.Combine(projectDirectory, "internship_week");
                if (!Directory.Exists(internshipWeekFolder))
                {
                    Directory.CreateDirectory(internshipWeekFolder);
                }

                // Lấy thông tin từ người dùng
                string author = authorsComboBox.SelectedItem.ToString();
                DateTime internshipStartDate = startDatePicker.Value;
                DateTime projectStartDate = bll.GetProjectStartDate(projectDirectory);
                int startingWeek = bll.CalculateWeekNumber(internshipStartDate, projectStartDate);

                List<string> folders = new List<string>();

                // Lặp qua từng tuần
                for (int weekOffset = 0; weekOffset < 8; weekOffset++)
                {
                    DateTime currentWeekStart = internshipStartDate.AddDays(weekOffset * 7);
                    int currentWeek = startingWeek + weekOffset;
                    string weekFolder = Path.Combine(internshipWeekFolder, "Week_" + currentWeek);
                    string combinedFile = Path.Combine(weekFolder, "combined_commits.txt");

                    // Tạo thư mục tuần nếu chưa tồn tại
                    Directory.CreateDirectory(weekFolder);

                    // Xóa file cũ nếu đã tồn tại
                    if (File.Exists(combinedFile))
                    {
                        File.Delete(combinedFile);
                    }

                    // Duyệt từng ngày trong tuần
                    string[] days = { "monday", "tuesday", "wednesday", "thursday", "friday", "saturday", "sunday" };
                    foreach (string day in days)
                    {
                        string dailyFile = Path.Combine(weekFolder, $"{day}_commits.txt");

                        string since = $"{currentWeekStart:yyyy-MM-dd} 00:00";
                        string until = $"{currentWeekStart:yyyy-MM-dd} 23:59";
                        string gitLogCommand = $"log --author=\"{author}\" --since=\"{since}\" --until=\"{until}\"";

                        // Chạy lệnh Git và ghi kết quả
                        bll.RunGitCommand(gitLogCommand, dailyFile, projectDirectory);

                        if (File.Exists(dailyFile))
                        {
                            // Đọc và tổng hợp dữ liệu
                            using (StreamReader reader = new StreamReader(dailyFile))
                            using (StreamWriter writer = new StreamWriter(combinedFile, true))
                            {
                                writer.Write(reader.ReadToEnd());
                                writer.WriteLine();
                            }
                        }
                        else
                        {
                            throw new FileNotFoundException($"Không thể tìm thấy file {dailyFile}");
                        }

                        currentWeekStart = currentWeekStart.AddDays(1);
                    }

                    // Thêm thư mục tuần vào danh sách
                    folders.Add(weekFolder);

                    // Thông báo hoàn tất tuần
                    AppendTextWithScroll($"Week {currentWeek} commits đã tổng hợp vào: {combinedFile}\n");
                }

                // Lưu thông tin tổng hợp
                AggregateInfo aggregateInfo = new AggregateInfo
                {
                    Author = author,
                    StartDate = internshipStartDate,
                    Folders = folders,
                    ProjectDirectory = projectDirectory
                };
                bll.SaveAggregateInfo(aggregateInfo);

                // Cập nhật giao diện sau khi hoàn tất
                DisplayDirectoriesInListView();
                AppendTextWithScroll("Đã tổng hợp tất cả commit.\n");

                // Đổi trạng thái nút
                aggregateButton.Enabled = false;
                deleteFolderButton.Enabled = true;
            }
            catch (Exception ex)
            {
                AppendTextWithScroll($"Lỗi: {ex.Message}\n");
            }
            finally
            {
                isProcessing = false; // Luôn cập nhật trạng thái xử lý
            }
        }

        /// <summary>
        /// hiển thị danh sách thư mục trong internship_week
        /// </summary>
        private void EnableControls()
        {
            authorsComboBox.Enabled = true;
            startDatePicker.Enabled = true;
            aggregateButton.Enabled = true;
        }
        /// <summary>
        /// Xóa thư mục: Nút xóa sẽ chỉ xóa những thư mục hoặc file không cần thiết
        /// mà không cần phải tạo lại những thư mục con như internship_week hoặc các thư mục tuần.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void deleteFolderButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(projectDirectory))
            {
                AppendTextWithScroll("Vui lòng chọn thư mục dự án trước khi xóa.");
                return;
            }

            string internshipWeekFolder = Path.Combine(projectDirectory, "internship_week");

            if (Directory.Exists(internshipWeekFolder))
            {
                // Xóa tất cả thư mục và tệp trong thư mục internship_week
                Directory.Delete(internshipWeekFolder, true);  // true để xóa tất cả các file và thư mục con
                AppendTextWithScroll($"Đã xóa thư mục: {internshipWeekFolder}\n");
                folderPathInternWeekLabel.Text = string.Empty;

                // Cập nhật trạng thái nút
                deleteFolderButton.Enabled = false;  // Vô hiệu hóa nút xóa
                AppendTextWithScroll("Nút xóa đã bị vô hiệu hóa sau khi xóa thư mục.\n");

                aggregateButton.Enabled = true; // Bật lại nút tổng hợp commit

                // Làm trống weekListView sau khi xóa thư mục
                weekListView.Items.Clear();  // Xóa tất cả mục trong ListView
                AppendTextWithScroll("Danh sách thư mục đã được làm trống.\n");

                // Làm trống fileListView sau khi xóa thư mục
                fileListView.Items.Clear();  // Xóa tất cả mục trong ListView
                AppendTextWithScroll("Danh sách file đã được làm trống.\n");
            }
            else
            {
                AppendTextWithScroll("Thư mục internship_week không tồn tại.");
            }
        }
        /// <summary>
        /// Hiển thị thư mục khi load thư mục project
        /// </summary>
        /// <param name="folders"></param>
        private void DisplayFoldersInListView(List<string> folders)
        {
            weekListView.Items.Clear();
            ImageList imageList = new ImageList();
            imageList.ImageSize = new Size(32, 32);
            imageList.Images.Add("folder", Properties.Resources.Git_commit_aggregation_tool); // Icon folder

            weekListView.SmallImageList = imageList;

            foreach (var folder in folders)
            {
                // Lấy tên thư mục từ đường dẫn
                string folderName = Path.GetFileName(folder);

                ListViewItem item = new ListViewItem(folderName);
                item.ImageKey = "folder";
                item.Tag = folder; // Lưu đường dẫn đầy đủ của thư mục vào thuộc tính Tag của ListViewItem
                weekListView.Items.Add(item);
            }
        }
        /// <summary>
        /// Hiển thị danh sách thư mục thực tập
        /// </summary>
        private void DisplayDirectoriesInListView()
        {
            if (string.IsNullOrEmpty(projectDirectory))
            {
                AppendTextWithScroll("Chưa chọn thư mục dự án.");
                return;
            }

            // Xác định đường dẫn thư mục internship_week
            string internshipWeekFolder = Path.Combine(projectDirectory, "internship_week");

            // Kiểm tra xem thư mục internship_week có tồn tại không
            if (!Directory.Exists(internshipWeekFolder))
            {
                AppendTextWithScroll("Thư mục internship_week không tồn tại.");
                return;
            }

            // Khởi tạo ImageList
            ImageList imageList = new ImageList();
            imageList.ImageSize = new Size(32, 32);
            imageList.Images.Add("folder", Properties.Resources.Git_commit_aggregation_tool);
            weekListView.SmallImageList = imageList;

            // Lấy danh sách thư mục trong thư mục internship_week
            string[] directories = Directory.GetDirectories(internshipWeekFolder);

            // Thêm các thư mục vào ListView
            weekListView.Items.Clear();  // Xóa tất cả các mục hiện có
            foreach (string directory in directories)
            {
                ListViewItem item = new ListViewItem(Path.GetFileName(directory));
                item.ImageKey = "folder";  // Sử dụng icon thư mục
                item.Tag = directory; // Lưu đường dẫn đầy đủ của thư mục vào thuộc tính Tag của ListViewItem
                weekListView.Items.Add(item);
            }

            // Hiển thị thông báo nếu không có thư mục nào trong internship_week
            if (directories.Length == 0)
            {
                AppendTextWithScroll("Không có thư mục nào trong internship_week.");
            }
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

                foreach (var file in files)
                {
                    ListViewItem item = new ListViewItem(Path.GetFileName(file));
                    item.Tag = file; // Lưu đường dẫn đầy đủ của file vào thuộc tính Tag của ListViewItem
                    fileListView.Items.Add(item);
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
            resultRichTextBox.AppendText(text);
            resultRichTextBox.ScrollToCaret();
        }
        /// <summary>
        /// xem hướng dẫn thực hiện
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void helpButton_Click(object sender, EventArgs e)
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
        private void fileListView_MouseDoubleClick(object sender, MouseEventArgs e)
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
        private void weekListView_MouseClick(object sender, MouseEventArgs e)
        {
            if (weekListView.SelectedItems.Count > 0)
            {
                ListViewItem selectedItem = weekListView.SelectedItems[0];

                // Kiểm tra null trước khi sử dụng thuộc tính Tag
                if (selectedItem.Tag != null)
                {
                    string directoryPath = selectedItem.Tag.ToString();
                    DisplayFilesInDirectory(directoryPath);
                    AppendTextWithScroll($"{directoryPath}.\n");
                }
                else
                {
                    AppendTextWithScroll("Đường dẫn thư mục không tồn tại.\n");
                }
            }
        }

    }
}