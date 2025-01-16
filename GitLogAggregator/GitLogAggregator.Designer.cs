namespace GitLogAggregator
{
    partial class GitLogAggregator
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GitLogAggregator));
            this.cboAuthorCommit = new System.Windows.Forms.ComboBox();
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.btnOpenGitFolder = new System.Windows.Forms.Button();
            this.txtInternshipStartDate = new System.Windows.Forms.DateTimePicker();
            this.txtResult = new System.Windows.Forms.RichTextBox();
            this.btnAggregator = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.weekListView = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.btnClearDataListView = new System.Windows.Forms.Button();
            this.helpButton = new System.Windows.Forms.Button();
            this.setupThuMucThucTap = new System.Windows.Forms.PictureBox();
            this.fileListView = new System.Windows.Forms.ListView();
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.dataGridViewCommits = new System.Windows.Forms.DataGridView();
            this.label8 = new System.Windows.Forms.Label();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.txtFirstCommitDate = new System.Windows.Forms.DateTimePicker();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.txtInternshipEndDate = new System.Windows.Forms.DateTimePicker();
            this.txtNumericsWeek = new System.Windows.Forms.NumericUpDown();
            this.checkedListBoxCommits = new System.Windows.Forms.CheckedListBox();
            this.btnExcelCommits = new System.Windows.Forms.Button();
            this.btnReviewCommits = new System.Windows.Forms.Button();
            this.listViewProjects = new System.Windows.Forms.ListView();
            this.btnExpanDataGridview = new System.Windows.Forms.Button();
            this.btnDeleteCommits = new System.Windows.Forms.Button();
            this.txtResultMouseEvents = new System.Windows.Forms.RichTextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.cboThuMucThucTap = new System.Windows.Forms.ComboBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel9 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel6 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel7 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel8 = new System.Windows.Forms.TableLayoutPanel();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.danhMụcToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.commitsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.weeksToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.foldersProjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.chatbotSummaryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cònToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tableLayoutPanel10 = new System.Windows.Forms.TableLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)(this.setupThuMucThucTap)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewCommits)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNumericsWeek)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel9.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            this.tableLayoutPanel6.SuspendLayout();
            this.tableLayoutPanel7.SuspendLayout();
            this.tableLayoutPanel8.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.tableLayoutPanel10.SuspendLayout();
            this.SuspendLayout();
            // 
            // cboAuthorCommit
            // 
            this.cboAuthorCommit.FormattingEnabled = true;
            this.cboAuthorCommit.Location = new System.Drawing.Point(120, 69);
            this.cboAuthorCommit.Name = "cboAuthorCommit";
            this.cboAuthorCommit.Size = new System.Drawing.Size(132, 25);
            this.cboAuthorCommit.TabIndex = 0;
            // 
            // btnOpenGitFolder
            // 
            this.btnOpenGitFolder.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnOpenGitFolder.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnOpenGitFolder.Location = new System.Drawing.Point(3, 3);
            this.btnOpenGitFolder.Name = "btnOpenGitFolder";
            this.btnOpenGitFolder.Size = new System.Drawing.Size(178, 29);
            this.btnOpenGitFolder.TabIndex = 1;
            this.btnOpenGitFolder.Text = "Chọn dự án";
            this.btnOpenGitFolder.UseVisualStyleBackColor = true;
            this.btnOpenGitFolder.Click += new System.EventHandler(this.BtnAddProject_Click);
            // 
            // txtInternshipStartDate
            // 
            this.txtInternshipStartDate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtInternshipStartDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.txtInternshipStartDate.Location = new System.Drawing.Point(3, 38);
            this.txtInternshipStartDate.Name = "txtInternshipStartDate";
            this.txtInternshipStartDate.Size = new System.Drawing.Size(174, 25);
            this.txtInternshipStartDate.TabIndex = 3;
            this.txtInternshipStartDate.ValueChanged += new System.EventHandler(this.NumericWeeks_ValueChanged);
            // 
            // txtResult
            // 
            this.txtResult.BackColor = System.Drawing.Color.Gainsboro;
            this.txtResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtResult.ForeColor = System.Drawing.Color.IndianRed;
            this.txtResult.Location = new System.Drawing.Point(3, 26);
            this.txtResult.Name = "txtResult";
            this.txtResult.ReadOnly = true;
            this.txtResult.Size = new System.Drawing.Size(542, 82);
            this.txtResult.TabIndex = 4;
            this.txtResult.Text = "";
            // 
            // btnAggregator
            // 
            this.btnAggregator.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAggregator.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnAggregator.Location = new System.Drawing.Point(371, 3);
            this.btnAggregator.Name = "btnAggregator";
            this.btnAggregator.Size = new System.Drawing.Size(180, 29);
            this.btnAggregator.TabIndex = 5;
            this.btnAggregator.Text = "Tổng hợp commit";
            this.btnAggregator.UseVisualStyleBackColor = true;
            this.btnAggregator.Click += new System.EventHandler(this.BtnAggregateCommits_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanel9.SetColumnSpan(this.label1, 2);
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(4, 0);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(415, 33);
            this.label1.TabIndex = 6;
            this.label1.Text = "GitLogAggregator";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 69);
            this.label2.Margin = new System.Windows.Forms.Padding(3);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(108, 17);
            this.label2.TabIndex = 7;
            this.label2.Text = "Nhập tên tác giả:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Location = new System.Drawing.Point(3, 3);
            this.label3.Margin = new System.Windows.Forms.Padding(3);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(174, 29);
            this.label3.TabIndex = 8;
            this.label3.Text = "Ngày bắt đầu TT:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // weekListView
            // 
            this.weekListView.BackColor = System.Drawing.Color.WhiteSmoke;
            this.weekListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.weekListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.weekListView.HideSelection = false;
            this.weekListView.Location = new System.Drawing.Point(3, 33);
            this.weekListView.Name = "weekListView";
            this.weekListView.Size = new System.Drawing.Size(170, 187);
            this.weekListView.TabIndex = 12;
            this.weekListView.UseCompatibleStateImageBehavior = false;
            this.weekListView.View = System.Windows.Forms.View.Details;
            this.weekListView.MouseClick += new System.Windows.Forms.MouseEventHandler(this.WeekListView_MouseClick);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Tên thư mục";
            this.columnHeader1.Width = 259;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label6.Location = new System.Drawing.Point(3, 3);
            this.label6.Margin = new System.Windows.Forms.Padding(3);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(542, 17);
            this.label6.TabIndex = 11;
            this.label6.Text = "Hiển thị thông báo:";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.tableLayoutPanel3.SetColumnSpan(this.label7, 2);
            this.label7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label7.Location = new System.Drawing.Point(3, 3);
            this.label7.Margin = new System.Windows.Forms.Padding(3);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(548, 24);
            this.label7.TabIndex = 9;
            this.label7.Text = "Danh mục tuần thực tập";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnClearDataListView
            // 
            this.btnClearDataListView.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnClearDataListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnClearDataListView.Location = new System.Drawing.Point(187, 3);
            this.btnClearDataListView.Name = "btnClearDataListView";
            this.btnClearDataListView.Size = new System.Drawing.Size(178, 29);
            this.btnClearDataListView.TabIndex = 14;
            this.btnClearDataListView.Text = "Xóa dữ liệu";
            this.btnClearDataListView.UseVisualStyleBackColor = true;
            this.btnClearDataListView.Click += new System.EventHandler(this.BtnClearDataListView_Click);
            // 
            // helpButton
            // 
            this.helpButton.BackColor = System.Drawing.Color.DeepSkyBlue;
            this.helpButton.Cursor = System.Windows.Forms.Cursors.Help;
            this.helpButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.helpButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.helpButton.Font = new System.Drawing.Font("Microsoft YaHei", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.helpButton.ForeColor = System.Drawing.Color.White;
            this.helpButton.Location = new System.Drawing.Point(0, 0);
            this.helpButton.Margin = new System.Windows.Forms.Padding(0);
            this.helpButton.Name = "helpButton";
            this.helpButton.Size = new System.Drawing.Size(20, 106);
            this.helpButton.TabIndex = 15;
            this.helpButton.Text = "i";
            this.helpButton.UseVisualStyleBackColor = false;
            this.helpButton.Click += new System.EventHandler(this.HelpButton_Click);
            // 
            // setupThuMucThucTap
            // 
            this.setupThuMucThucTap.BackColor = System.Drawing.Color.Transparent;
            this.setupThuMucThucTap.Cursor = System.Windows.Forms.Cursors.Hand;
            this.setupThuMucThucTap.Dock = System.Windows.Forms.DockStyle.Fill;
            this.setupThuMucThucTap.Image = ((System.Drawing.Image)(resources.GetObject("setupThuMucThucTap.Image")));
            this.setupThuMucThucTap.Location = new System.Drawing.Point(23, 3);
            this.setupThuMucThucTap.Name = "setupThuMucThucTap";
            this.setupThuMucThucTap.Size = new System.Drawing.Size(99, 100);
            this.setupThuMucThucTap.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.setupThuMucThucTap.TabIndex = 16;
            this.setupThuMucThucTap.TabStop = false;
            this.setupThuMucThucTap.Click += new System.EventHandler(this.SetupThuMucThucTap_Click);
            // 
            // fileListView
            // 
            this.fileListView.BackColor = System.Drawing.Color.WhiteSmoke;
            this.fileListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader5});
            this.fileListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fileListView.HideSelection = false;
            this.fileListView.Location = new System.Drawing.Point(179, 33);
            this.fileListView.Name = "fileListView";
            this.fileListView.Size = new System.Drawing.Size(372, 187);
            this.fileListView.TabIndex = 12;
            this.fileListView.UseCompatibleStateImageBehavior = false;
            this.fileListView.View = System.Windows.Forms.View.Details;
            this.fileListView.MouseClick += new System.Windows.Forms.MouseEventHandler(this.FileListView_MouseClick);
            this.fileListView.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.FileListView_MouseDoubleClick);
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "STT";
            this.columnHeader3.Width = 40;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Tên File";
            this.columnHeader4.Width = 120;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "Ngày Tạo";
            this.columnHeader5.Width = 100;
            // 
            // dataGridViewCommits
            // 
            this.dataGridViewCommits.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.dataGridViewCommits.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dataGridViewCommits.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewCommits.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewCommits.Location = new System.Drawing.Point(3, 101);
            this.dataGridViewCommits.Name = "dataGridViewCommits";
            this.dataGridViewCommits.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            this.dataGridViewCommits.Size = new System.Drawing.Size(429, 327);
            this.dataGridViewCommits.TabIndex = 18;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label8.Location = new System.Drawing.Point(371, 3);
            this.label8.Margin = new System.Windows.Forms.Padding(3);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(180, 29);
            this.label8.TabIndex = 9;
            this.label8.Text = "Số ngày TT:";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // imageList
            // 
            this.imageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageList.ImageSize = new System.Drawing.Size(16, 16);
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // txtFirstCommitDate
            // 
            this.tableLayoutPanel4.SetColumnSpan(this.txtFirstCommitDate, 2);
            this.txtFirstCommitDate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtFirstCommitDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.txtFirstCommitDate.Location = new System.Drawing.Point(3, 34);
            this.txtFirstCommitDate.Name = "txtFirstCommitDate";
            this.txtFirstCommitDate.Size = new System.Drawing.Size(442, 25);
            this.txtFirstCommitDate.TabIndex = 3;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.tableLayoutPanel4.SetColumnSpan(this.label9, 2);
            this.label9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label9.Location = new System.Drawing.Point(3, 3);
            this.label9.Margin = new System.Windows.Forms.Padding(3);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(442, 25);
            this.label9.TabIndex = 8;
            this.label9.Text = "Ngày commit đầu tiên";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label10.Location = new System.Drawing.Point(183, 3);
            this.label10.Margin = new System.Windows.Forms.Padding(3);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(182, 29);
            this.label10.TabIndex = 8;
            this.label10.Text = "Ngày kết thúc TT:";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtInternshipEndDate
            // 
            this.txtInternshipEndDate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtInternshipEndDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.txtInternshipEndDate.Location = new System.Drawing.Point(183, 38);
            this.txtInternshipEndDate.Name = "txtInternshipEndDate";
            this.txtInternshipEndDate.Size = new System.Drawing.Size(182, 25);
            this.txtInternshipEndDate.TabIndex = 3;
            // 
            // txtNumericsWeek
            // 
            this.txtNumericsWeek.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtNumericsWeek.Location = new System.Drawing.Point(371, 38);
            this.txtNumericsWeek.Name = "txtNumericsWeek";
            this.txtNumericsWeek.Size = new System.Drawing.Size(180, 25);
            this.txtNumericsWeek.TabIndex = 20;
            this.txtNumericsWeek.Value = new decimal(new int[] {
            8,
            0,
            0,
            0});
            this.txtNumericsWeek.ValueChanged += new System.EventHandler(this.NumericWeeks_ValueChanged);
            // 
            // checkedListBoxCommits
            // 
            this.checkedListBoxCommits.BackColor = System.Drawing.Color.WhiteSmoke;
            this.checkedListBoxCommits.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkedListBoxCommits.FormattingEnabled = true;
            this.checkedListBoxCommits.Items.AddRange(new object[] {
            "Danh sách commit phát hiện lỗi"});
            this.checkedListBoxCommits.Location = new System.Drawing.Point(3, 3);
            this.checkedListBoxCommits.Name = "checkedListBoxCommits";
            this.checkedListBoxCommits.Size = new System.Drawing.Size(429, 92);
            this.checkedListBoxCommits.TabIndex = 23;
            // 
            // btnExcelCommits
            // 
            this.btnExcelCommits.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnExcelCommits.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnExcelCommits.Location = new System.Drawing.Point(227, 63);
            this.btnExcelCommits.Name = "btnExcelCommits";
            this.btnExcelCommits.Size = new System.Drawing.Size(218, 40);
            this.btnExcelCommits.TabIndex = 5;
            this.btnExcelCommits.Text = "Xuất excel";
            this.btnExcelCommits.UseVisualStyleBackColor = true;
            this.btnExcelCommits.Click += new System.EventHandler(this.BtnExportExcel_Click);
            this.btnExcelCommits.MouseEnter += new System.EventHandler(this.btnExcelCommits_MouseEnter);
            // 
            // btnReviewCommits
            // 
            this.btnReviewCommits.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnReviewCommits.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnReviewCommits.Location = new System.Drawing.Point(3, 63);
            this.btnReviewCommits.Name = "btnReviewCommits";
            this.btnReviewCommits.Size = new System.Drawing.Size(218, 40);
            this.btnReviewCommits.TabIndex = 5;
            this.btnReviewCommits.Text = "Kiểm tra";
            this.btnReviewCommits.UseVisualStyleBackColor = true;
            this.btnReviewCommits.Click += new System.EventHandler(this.BtnReviewCommits_Click);
            this.btnReviewCommits.MouseEnter += new System.EventHandler(this.btnReviewCommits_MouseEnter);
            // 
            // listViewProjects
            // 
            this.listViewProjects.BackColor = System.Drawing.Color.WhiteSmoke;
            this.tableLayoutPanel8.SetColumnSpan(this.listViewProjects, 3);
            this.listViewProjects.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewProjects.HideSelection = false;
            this.listViewProjects.Location = new System.Drawing.Point(3, 38);
            this.listViewProjects.Name = "listViewProjects";
            this.listViewProjects.Size = new System.Drawing.Size(548, 96);
            this.listViewProjects.TabIndex = 24;
            this.listViewProjects.UseCompatibleStateImageBehavior = false;
            this.listViewProjects.View = System.Windows.Forms.View.Details;
            this.listViewProjects.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.ListViewProjects_ItemSelectionChanged);
            // 
            // btnExpanDataGridview
            // 
            this.btnExpanDataGridview.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btnExpanDataGridview.Cursor = System.Windows.Forms.Cursors.SizeWE;
            this.btnExpanDataGridview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnExpanDataGridview.FlatAppearance.BorderSize = 0;
            this.btnExpanDataGridview.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExpanDataGridview.Location = new System.Drawing.Point(436, 99);
            this.btnExpanDataGridview.Margin = new System.Windows.Forms.Padding(1);
            this.btnExpanDataGridview.Name = "btnExpanDataGridview";
            this.btnExpanDataGridview.Size = new System.Drawing.Size(11, 331);
            this.btnExpanDataGridview.TabIndex = 25;
            this.btnExpanDataGridview.UseVisualStyleBackColor = false;
            this.btnExpanDataGridview.MouseEnter += new System.EventHandler(this.btnExpanDataGridview_MouseEnter);
            this.btnExpanDataGridview.MouseLeave += new System.EventHandler(this.btnExpanDataGridview_MouseLeave);
            // 
            // btnDeleteCommits
            // 
            this.btnDeleteCommits.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.btnDeleteCommits.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDeleteCommits.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnDeleteCommits.FlatAppearance.BorderSize = 0;
            this.btnDeleteCommits.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDeleteCommits.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDeleteCommits.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btnDeleteCommits.Location = new System.Drawing.Point(436, 1);
            this.btnDeleteCommits.Margin = new System.Windows.Forms.Padding(1);
            this.btnDeleteCommits.Name = "btnDeleteCommits";
            this.btnDeleteCommits.Size = new System.Drawing.Size(11, 96);
            this.btnDeleteCommits.TabIndex = 25;
            this.btnDeleteCommits.Text = "Xóa";
            this.btnDeleteCommits.UseVisualStyleBackColor = false;
            this.btnDeleteCommits.Click += new System.EventHandler(this.BtnDeleteCommits_Click);
            this.btnDeleteCommits.MouseEnter += new System.EventHandler(this.OnMouseEnter);
            this.btnDeleteCommits.MouseLeave += new System.EventHandler(this.OnMouseLeave);
            // 
            // txtResultMouseEvents
            // 
            this.txtResultMouseEvents.BackColor = System.Drawing.Color.Gainsboro;
            this.txtResultMouseEvents.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtResultMouseEvents.ForeColor = System.Drawing.Color.IndianRed;
            this.txtResultMouseEvents.Location = new System.Drawing.Point(3, 26);
            this.txtResultMouseEvents.Name = "txtResultMouseEvents";
            this.txtResultMouseEvents.ReadOnly = true;
            this.txtResultMouseEvents.Size = new System.Drawing.Size(436, 82);
            this.txtResultMouseEvents.TabIndex = 4;
            this.txtResultMouseEvents.Text = "";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label4.Location = new System.Drawing.Point(3, 3);
            this.label4.Margin = new System.Windows.Forms.Padding(3);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(436, 17);
            this.label4.TabIndex = 11;
            this.label4.Text = "Sự kiện rê chuột khu vực commit:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(3, 36);
            this.label5.Margin = new System.Windows.Forms.Padding(3);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(91, 27);
            this.label5.TabIndex = 7;
            this.label5.Text = "Thư mục thực tập:";
            // 
            // cboThuMucThucTap
            // 
            this.cboThuMucThucTap.FormattingEnabled = true;
            this.cboThuMucThucTap.Location = new System.Drawing.Point(120, 36);
            this.cboThuMucThucTap.Name = "cboThuMucThucTap";
            this.cboThuMucThucTap.Size = new System.Drawing.Size(132, 25);
            this.cboThuMucThucTap.TabIndex = 0;
            this.cboThuMucThucTap.SelectedIndexChanged += new System.EventHandler(this.cboThuMucThucTap_SelectedIndexChanged);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 19.78221F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 80.21779F));
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel9, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.helpButton, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.setupThuMucThucTap, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(554, 106);
            this.tableLayoutPanel1.TabIndex = 26;
            // 
            // tableLayoutPanel9
            // 
            this.tableLayoutPanel9.ColumnCount = 2;
            this.tableLayoutPanel9.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 27.68879F));
            this.tableLayoutPanel9.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 72.31121F));
            this.tableLayoutPanel9.Controls.Add(this.label2, 0, 2);
            this.tableLayoutPanel9.Controls.Add(this.label5, 0, 1);
            this.tableLayoutPanel9.Controls.Add(this.cboAuthorCommit, 1, 2);
            this.tableLayoutPanel9.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel9.Controls.Add(this.cboThuMucThucTap, 1, 1);
            this.tableLayoutPanel9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel9.Location = new System.Drawing.Point(128, 3);
            this.tableLayoutPanel9.Name = "tableLayoutPanel9";
            this.tableLayoutPanel9.RowCount = 3;
            this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel9.Size = new System.Drawing.Size(423, 100);
            this.tableLayoutPanel9.TabIndex = 17;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 3;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 32.63403F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 34.03264F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.Controls.Add(this.label3, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.label10, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.label8, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.txtInternshipStartDate, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.txtInternshipEndDate, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.txtNumericsWeek, 2, 1);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 106);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(554, 71);
            this.tableLayoutPanel2.TabIndex = 27;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 31.89189F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 68.10811F));
            this.tableLayoutPanel3.Controls.Add(this.weekListView, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.fileListView, 1, 1);
            this.tableLayoutPanel3.Controls.Add(this.label7, 0, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(0, 177);
            this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 2;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(554, 223);
            this.tableLayoutPanel3.TabIndex = 28;
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 2;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.Controls.Add(this.txtFirstCommitDate, 0, 1);
            this.tableLayoutPanel4.Controls.Add(this.btnReviewCommits, 0, 2);
            this.tableLayoutPanel4.Controls.Add(this.btnExcelCommits, 1, 2);
            this.tableLayoutPanel4.Controls.Add(this.label9, 0, 0);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(554, 0);
            this.tableLayoutPanel4.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 3;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 29.24528F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 27.35849F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 42.45283F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(448, 106);
            this.tableLayoutPanel4.TabIndex = 29;
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.ColumnCount = 2;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 13F));
            this.tableLayoutPanel5.Controls.Add(this.checkedListBoxCommits, 0, 0);
            this.tableLayoutPanel5.Controls.Add(this.btnExpanDataGridview, 1, 1);
            this.tableLayoutPanel5.Controls.Add(this.btnDeleteCommits, 1, 0);
            this.tableLayoutPanel5.Controls.Add(this.dataGridViewCommits, 0, 1);
            this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel5.Location = new System.Drawing.Point(554, 106);
            this.tableLayoutPanel5.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 2;
            this.tableLayoutPanel7.SetRowSpan(this.tableLayoutPanel5, 3);
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 22.90249F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 77.0975F));
            this.tableLayoutPanel5.Size = new System.Drawing.Size(448, 431);
            this.tableLayoutPanel5.TabIndex = 30;
            // 
            // tableLayoutPanel6
            // 
            this.tableLayoutPanel6.ColumnCount = 1;
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel6.Controls.Add(this.txtResultMouseEvents, 1, 1);
            this.tableLayoutPanel6.Controls.Add(this.label4, 0, 0);
            this.tableLayoutPanel6.Location = new System.Drawing.Point(557, 540);
            this.tableLayoutPanel6.Name = "tableLayoutPanel6";
            this.tableLayoutPanel6.RowCount = 1;
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel6.Size = new System.Drawing.Size(442, 111);
            this.tableLayoutPanel6.TabIndex = 31;
            // 
            // tableLayoutPanel7
            // 
            this.tableLayoutPanel7.ColumnCount = 2;
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 55.28942F));
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 44.71058F));
            this.tableLayoutPanel7.Controls.Add(this.tableLayoutPanel8, 0, 3);
            this.tableLayoutPanel7.Controls.Add(this.tableLayoutPanel1, 0, 0);
            this.tableLayoutPanel7.Controls.Add(this.tableLayoutPanel4, 1, 0);
            this.tableLayoutPanel7.Controls.Add(this.tableLayoutPanel3, 0, 2);
            this.tableLayoutPanel7.Controls.Add(this.tableLayoutPanel2, 0, 1);
            this.tableLayoutPanel7.Controls.Add(this.tableLayoutPanel5, 1, 1);
            this.tableLayoutPanel7.Controls.Add(this.tableLayoutPanel6, 1, 4);
            this.tableLayoutPanel7.Controls.Add(this.tableLayoutPanel10, 0, 4);
            this.tableLayoutPanel7.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tableLayoutPanel7.Location = new System.Drawing.Point(0, 57);
            this.tableLayoutPanel7.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel7.Name = "tableLayoutPanel7";
            this.tableLayoutPanel7.RowCount = 5;
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 71F));
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 223F));
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 137F));
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 117F));
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel7.Size = new System.Drawing.Size(1002, 654);
            this.tableLayoutPanel7.TabIndex = 32;
            // 
            // tableLayoutPanel8
            // 
            this.tableLayoutPanel8.ColumnCount = 3;
            this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel8.Controls.Add(this.btnOpenGitFolder, 0, 0);
            this.tableLayoutPanel8.Controls.Add(this.btnClearDataListView, 1, 0);
            this.tableLayoutPanel8.Controls.Add(this.btnAggregator, 2, 0);
            this.tableLayoutPanel8.Controls.Add(this.listViewProjects, 0, 1);
            this.tableLayoutPanel8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel8.Location = new System.Drawing.Point(0, 400);
            this.tableLayoutPanel8.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel8.Name = "tableLayoutPanel8";
            this.tableLayoutPanel8.RowCount = 2;
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 26.22951F));
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 73.77049F));
            this.tableLayoutPanel8.Size = new System.Drawing.Size(554, 137);
            this.tableLayoutPanel8.TabIndex = 33;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(644, 24);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(358, 25);
            this.textBox1.TabIndex = 33;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.danhMụcToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1002, 24);
            this.menuStrip1.TabIndex = 35;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // danhMụcToolStripMenuItem
            // 
            this.danhMụcToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.commitsToolStripMenuItem,
            this.weeksToolStripMenuItem,
            this.foldersProjectToolStripMenuItem,
            this.chatbotSummaryToolStripMenuItem,
            this.cònToolStripMenuItem});
            this.danhMụcToolStripMenuItem.Name = "danhMụcToolStripMenuItem";
            this.danhMụcToolStripMenuItem.Size = new System.Drawing.Size(74, 20);
            this.danhMụcToolStripMenuItem.Text = "Danh mục";
            // 
            // commitsToolStripMenuItem
            // 
            this.commitsToolStripMenuItem.Name = "commitsToolStripMenuItem";
            this.commitsToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.commitsToolStripMenuItem.Text = "Commits";
            // 
            // weeksToolStripMenuItem
            // 
            this.weeksToolStripMenuItem.Name = "weeksToolStripMenuItem";
            this.weeksToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.weeksToolStripMenuItem.Text = "Weeks";
            // 
            // foldersProjectToolStripMenuItem
            // 
            this.foldersProjectToolStripMenuItem.Name = "foldersProjectToolStripMenuItem";
            this.foldersProjectToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.foldersProjectToolStripMenuItem.Text = "FoldersProject";
            // 
            // chatbotSummaryToolStripMenuItem
            // 
            this.chatbotSummaryToolStripMenuItem.Name = "chatbotSummaryToolStripMenuItem";
            this.chatbotSummaryToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.chatbotSummaryToolStripMenuItem.Text = "ChatbotSummary";
            // 
            // cònToolStripMenuItem
            // 
            this.cònToolStripMenuItem.Name = "cònToolStripMenuItem";
            this.cònToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.cònToolStripMenuItem.Text = "ConfigFiles";
            // 
            // tableLayoutPanel10
            // 
            this.tableLayoutPanel10.ColumnCount = 1;
            this.tableLayoutPanel10.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel10.Controls.Add(this.label6, 0, 0);
            this.tableLayoutPanel10.Controls.Add(this.txtResult, 0, 1);
            this.tableLayoutPanel10.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel10.Location = new System.Drawing.Point(3, 540);
            this.tableLayoutPanel10.Name = "tableLayoutPanel10";
            this.tableLayoutPanel10.RowCount = 2;
            this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel10.Size = new System.Drawing.Size(548, 111);
            this.tableLayoutPanel10.TabIndex = 34;
            // 
            // GitLogAggregator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1002, 711);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.tableLayoutPanel7);
            this.Controls.Add(this.menuStrip1);
            this.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "GitLogAggregator";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "GitLogAggregator";
            this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
            this.Load += new System.EventHandler(this.GitLogAggregator_Load);
            ((System.ComponentModel.ISupportInitialize)(this.setupThuMucThucTap)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewCommits)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNumericsWeek)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel9.ResumeLayout(false);
            this.tableLayoutPanel9.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            this.tableLayoutPanel5.ResumeLayout(false);
            this.tableLayoutPanel6.ResumeLayout(false);
            this.tableLayoutPanel6.PerformLayout();
            this.tableLayoutPanel7.ResumeLayout(false);
            this.tableLayoutPanel8.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.tableLayoutPanel10.ResumeLayout(false);
            this.tableLayoutPanel10.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cboAuthorCommit;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
        private System.Windows.Forms.Button btnOpenGitFolder;
        private System.Windows.Forms.DateTimePicker txtInternshipStartDate;
        private System.Windows.Forms.RichTextBox txtResult;
        private System.Windows.Forms.Button btnAggregator;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ListView weekListView;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btnClearDataListView;
        private System.Windows.Forms.Button helpButton;
        private System.Windows.Forms.PictureBox setupThuMucThucTap;
        private System.Windows.Forms.ListView fileListView;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.DataGridView dataGridViewCommits;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ImageList imageList;
        private System.Windows.Forms.DateTimePicker txtFirstCommitDate;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.DateTimePicker txtInternshipEndDate;
        private System.Windows.Forms.NumericUpDown txtNumericsWeek;
        private System.Windows.Forms.CheckedListBox checkedListBoxCommits;
        private System.Windows.Forms.Button btnExcelCommits;
        private System.Windows.Forms.Button btnReviewCommits;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ListView listViewProjects;
        private System.Windows.Forms.Button btnExpanDataGridview;
        private System.Windows.Forms.Button btnDeleteCommits;
        private System.Windows.Forms.RichTextBox txtResultMouseEvents;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cboThuMucThucTap;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel6;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel7;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel8;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel9;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem danhMụcToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem commitsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem weeksToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem foldersProjectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem chatbotSummaryToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cònToolStripMenuItem;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel10;
    }
}