﻿namespace GitLogAggregator
{
    partial class ucMainForm
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ucMainForm));
            this.mainGitPanel = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel9 = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.cboConfigFiles = new System.Windows.Forms.ComboBox();
            this.cboInternshipFolder = new System.Windows.Forms.ComboBox();
            this.btnRefreshData = new System.Windows.Forms.Button();
            this.cboAuthorCommit = new System.Windows.Forms.ComboBox();
            this.helpButton = new System.Windows.Forms.Button();
            this.btnSetupThuMucThucTap = new System.Windows.Forms.PictureBox();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.label9 = new System.Windows.Forms.Label();
            this.txtFirstCommitDate = new System.Windows.Forms.DateTimePicker();
            this.label11 = new System.Windows.Forms.Label();
            this.cboSearchByWeek = new System.Windows.Forms.ComboBox();
            this.label12 = new System.Windows.Forms.Label();
            this.cboSearchByAuthor = new System.Windows.Forms.ComboBox();
            this.chkSearchCriteria = new System.Windows.Forms.CheckedListBox();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.weekListView = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.fileListView = new System.Windows.Forms.ListView();
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chkDeleteAllProject = new System.Windows.Forms.CheckBox();
            this.tableLayoutPanel8 = new System.Windows.Forms.TableLayoutPanel();
            this.btnSaveGit = new System.Windows.Forms.Button();
            this.btnRemoveAll = new System.Windows.Forms.Button();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.label3 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.txtInternshipStartDate = new System.Windows.Forms.DateTimePicker();
            this.btnCreateWeek = new System.Windows.Forms.Button();
            this.txtInternshipEndDate = new System.Windows.Forms.DateTimePicker();
            this.txtNumericsWeek = new System.Windows.Forms.NumericUpDown();
            this.chkConfirmInternshipDate = new System.Windows.Forms.CheckBox();
            this.btnOpenGitFolder = new System.Windows.Forms.Button();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel7 = new System.Windows.Forms.TableLayoutPanel();
            this.dgvReportCommits = new System.Windows.Forms.DataGridView();
            this.btnNextReport = new System.Windows.Forms.Button();
            this.btnPreviousReport = new System.Windows.Forms.Button();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.btnExportTXT = new System.Windows.Forms.Button();
            this.searchPanel = new System.Windows.Forms.TableLayoutPanel();
            this.txtSearchReport = new System.Windows.Forms.TextBox();
            this.btnSearchReport = new System.Windows.Forms.Button();
            this.tableLayoutPanel6 = new System.Windows.Forms.TableLayoutPanel();
            this.txtResultMouseEvents = new System.Windows.Forms.RichTextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tableLayoutPanel10 = new System.Windows.Forms.TableLayoutPanel();
            this.label6 = new System.Windows.Forms.Label();
            this.txtResult = new System.Windows.Forms.RichTextBox();
            this.crudImageList = new System.Windows.Forms.ImageList(this.components);
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.mainGitPanel.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel9.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnSetupThuMucThucTap)).BeginInit();
            this.tableLayoutPanel4.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel8.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtNumericsWeek)).BeginInit();
            this.tableLayoutPanel5.SuspendLayout();
            this.tableLayoutPanel7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvReportCommits)).BeginInit();
            this.flowLayoutPanel1.SuspendLayout();
            this.searchPanel.SuspendLayout();
            this.tableLayoutPanel6.SuspendLayout();
            this.tableLayoutPanel10.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainGitPanel
            // 
            this.mainGitPanel.ColumnCount = 2;
            this.mainGitPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 38.12375F));
            this.mainGitPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 61.87625F));
            this.mainGitPanel.Controls.Add(this.tableLayoutPanel1, 0, 0);
            this.mainGitPanel.Controls.Add(this.tableLayoutPanel4, 1, 0);
            this.mainGitPanel.Controls.Add(this.tableLayoutPanel3, 0, 2);
            this.mainGitPanel.Controls.Add(this.tableLayoutPanel2, 0, 1);
            this.mainGitPanel.Controls.Add(this.tableLayoutPanel5, 1, 1);
            this.mainGitPanel.Controls.Add(this.tableLayoutPanel6, 1, 4);
            this.mainGitPanel.Controls.Add(this.tableLayoutPanel10, 0, 4);
            this.mainGitPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainGitPanel.Location = new System.Drawing.Point(0, 0);
            this.mainGitPanel.Margin = new System.Windows.Forms.Padding(0);
            this.mainGitPanel.Name = "mainGitPanel";
            this.mainGitPanel.RowCount = 5;
            this.mainGitPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.mainGitPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 107F));
            this.mainGitPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 223F));
            this.mainGitPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 137F));
            this.mainGitPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 117F));
            this.mainGitPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.mainGitPanel.Size = new System.Drawing.Size(875, 674);
            this.mainGitPanel.TabIndex = 41;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 29.63989F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70.36011F));
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel9, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.helpButton, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnSetupThuMucThucTap, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(333, 90);
            this.tableLayoutPanel1.TabIndex = 26;
            // 
            // tableLayoutPanel9
            // 
            this.tableLayoutPanel9.ColumnCount = 2;
            this.tableLayoutPanel9.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel9.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel9.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel9.Controls.Add(this.cboConfigFiles, 0, 2);
            this.tableLayoutPanel9.Controls.Add(this.cboInternshipFolder, 0, 1);
            this.tableLayoutPanel9.Controls.Add(this.btnRefreshData, 1, 1);
            this.tableLayoutPanel9.Controls.Add(this.cboAuthorCommit, 1, 2);
            this.tableLayoutPanel9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel9.Location = new System.Drawing.Point(112, 0);
            this.tableLayoutPanel9.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel9.Name = "tableLayoutPanel9";
            this.tableLayoutPanel9.RowCount = 3;
            this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel9.Size = new System.Drawing.Size(221, 90);
            this.tableLayoutPanel9.TabIndex = 17;
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
            this.label1.Size = new System.Drawing.Size(213, 30);
            this.label1.TabIndex = 6;
            this.label1.Text = "GitLogAggregator";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cboConfigFiles
            // 
            this.cboConfigFiles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cboConfigFiles.FormattingEnabled = true;
            this.cboConfigFiles.Location = new System.Drawing.Point(3, 63);
            this.cboConfigFiles.Name = "cboConfigFiles";
            this.cboConfigFiles.Size = new System.Drawing.Size(104, 21);
            this.cboConfigFiles.TabIndex = 26;
            this.cboConfigFiles.SelectedIndexChanged += new System.EventHandler(this.CboConfigFiles_SelectedIndexChanged);
            // 
            // cboInternshipFolder
            // 
            this.cboInternshipFolder.Cursor = System.Windows.Forms.Cursors.Hand;
            this.cboInternshipFolder.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cboInternshipFolder.FormattingEnabled = true;
            this.cboInternshipFolder.Location = new System.Drawing.Point(3, 33);
            this.cboInternshipFolder.Name = "cboInternshipFolder";
            this.cboInternshipFolder.Size = new System.Drawing.Size(104, 21);
            this.cboInternshipFolder.TabIndex = 0;
            this.cboInternshipFolder.SelectedIndexChanged += new System.EventHandler(this.CboThuMucThucTap_SelectedIndexChanged);
            // 
            // btnRefreshData
            // 
            this.btnRefreshData.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btnRefreshData.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnRefreshData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnRefreshData.FlatAppearance.BorderSize = 0;
            this.btnRefreshData.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRefreshData.Location = new System.Drawing.Point(111, 31);
            this.btnRefreshData.Margin = new System.Windows.Forms.Padding(1);
            this.btnRefreshData.Name = "btnRefreshData";
            this.btnRefreshData.Size = new System.Drawing.Size(109, 28);
            this.btnRefreshData.TabIndex = 14;
            this.btnRefreshData.Text = "Làm mới";
            this.btnRefreshData.UseVisualStyleBackColor = false;
            this.btnRefreshData.Click += new System.EventHandler(this.BtnRefreshData_Click);
            // 
            // cboAuthorCommit
            // 
            this.cboAuthorCommit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cboAuthorCommit.FormattingEnabled = true;
            this.cboAuthorCommit.Location = new System.Drawing.Point(113, 63);
            this.cboAuthorCommit.Name = "cboAuthorCommit";
            this.cboAuthorCommit.Size = new System.Drawing.Size(105, 21);
            this.cboAuthorCommit.TabIndex = 26;
            this.cboAuthorCommit.SelectedIndexChanged += new System.EventHandler(this.CboSearchByAuthor_SelectedIndexChanged);
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
            this.helpButton.Size = new System.Drawing.Size(20, 90);
            this.helpButton.TabIndex = 15;
            this.helpButton.Text = "i";
            this.helpButton.UseVisualStyleBackColor = false;
            this.helpButton.Click += new System.EventHandler(this.BtnHelpButton_Click);
            // 
            // btnSetupThuMucThucTap
            // 
            this.btnSetupThuMucThucTap.BackColor = System.Drawing.Color.Transparent;
            this.btnSetupThuMucThucTap.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSetupThuMucThucTap.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSetupThuMucThucTap.Image = ((System.Drawing.Image)(resources.GetObject("btnSetupThuMucThucTap.Image")));
            this.btnSetupThuMucThucTap.Location = new System.Drawing.Point(20, 0);
            this.btnSetupThuMucThucTap.Margin = new System.Windows.Forms.Padding(0);
            this.btnSetupThuMucThucTap.Name = "btnSetupThuMucThucTap";
            this.btnSetupThuMucThucTap.Size = new System.Drawing.Size(92, 90);
            this.btnSetupThuMucThucTap.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.btnSetupThuMucThucTap.TabIndex = 16;
            this.btnSetupThuMucThucTap.TabStop = false;
            this.btnSetupThuMucThucTap.Click += new System.EventHandler(this.BtnSetupThuMucThucTap_Click);
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 3;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 24.95974F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40.74074F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 34.29952F));
            this.tableLayoutPanel4.Controls.Add(this.label9, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.txtFirstCommitDate, 1, 0);
            this.tableLayoutPanel4.Controls.Add(this.label11, 0, 1);
            this.tableLayoutPanel4.Controls.Add(this.cboSearchByWeek, 1, 1);
            this.tableLayoutPanel4.Controls.Add(this.label12, 0, 2);
            this.tableLayoutPanel4.Controls.Add(this.cboSearchByAuthor, 1, 2);
            this.tableLayoutPanel4.Controls.Add(this.chkSearchCriteria, 2, 0);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(333, 0);
            this.tableLayoutPanel4.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 3;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(542, 90);
            this.tableLayoutPanel4.TabIndex = 29;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(3, 3);
            this.label9.Margin = new System.Windows.Forms.Padding(3);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(110, 13);
            this.label9.TabIndex = 8;
            this.label9.Text = "Ngày commit đầu tiên";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtFirstCommitDate
            // 
            this.txtFirstCommitDate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtFirstCommitDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.txtFirstCommitDate.Location = new System.Drawing.Point(138, 3);
            this.txtFirstCommitDate.Name = "txtFirstCommitDate";
            this.txtFirstCommitDate.Size = new System.Drawing.Size(214, 20);
            this.txtFirstCommitDate.TabIndex = 3;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(3, 33);
            this.label11.Margin = new System.Windows.Forms.Padding(3);
            this.label11.Name = "label11";
            this.label11.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label11.Size = new System.Drawing.Size(109, 13);
            this.label11.TabIndex = 8;
            this.label11.Text = "Lọc commit theo tuần";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cboSearchByWeek
            // 
            this.cboSearchByWeek.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cboSearchByWeek.FormattingEnabled = true;
            this.cboSearchByWeek.Location = new System.Drawing.Point(138, 33);
            this.cboSearchByWeek.Name = "cboSearchByWeek";
            this.cboSearchByWeek.Size = new System.Drawing.Size(214, 21);
            this.cboSearchByWeek.TabIndex = 9;
            this.cboSearchByWeek.SelectedIndexChanged += new System.EventHandler(this.CboSearchByWeek_SelectedIndexChanged);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(3, 63);
            this.label12.Margin = new System.Windows.Forms.Padding(3);
            this.label12.Name = "label12";
            this.label12.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label12.Size = new System.Drawing.Size(120, 13);
            this.label12.TabIndex = 8;
            this.label12.Text = "Lọc commit theo tác giả";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cboSearchByAuthor
            // 
            this.cboSearchByAuthor.Cursor = System.Windows.Forms.Cursors.Hand;
            this.cboSearchByAuthor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cboSearchByAuthor.FormattingEnabled = true;
            this.cboSearchByAuthor.Location = new System.Drawing.Point(138, 63);
            this.cboSearchByAuthor.Name = "cboSearchByAuthor";
            this.cboSearchByAuthor.Size = new System.Drawing.Size(214, 21);
            this.cboSearchByAuthor.TabIndex = 11;
            this.cboSearchByAuthor.SelectedIndexChanged += new System.EventHandler(this.CboSearchByAuthor_SelectedIndexChanged);
            // 
            // chkSearchCriteria
            // 
            this.chkSearchCriteria.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chkSearchCriteria.FormattingEnabled = true;
            this.chkSearchCriteria.Location = new System.Drawing.Point(355, 0);
            this.chkSearchCriteria.Margin = new System.Windows.Forms.Padding(0);
            this.chkSearchCriteria.Name = "chkSearchCriteria";
            this.tableLayoutPanel4.SetRowSpan(this.chkSearchCriteria, 3);
            this.chkSearchCriteria.Size = new System.Drawing.Size(187, 90);
            this.chkSearchCriteria.TabIndex = 12;
            this.chkSearchCriteria.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.ChkSearchCriteria_ItemCheck);
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.07087F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 66.92913F));
            this.tableLayoutPanel3.Controls.Add(this.weekListView, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.fileListView, 1, 1);
            this.tableLayoutPanel3.Controls.Add(this.chkDeleteAllProject, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.tableLayoutPanel8, 1, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(0, 197);
            this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 2;
            this.mainGitPanel.SetRowSpan(this.tableLayoutPanel3, 2);
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 47F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(333, 360);
            this.tableLayoutPanel3.TabIndex = 28;
            // 
            // weekListView
            // 
            this.weekListView.BackColor = System.Drawing.Color.WhiteSmoke;
            this.weekListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.weekListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.weekListView.HideSelection = false;
            this.weekListView.Location = new System.Drawing.Point(3, 50);
            this.weekListView.Name = "weekListView";
            this.weekListView.Size = new System.Drawing.Size(104, 307);
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
            // fileListView
            // 
            this.fileListView.BackColor = System.Drawing.Color.WhiteSmoke;
            this.fileListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader5});
            this.fileListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fileListView.HideSelection = false;
            this.fileListView.Location = new System.Drawing.Point(113, 50);
            this.fileListView.Name = "fileListView";
            this.fileListView.Size = new System.Drawing.Size(217, 307);
            this.fileListView.TabIndex = 12;
            this.fileListView.UseCompatibleStateImageBehavior = false;
            this.fileListView.View = System.Windows.Forms.View.Details;
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
            // chkDeleteAllProject
            // 
            this.chkDeleteAllProject.AutoSize = true;
            this.chkDeleteAllProject.BackColor = System.Drawing.Color.Transparent;
            this.chkDeleteAllProject.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chkDeleteAllProject.Location = new System.Drawing.Point(3, 3);
            this.chkDeleteAllProject.Name = "chkDeleteAllProject";
            this.chkDeleteAllProject.Size = new System.Drawing.Size(104, 41);
            this.chkDeleteAllProject.TabIndex = 14;
            this.chkDeleteAllProject.Text = "Xóa All dự án";
            this.chkDeleteAllProject.UseVisualStyleBackColor = false;
            this.chkDeleteAllProject.Click += new System.EventHandler(this.ChkDeleteAllProject_CheckedChanged);
            // 
            // tableLayoutPanel8
            // 
            this.tableLayoutPanel8.ColumnCount = 2;
            this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel8.Controls.Add(this.btnSaveGit, 1, 0);
            this.tableLayoutPanel8.Controls.Add(this.btnRemoveAll, 0, 0);
            this.tableLayoutPanel8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel8.Location = new System.Drawing.Point(110, 0);
            this.tableLayoutPanel8.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel8.Name = "tableLayoutPanel8";
            this.tableLayoutPanel8.RowCount = 1;
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel8.Size = new System.Drawing.Size(223, 47);
            this.tableLayoutPanel8.TabIndex = 13;
            // 
            // btnSaveGit
            // 
            this.btnSaveGit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btnSaveGit.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSaveGit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSaveGit.FlatAppearance.BorderSize = 0;
            this.btnSaveGit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSaveGit.Location = new System.Drawing.Point(112, 1);
            this.btnSaveGit.Margin = new System.Windows.Forms.Padding(1);
            this.btnSaveGit.Name = "btnSaveGit";
            this.btnSaveGit.Size = new System.Drawing.Size(110, 45);
            this.btnSaveGit.TabIndex = 25;
            this.btnSaveGit.Text = "Tải Commit";
            this.btnSaveGit.UseVisualStyleBackColor = false;
            this.btnSaveGit.Click += new System.EventHandler(this.BtnSaveGit_Click);
            // 
            // btnRemoveAll
            // 
            this.btnRemoveAll.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.btnRemoveAll.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnRemoveAll.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnRemoveAll.FlatAppearance.BorderSize = 0;
            this.btnRemoveAll.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRemoveAll.Location = new System.Drawing.Point(1, 1);
            this.btnRemoveAll.Margin = new System.Windows.Forms.Padding(1);
            this.btnRemoveAll.Name = "btnRemoveAll";
            this.btnRemoveAll.Size = new System.Drawing.Size(109, 45);
            this.btnRemoveAll.TabIndex = 25;
            this.btnRemoveAll.Text = "Xóa tất cả";
            this.btnRemoveAll.UseVisualStyleBackColor = false;
            this.btnRemoveAll.Click += new System.EventHandler(this.BtnRemoveAll_Click);
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 3;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.Controls.Add(this.label3, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.label10, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.label8, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.txtInternshipStartDate, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.btnCreateWeek, 1, 2);
            this.tableLayoutPanel2.Controls.Add(this.txtInternshipEndDate, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.txtNumericsWeek, 2, 1);
            this.tableLayoutPanel2.Controls.Add(this.chkConfirmInternshipDate, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.btnOpenGitFolder, 2, 2);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 90);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 3;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.12693F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 27.86378F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 39.00929F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(333, 107);
            this.tableLayoutPanel2.TabIndex = 27;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Location = new System.Drawing.Point(3, 3);
            this.label3.Margin = new System.Windows.Forms.Padding(3);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(105, 29);
            this.label3.TabIndex = 8;
            this.label3.Text = "Ngày bắt đầu TT:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label10.Location = new System.Drawing.Point(114, 3);
            this.label10.Margin = new System.Windows.Forms.Padding(3);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(105, 29);
            this.label10.TabIndex = 8;
            this.label10.Text = "Ngày kết thúc TT:";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label8.Location = new System.Drawing.Point(225, 3);
            this.label8.Margin = new System.Windows.Forms.Padding(3);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(105, 29);
            this.label8.TabIndex = 9;
            this.label8.Text = "Số ngày TT:";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtInternshipStartDate
            // 
            this.txtInternshipStartDate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtInternshipStartDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.txtInternshipStartDate.Location = new System.Drawing.Point(3, 38);
            this.txtInternshipStartDate.Name = "txtInternshipStartDate";
            this.txtInternshipStartDate.Size = new System.Drawing.Size(105, 20);
            this.txtInternshipStartDate.TabIndex = 3;
            this.txtInternshipStartDate.ValueChanged += new System.EventHandler(this.TxtNumericWeeks_ValueChanged);
            // 
            // btnCreateWeek
            // 
            this.btnCreateWeek.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.btnCreateWeek.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCreateWeek.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCreateWeek.FlatAppearance.BorderSize = 0;
            this.btnCreateWeek.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCreateWeek.ImageKey = "plus.png";
            this.btnCreateWeek.Location = new System.Drawing.Point(112, 65);
            this.btnCreateWeek.Margin = new System.Windows.Forms.Padding(1);
            this.btnCreateWeek.Name = "btnCreateWeek";
            this.btnCreateWeek.Size = new System.Drawing.Size(109, 41);
            this.btnCreateWeek.TabIndex = 25;
            this.btnCreateWeek.Text = "Tạo tuần";
            this.btnCreateWeek.UseVisualStyleBackColor = false;
            this.btnCreateWeek.Click += new System.EventHandler(this.BtnCreateWeekAndPeriod_Click);
            // 
            // txtInternshipEndDate
            // 
            this.txtInternshipEndDate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtInternshipEndDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.txtInternshipEndDate.Location = new System.Drawing.Point(114, 38);
            this.txtInternshipEndDate.Name = "txtInternshipEndDate";
            this.txtInternshipEndDate.Size = new System.Drawing.Size(105, 20);
            this.txtInternshipEndDate.TabIndex = 3;
            this.txtInternshipEndDate.ValueChanged += new System.EventHandler(this.TxtNumericWeeks_ValueChanged);
            // 
            // txtNumericsWeek
            // 
            this.txtNumericsWeek.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtNumericsWeek.Location = new System.Drawing.Point(225, 38);
            this.txtNumericsWeek.Name = "txtNumericsWeek";
            this.txtNumericsWeek.Size = new System.Drawing.Size(105, 20);
            this.txtNumericsWeek.TabIndex = 20;
            this.txtNumericsWeek.Value = new decimal(new int[] {
            8,
            0,
            0,
            0});
            this.txtNumericsWeek.ValueChanged += new System.EventHandler(this.TxtNumericWeeks_ValueChanged);
            // 
            // chkConfirmInternshipDate
            // 
            this.chkConfirmInternshipDate.AutoSize = true;
            this.chkConfirmInternshipDate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chkConfirmInternshipDate.Location = new System.Drawing.Point(3, 64);
            this.chkConfirmInternshipDate.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.chkConfirmInternshipDate.Name = "chkConfirmInternshipDate";
            this.chkConfirmInternshipDate.Size = new System.Drawing.Size(108, 43);
            this.chkConfirmInternshipDate.TabIndex = 26;
            this.chkConfirmInternshipDate.Text = "Xác nhậnDateTT";
            this.chkConfirmInternshipDate.UseVisualStyleBackColor = true;
            this.chkConfirmInternshipDate.Click += new System.EventHandler(this.ChkConfirmInternshipDate_CheckedChanged);
            // 
            // btnOpenGitFolder
            // 
            this.btnOpenGitFolder.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.btnOpenGitFolder.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnOpenGitFolder.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnOpenGitFolder.FlatAppearance.BorderSize = 0;
            this.btnOpenGitFolder.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOpenGitFolder.Location = new System.Drawing.Point(223, 65);
            this.btnOpenGitFolder.Margin = new System.Windows.Forms.Padding(1);
            this.btnOpenGitFolder.Name = "btnOpenGitFolder";
            this.btnOpenGitFolder.Size = new System.Drawing.Size(109, 41);
            this.btnOpenGitFolder.TabIndex = 1;
            this.btnOpenGitFolder.Text = "Thêm dự án";
            this.btnOpenGitFolder.UseVisualStyleBackColor = false;
            this.btnOpenGitFolder.Click += new System.EventHandler(this.BtnAddProject_Click);
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.ColumnCount = 1;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel5.Controls.Add(this.tableLayoutPanel7, 0, 1);
            this.tableLayoutPanel5.Controls.Add(this.searchPanel, 0, 0);
            this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel5.Location = new System.Drawing.Point(333, 90);
            this.tableLayoutPanel5.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 2;
            this.mainGitPanel.SetRowSpan(this.tableLayoutPanel5, 3);
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 99.99999F));
            this.tableLayoutPanel5.Size = new System.Drawing.Size(542, 467);
            this.tableLayoutPanel5.TabIndex = 30;
            // 
            // tableLayoutPanel7
            // 
            this.tableLayoutPanel7.ColumnCount = 3;
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel7.Controls.Add(this.dgvReportCommits, 1, 1);
            this.tableLayoutPanel7.Controls.Add(this.btnNextReport, 2, 1);
            this.tableLayoutPanel7.Controls.Add(this.btnPreviousReport, 0, 1);
            this.tableLayoutPanel7.Controls.Add(this.flowLayoutPanel1, 1, 2);
            this.tableLayoutPanel7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel7.Location = new System.Drawing.Point(0, 30);
            this.tableLayoutPanel7.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel7.Name = "tableLayoutPanel7";
            this.tableLayoutPanel7.RowCount = 3;
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel7.Size = new System.Drawing.Size(542, 437);
            this.tableLayoutPanel7.TabIndex = 37;
            // 
            // dgvReportCommits
            // 
            this.dgvReportCommits.BackgroundColor = System.Drawing.Color.Gainsboro;
            this.dgvReportCommits.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgvReportCommits.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvReportCommits.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvReportCommits.Location = new System.Drawing.Point(11, 0);
            this.dgvReportCommits.Margin = new System.Windows.Forms.Padding(0);
            this.dgvReportCommits.Name = "dgvReportCommits";
            this.dgvReportCommits.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            this.dgvReportCommits.Size = new System.Drawing.Size(520, 399);
            this.dgvReportCommits.TabIndex = 18;
            // 
            // btnNextReport
            // 
            this.btnNextReport.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btnNextReport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnNextReport.FlatAppearance.BorderSize = 0;
            this.btnNextReport.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNextReport.ImageIndex = 1;
            this.btnNextReport.Location = new System.Drawing.Point(531, 0);
            this.btnNextReport.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.btnNextReport.Name = "btnNextReport";
            this.btnNextReport.Size = new System.Drawing.Size(8, 399);
            this.btnNextReport.TabIndex = 38;
            this.btnNextReport.UseVisualStyleBackColor = false;
            this.btnNextReport.Click += new System.EventHandler(this.BtnNextReport_Click);
            // 
            // btnPreviousReport
            // 
            this.btnPreviousReport.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btnPreviousReport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnPreviousReport.FlatAppearance.BorderSize = 0;
            this.btnPreviousReport.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPreviousReport.ImageIndex = 0;
            this.btnPreviousReport.Location = new System.Drawing.Point(3, 0);
            this.btnPreviousReport.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.btnPreviousReport.Name = "btnPreviousReport";
            this.btnPreviousReport.Size = new System.Drawing.Size(8, 399);
            this.btnPreviousReport.TabIndex = 38;
            this.btnPreviousReport.UseVisualStyleBackColor = false;
            this.btnPreviousReport.Click += new System.EventHandler(this.BtnPreviousReport_Click);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.tableLayoutPanel7.SetColumnSpan(this.flowLayoutPanel1, 3);
            this.flowLayoutPanel1.Controls.Add(this.btnExportTXT);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 399);
            this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(542, 38);
            this.flowLayoutPanel1.TabIndex = 37;
            // 
            // btnExportTXT
            // 
            this.btnExportTXT.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.btnExportTXT.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnExportTXT.FlatAppearance.BorderSize = 0;
            this.btnExportTXT.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExportTXT.Location = new System.Drawing.Point(362, 0);
            this.btnExportTXT.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.btnExportTXT.Name = "btnExportTXT";
            this.btnExportTXT.Size = new System.Drawing.Size(177, 37);
            this.btnExportTXT.TabIndex = 6;
            this.btnExportTXT.Text = "Ghi File";
            this.btnExportTXT.UseVisualStyleBackColor = false;
            this.btnExportTXT.Click += new System.EventHandler(this.BtnExportTXT_Click);
            // 
            // searchPanel
            // 
            this.searchPanel.ColumnCount = 2;
            this.searchPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.searchPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.searchPanel.Controls.Add(this.txtSearchReport, 0, 0);
            this.searchPanel.Controls.Add(this.btnSearchReport, 1, 0);
            this.searchPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.searchPanel.Location = new System.Drawing.Point(0, 0);
            this.searchPanel.Margin = new System.Windows.Forms.Padding(0, 0, 1, 0);
            this.searchPanel.Name = "searchPanel";
            this.searchPanel.RowCount = 1;
            this.searchPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.searchPanel.Size = new System.Drawing.Size(541, 30);
            this.searchPanel.TabIndex = 37;
            // 
            // txtSearchReport
            // 
            this.txtSearchReport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtSearchReport.Location = new System.Drawing.Point(3, 3);
            this.txtSearchReport.Name = "txtSearchReport";
            this.txtSearchReport.Size = new System.Drawing.Size(438, 20);
            this.txtSearchReport.TabIndex = 33;
            this.txtSearchReport.Click += new System.EventHandler(this.TxtSearchReport_TextChanged);
            // 
            // btnSearchReport
            // 
            this.btnSearchReport.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btnSearchReport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSearchReport.FlatAppearance.BorderSize = 0;
            this.btnSearchReport.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSearchReport.ImageKey = "magnifying-glass.png";
            this.btnSearchReport.Location = new System.Drawing.Point(447, 3);
            this.btnSearchReport.Name = "btnSearchReport";
            this.btnSearchReport.Size = new System.Drawing.Size(91, 24);
            this.btnSearchReport.TabIndex = 34;
            this.btnSearchReport.Text = "Tìm kiếm";
            this.btnSearchReport.UseVisualStyleBackColor = false;
            this.btnSearchReport.Click += new System.EventHandler(this.BtnSearchReport_Click);
            // 
            // tableLayoutPanel6
            // 
            this.tableLayoutPanel6.ColumnCount = 1;
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel6.Controls.Add(this.txtResultMouseEvents, 1, 1);
            this.tableLayoutPanel6.Controls.Add(this.label4, 0, 0);
            this.tableLayoutPanel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel6.Location = new System.Drawing.Point(336, 560);
            this.tableLayoutPanel6.Name = "tableLayoutPanel6";
            this.tableLayoutPanel6.RowCount = 1;
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel6.Size = new System.Drawing.Size(536, 111);
            this.tableLayoutPanel6.TabIndex = 31;
            // 
            // txtResultMouseEvents
            // 
            this.txtResultMouseEvents.BackColor = System.Drawing.Color.Gainsboro;
            this.txtResultMouseEvents.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtResultMouseEvents.ForeColor = System.Drawing.Color.IndianRed;
            this.txtResultMouseEvents.Location = new System.Drawing.Point(3, 22);
            this.txtResultMouseEvents.Name = "txtResultMouseEvents";
            this.txtResultMouseEvents.ReadOnly = true;
            this.txtResultMouseEvents.Size = new System.Drawing.Size(530, 86);
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
            this.label4.Size = new System.Drawing.Size(530, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "Sự kiện rê chuột khu vực commit:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tableLayoutPanel10
            // 
            this.tableLayoutPanel10.ColumnCount = 1;
            this.tableLayoutPanel10.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel10.Controls.Add(this.label6, 0, 0);
            this.tableLayoutPanel10.Controls.Add(this.txtResult, 0, 1);
            this.tableLayoutPanel10.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel10.Location = new System.Drawing.Point(3, 560);
            this.tableLayoutPanel10.Name = "tableLayoutPanel10";
            this.tableLayoutPanel10.RowCount = 2;
            this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel10.Size = new System.Drawing.Size(327, 111);
            this.tableLayoutPanel10.TabIndex = 34;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label6.Location = new System.Drawing.Point(3, 3);
            this.label6.Margin = new System.Windows.Forms.Padding(3);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(321, 13);
            this.label6.TabIndex = 11;
            this.label6.Text = "Hiển thị thông báo:";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtResult
            // 
            this.txtResult.BackColor = System.Drawing.Color.Gainsboro;
            this.txtResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtResult.ForeColor = System.Drawing.Color.IndianRed;
            this.txtResult.Location = new System.Drawing.Point(3, 22);
            this.txtResult.Name = "txtResult";
            this.txtResult.ReadOnly = true;
            this.txtResult.Size = new System.Drawing.Size(321, 86);
            this.txtResult.TabIndex = 4;
            this.txtResult.Text = "";
            // 
            // crudImageList
            // 
            this.crudImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("crudImageList.ImageStream")));
            this.crudImageList.TransparentColor = System.Drawing.Color.Transparent;
            this.crudImageList.Images.SetKeyName(0, "edit_icon.png");
            this.crudImageList.Images.SetKeyName(1, "add_icon.png");
            this.crudImageList.Images.SetKeyName(2, "delete_icon.png");
            // 
            // imageList
            // 
            this.imageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.imageList.ImageSize = new System.Drawing.Size(16, 16);
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // ucMainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.Controls.Add(this.mainGitPanel);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "ucMainForm";
            this.Size = new System.Drawing.Size(875, 674);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.mainGitPanel.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel9.ResumeLayout(false);
            this.tableLayoutPanel9.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnSetupThuMucThucTap)).EndInit();
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.tableLayoutPanel8.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtNumericsWeek)).EndInit();
            this.tableLayoutPanel5.ResumeLayout(false);
            this.tableLayoutPanel7.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvReportCommits)).EndInit();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.searchPanel.ResumeLayout(false);
            this.searchPanel.PerformLayout();
            this.tableLayoutPanel6.ResumeLayout(false);
            this.tableLayoutPanel6.PerformLayout();
            this.tableLayoutPanel10.ResumeLayout(false);
            this.tableLayoutPanel10.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel mainGitPanel;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel9;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cboConfigFiles;
        private System.Windows.Forms.ComboBox cboInternshipFolder;
        private System.Windows.Forms.Button btnRefreshData;
        private System.Windows.Forms.ComboBox cboAuthorCommit;
        private System.Windows.Forms.Button helpButton;
        private System.Windows.Forms.PictureBox btnSetupThuMucThucTap;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.DateTimePicker txtFirstCommitDate;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ComboBox cboSearchByWeek;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.ComboBox cboSearchByAuthor;
        private System.Windows.Forms.CheckedListBox chkSearchCriteria;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.ListView weekListView;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ListView fileListView;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.CheckBox chkDeleteAllProject;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel8;
        private System.Windows.Forms.Button btnSaveGit;
        private System.Windows.Forms.Button btnRemoveAll;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.DateTimePicker txtInternshipStartDate;
        private System.Windows.Forms.Button btnCreateWeek;
        private System.Windows.Forms.DateTimePicker txtInternshipEndDate;
        private System.Windows.Forms.NumericUpDown txtNumericsWeek;
        private System.Windows.Forms.CheckBox chkConfirmInternshipDate;
        private System.Windows.Forms.Button btnOpenGitFolder;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel7;
        private System.Windows.Forms.DataGridView dgvReportCommits;
        private System.Windows.Forms.Button btnNextReport;
        private System.Windows.Forms.Button btnPreviousReport;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button btnExportTXT;
        private System.Windows.Forms.TableLayoutPanel searchPanel;
        private System.Windows.Forms.TextBox txtSearchReport;
        private System.Windows.Forms.Button btnSearchReport;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel6;
        private System.Windows.Forms.RichTextBox txtResultMouseEvents;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel10;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.RichTextBox txtResult;
        private System.Windows.Forms.ImageList crudImageList;
        private System.Windows.Forms.ImageList imageList;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
    }
}
