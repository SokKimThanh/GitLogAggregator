﻿namespace GitLogAggregator
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
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
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
            this.btnDelete = new System.Windows.Forms.Button();
            this.helpButton = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.fileListView = new System.Windows.Forms.ListView();
            this.dataGridViewCommits = new System.Windows.Forms.DataGridView();
            this.label8 = new System.Windows.Forms.Label();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.txtFirstCommitDate = new System.Windows.Forms.DateTimePicker();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.txtInternshipEndDate = new System.Windows.Forms.DateTimePicker();
            this.txtNumericsWeek = new System.Windows.Forms.NumericUpDown();
            this.checkedListBoxCommits = new System.Windows.Forms.CheckedListBox();
            this.btnExport = new System.Windows.Forms.Button();
            this.btnReviewCommits = new System.Windows.Forms.Button();
            this.btnCompleteReview = new System.Windows.Forms.Button();
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.configListView = new System.Windows.Forms.ListView();
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader7 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewCommits)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNumericsWeek)).BeginInit();
            this.SuspendLayout();
            // 
            // cboAuthorCommit
            // 
            this.cboAuthorCommit.FormattingEnabled = true;
            this.cboAuthorCommit.Location = new System.Drawing.Point(184, 73);
            this.cboAuthorCommit.Margin = new System.Windows.Forms.Padding(4);
            this.cboAuthorCommit.Name = "cboAuthorCommit";
            this.cboAuthorCommit.Size = new System.Drawing.Size(200, 25);
            this.cboAuthorCommit.TabIndex = 0;
            // 
            // btnOpenGitFolder
            // 
            this.btnOpenGitFolder.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnOpenGitFolder.Location = new System.Drawing.Point(12, 415);
            this.btnOpenGitFolder.Margin = new System.Windows.Forms.Padding(4);
            this.btnOpenGitFolder.Name = "btnOpenGitFolder";
            this.btnOpenGitFolder.Size = new System.Drawing.Size(85, 30);
            this.btnOpenGitFolder.TabIndex = 1;
            this.btnOpenGitFolder.Text = "Chọn dự án";
            this.btnOpenGitFolder.UseVisualStyleBackColor = true;
            this.btnOpenGitFolder.Click += new System.EventHandler(this.BtnSelectGitFolder_Click);
            // 
            // txtInternshipStartDate
            // 
            this.txtInternshipStartDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.txtInternshipStartDate.Location = new System.Drawing.Point(184, 108);
            this.txtInternshipStartDate.Margin = new System.Windows.Forms.Padding(4);
            this.txtInternshipStartDate.Name = "txtInternshipStartDate";
            this.txtInternshipStartDate.Size = new System.Drawing.Size(199, 25);
            this.txtInternshipStartDate.TabIndex = 3;
            this.txtInternshipStartDate.ValueChanged += new System.EventHandler(this.NumericWeeks_ValueChanged);
            // 
            // txtResult
            // 
            this.txtResult.BackColor = System.Drawing.Color.Gainsboro;
            this.txtResult.ForeColor = System.Drawing.Color.IndianRed;
            this.txtResult.Location = new System.Drawing.Point(12, 538);
            this.txtResult.Margin = new System.Windows.Forms.Padding(4);
            this.txtResult.Name = "txtResult";
            this.txtResult.ReadOnly = true;
            this.txtResult.Size = new System.Drawing.Size(371, 76);
            this.txtResult.TabIndex = 4;
            this.txtResult.Text = "";
            // 
            // btnAggregator
            // 
            this.btnAggregator.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAggregator.Location = new System.Drawing.Point(237, 415);
            this.btnAggregator.Margin = new System.Windows.Forms.Padding(4);
            this.btnAggregator.Name = "btnAggregator";
            this.btnAggregator.Size = new System.Drawing.Size(146, 30);
            this.btnAggregator.TabIndex = 5;
            this.btnAggregator.Text = "Tổng hợp commit";
            this.btnAggregator.UseVisualStyleBackColor = true;
            this.btnAggregator.Click += new System.EventHandler(this.BtnAggregate_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(86, 28);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(197, 25);
            this.label1.TabIndex = 6;
            this.label1.Text = "GitLogAggregator";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 73);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(105, 17);
            this.label2.TabIndex = 7;
            this.label2.Text = "Nhập tên tác giả";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 108);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(137, 17);
            this.label3.TabIndex = 8;
            this.label3.Text = "Chọn ngày bắt đầu TT";
            // 
            // weekListView
            // 
            this.weekListView.BackColor = System.Drawing.Color.WhiteSmoke;
            this.weekListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.weekListView.HideSelection = false;
            this.weekListView.Location = new System.Drawing.Point(13, 214);
            this.weekListView.Margin = new System.Windows.Forms.Padding(4);
            this.weekListView.Name = "weekListView";
            this.weekListView.Size = new System.Drawing.Size(94, 191);
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
            this.label6.Location = new System.Drawing.Point(14, 517);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(120, 17);
            this.label6.TabIndex = 11;
            this.label6.Text = "Hiển thị thông báo:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(17, 183);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(118, 17);
            this.label7.TabIndex = 9;
            this.label7.Text = "Danh mục thực tập";
            // 
            // btnDelete
            // 
            this.btnDelete.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDelete.Location = new System.Drawing.Point(105, 415);
            this.btnDelete.Margin = new System.Windows.Forms.Padding(4);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(122, 30);
            this.btnDelete.TabIndex = 14;
            this.btnDelete.Text = "Xóa dữ liệu";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.BtnDeleteFolderInternship_Click);
            // 
            // helpButton
            // 
            this.helpButton.BackColor = System.Drawing.Color.DeepSkyBlue;
            this.helpButton.Cursor = System.Windows.Forms.Cursors.Help;
            this.helpButton.Dock = System.Windows.Forms.DockStyle.Left;
            this.helpButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.helpButton.Font = new System.Drawing.Font("Microsoft YaHei", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.helpButton.ForeColor = System.Drawing.Color.White;
            this.helpButton.Location = new System.Drawing.Point(0, 0);
            this.helpButton.Margin = new System.Windows.Forms.Padding(4);
            this.helpButton.Name = "helpButton";
            this.helpButton.Size = new System.Drawing.Size(11, 626);
            this.helpButton.TabIndex = 15;
            this.helpButton.Text = "i";
            this.helpButton.UseVisualStyleBackColor = false;
            this.helpButton.Click += new System.EventHandler(this.HelpButton_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(29, 16);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(4);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(49, 49);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 16;
            this.pictureBox1.TabStop = false;
            // 
            // fileListView
            // 
            this.fileListView.BackColor = System.Drawing.Color.WhiteSmoke;
            this.fileListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader5});
            this.fileListView.HideSelection = false;
            this.fileListView.Location = new System.Drawing.Point(115, 214);
            this.fileListView.Margin = new System.Windows.Forms.Padding(4);
            this.fileListView.Name = "fileListView";
            this.fileListView.Size = new System.Drawing.Size(268, 191);
            this.fileListView.TabIndex = 12;
            this.fileListView.UseCompatibleStateImageBehavior = false;
            this.fileListView.View = System.Windows.Forms.View.Details;
            this.fileListView.MouseClick += new System.Windows.Forms.MouseEventHandler(this.FileListView_MouseClick);
            this.fileListView.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.FileListView_MouseDoubleClick);
            // 
            // dataGridViewCommits
            // 
            this.dataGridViewCommits.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.dataGridViewCommits.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dataGridViewCommits.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewCommits.Location = new System.Drawing.Point(395, 214);
            this.dataGridViewCommits.Name = "dataGridViewCommits";
            this.dataGridViewCommits.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            this.dataGridViewCommits.Size = new System.Drawing.Size(274, 400);
            this.dataGridViewCommits.TabIndex = 18;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(234, 181);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(33, 17);
            this.label8.TabIndex = 9;
            this.label8.Text = "tuần";
            // 
            // imageList
            // 
            this.imageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageList.ImageSize = new System.Drawing.Size(16, 16);
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // txtFirstCommitDate
            // 
            this.txtFirstCommitDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.txtFirstCommitDate.Location = new System.Drawing.Point(395, 40);
            this.txtFirstCommitDate.Margin = new System.Windows.Forms.Padding(4);
            this.txtFirstCommitDate.Name = "txtFirstCommitDate";
            this.txtFirstCommitDate.Size = new System.Drawing.Size(273, 25);
            this.txtFirstCommitDate.TabIndex = 3;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(392, 16);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(137, 17);
            this.label9.TabIndex = 8;
            this.label9.Text = "Ngày commit đầu tiên";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(15, 146);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(137, 17);
            this.label10.TabIndex = 8;
            this.label10.Text = "Chọn ngày kết thúc TT";
            // 
            // txtInternshipEndDate
            // 
            this.txtInternshipEndDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.txtInternshipEndDate.Location = new System.Drawing.Point(184, 140);
            this.txtInternshipEndDate.Margin = new System.Windows.Forms.Padding(4);
            this.txtInternshipEndDate.Name = "txtInternshipEndDate";
            this.txtInternshipEndDate.Size = new System.Drawing.Size(199, 25);
            this.txtInternshipEndDate.TabIndex = 3;
            // 
            // txtNumericsWeek
            // 
            this.txtNumericsWeek.Location = new System.Drawing.Point(184, 179);
            this.txtNumericsWeek.Name = "txtNumericsWeek";
            this.txtNumericsWeek.Size = new System.Drawing.Size(43, 25);
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
            this.checkedListBoxCommits.FormattingEnabled = true;
            this.checkedListBoxCommits.Location = new System.Drawing.Point(395, 113);
            this.checkedListBoxCommits.Name = "checkedListBoxCommits";
            this.checkedListBoxCommits.Size = new System.Drawing.Size(274, 84);
            this.checkedListBoxCommits.TabIndex = 23;
            // 
            // btnExport
            // 
            this.btnExport.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnExport.Location = new System.Drawing.Point(287, 177);
            this.btnExport.Margin = new System.Windows.Forms.Padding(4);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(96, 28);
            this.btnExport.TabIndex = 5;
            this.btnExport.Text = "Xuất excel Commit";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.BtnExportExcel_Click);
            // 
            // btnReviewCommits
            // 
            this.btnReviewCommits.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnReviewCommits.Location = new System.Drawing.Point(395, 73);
            this.btnReviewCommits.Margin = new System.Windows.Forms.Padding(4);
            this.btnReviewCommits.Name = "btnReviewCommits";
            this.btnReviewCommits.Size = new System.Drawing.Size(120, 33);
            this.btnReviewCommits.TabIndex = 5;
            this.btnReviewCommits.Text = "Kiểm tra";
            this.btnReviewCommits.UseVisualStyleBackColor = true;
            this.btnReviewCommits.Click += new System.EventHandler(this.BtnReviewCommits_Click);
            // 
            // btnCompleteReview
            // 
            this.btnCompleteReview.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCompleteReview.Location = new System.Drawing.Point(523, 73);
            this.btnCompleteReview.Margin = new System.Windows.Forms.Padding(4);
            this.btnCompleteReview.Name = "btnCompleteReview";
            this.btnCompleteReview.Size = new System.Drawing.Size(146, 33);
            this.btnCompleteReview.TabIndex = 5;
            this.btnCompleteReview.Text = "Tiếp theo";
            this.btnCompleteReview.UseVisualStyleBackColor = true;
            this.btnCompleteReview.Click += new System.EventHandler(this.BtnCompleteReview_Click);
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
            // configListView
            // 
            this.configListView.BackColor = System.Drawing.Color.WhiteSmoke;
            this.configListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader2,
            this.columnHeader6,
            this.columnHeader7});
            this.configListView.HideSelection = false;
            this.configListView.Location = new System.Drawing.Point(14, 452);
            this.configListView.Name = "configListView";
            this.configListView.Size = new System.Drawing.Size(369, 62);
            this.configListView.TabIndex = 24;
            this.configListView.UseCompatibleStateImageBehavior = false;
            this.configListView.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "STT";
            this.columnHeader2.Width = 40;
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "Đường dẫn";
            this.columnHeader6.Width = 150;
            // 
            // columnHeader7
            // 
            this.columnHeader7.Text = "Phân Cấp";
            this.columnHeader7.Width = 150;
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.button1.Cursor = System.Windows.Forms.Cursors.SizeWE;
            this.button1.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.button1.FlatAppearance.BorderSize = 0;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Location = new System.Drawing.Point(659, 214);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(10, 400);
            this.button1.TabIndex = 25;
            this.button1.UseVisualStyleBackColor = false;
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.button2.Cursor = System.Windows.Forms.Cursors.SizeWE;
            this.button2.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.button2.FlatAppearance.BorderSize = 0;
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.Location = new System.Drawing.Point(659, 113);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(10, 84);
            this.button2.TabIndex = 25;
            this.button2.UseVisualStyleBackColor = false;
            // 
            // GitLogAggregator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(681, 626);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.configListView);
            this.Controls.Add(this.checkedListBoxCommits);
            this.Controls.Add(this.txtNumericsWeek);
            this.Controls.Add(this.dataGridViewCommits);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.helpButton);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.fileListView);
            this.Controls.Add(this.weekListView);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.btnCompleteReview);
            this.Controls.Add(this.btnReviewCommits);
            this.Controls.Add(this.btnAggregator);
            this.Controls.Add(this.txtResult);
            this.Controls.Add(this.txtFirstCommitDate);
            this.Controls.Add(this.txtInternshipEndDate);
            this.Controls.Add(this.txtInternshipStartDate);
            this.Controls.Add(this.btnOpenGitFolder);
            this.Controls.Add(this.cboAuthorCommit);
            this.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "GitLogAggregator";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "GitLogAggregator";
            this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
            this.Load += new System.EventHandler(this.GitLogAggregator_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewCommits)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNumericsWeek)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cboAuthorCommit;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
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
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button helpButton;
        private System.Windows.Forms.PictureBox pictureBox1;
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
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Button btnReviewCommits;
        private System.Windows.Forms.Button btnCompleteReview;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ListView configListView;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.ColumnHeader columnHeader7;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
    }
}