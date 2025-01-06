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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GitLogAggregator));
            this.cboAuthorCommit = new System.Windows.Forms.ComboBox();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.selectGitFolderButton = new System.Windows.Forms.Button();
            this.txtInternshipDate = new System.Windows.Forms.DateTimePicker();
            this.resultRichTextBox = new System.Windows.Forms.RichTextBox();
            this.aggregateButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.folderPathLabel = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.weekListView = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.folderPathInternWeekLabel = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.deleteFolderButton = new System.Windows.Forms.Button();
            this.helpButton = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.fileListView = new System.Windows.Forms.ListView();
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // cboAuthorCommit
            // 
            this.cboAuthorCommit.FormattingEnabled = true;
            this.cboAuthorCommit.Location = new System.Drawing.Point(220, 69);
            this.cboAuthorCommit.Margin = new System.Windows.Forms.Padding(4);
            this.cboAuthorCommit.Name = "cboAuthorCommit";
            this.cboAuthorCommit.Size = new System.Drawing.Size(350, 25);
            this.cboAuthorCommit.TabIndex = 0;
            // 
            // selectGitFolderButton
            // 
            this.selectGitFolderButton.Location = new System.Drawing.Point(18, 373);
            this.selectGitFolderButton.Margin = new System.Windows.Forms.Padding(4);
            this.selectGitFolderButton.Name = "selectGitFolderButton";
            this.selectGitFolderButton.Size = new System.Drawing.Size(203, 30);
            this.selectGitFolderButton.TabIndex = 1;
            this.selectGitFolderButton.Text = "Chọn thư mục dự án";
            this.selectGitFolderButton.UseVisualStyleBackColor = true;
            this.selectGitFolderButton.Click += new System.EventHandler(this.selectGitFolderButton_Click);
            // 
            // txtInternshipDate
            // 
            this.txtInternshipDate.Location = new System.Drawing.Point(220, 104);
            this.txtInternshipDate.Margin = new System.Windows.Forms.Padding(4);
            this.txtInternshipDate.Name = "txtInternshipDate";
            this.txtInternshipDate.Size = new System.Drawing.Size(351, 25);
            this.txtInternshipDate.TabIndex = 3;
            // 
            // resultRichTextBox
            // 
            this.resultRichTextBox.BackColor = System.Drawing.Color.Gainsboro;
            this.resultRichTextBox.Enabled = false;
            this.resultRichTextBox.ForeColor = System.Drawing.Color.IndianRed;
            this.resultRichTextBox.Location = new System.Drawing.Point(228, 444);
            this.resultRichTextBox.Margin = new System.Windows.Forms.Padding(4);
            this.resultRichTextBox.Name = "resultRichTextBox";
            this.resultRichTextBox.ReadOnly = true;
            this.resultRichTextBox.Size = new System.Drawing.Size(343, 127);
            this.resultRichTextBox.TabIndex = 4;
            this.resultRichTextBox.Text = "";
            // 
            // aggregateButton
            // 
            this.aggregateButton.Location = new System.Drawing.Point(396, 373);
            this.aggregateButton.Margin = new System.Windows.Forms.Padding(4);
            this.aggregateButton.Name = "aggregateButton";
            this.aggregateButton.Size = new System.Drawing.Size(175, 30);
            this.aggregateButton.TabIndex = 5;
            this.aggregateButton.Text = "Tổng hợp commit";
            this.aggregateButton.UseVisualStyleBackColor = true;
            this.aggregateButton.Click += new System.EventHandler(this.aggregateButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(196, 21);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(197, 25);
            this.label1.TabIndex = 6;
            this.label1.Text = "GitLogAggregator";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 69);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(105, 17);
            this.label2.TabIndex = 7;
            this.label2.Text = "Nhập tên tác giả";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(14, 104);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(137, 17);
            this.label3.TabIndex = 8;
            this.label3.Text = "Chọn ngày bắt đầu TT";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(225, 146);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(134, 17);
            this.label4.TabIndex = 9;
            this.label4.Text = "Hiển thị URL báo cáo:";
            // 
            // folderPathLabel
            // 
            this.folderPathLabel.BackColor = System.Drawing.Color.Gainsboro;
            this.folderPathLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.folderPathLabel.Location = new System.Drawing.Point(18, 446);
            this.folderPathLabel.Margin = new System.Windows.Forms.Padding(4);
            this.folderPathLabel.Name = "folderPathLabel";
            this.folderPathLabel.ReadOnly = true;
            this.folderPathLabel.Size = new System.Drawing.Size(206, 25);
            this.folderPathLabel.TabIndex = 10;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(20, 419);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(121, 17);
            this.label5.TabIndex = 11;
            this.label5.Text = "Hiển thị URL Dự án:";
            // 
            // weekListView
            // 
            this.weekListView.BackColor = System.Drawing.Color.WhiteSmoke;
            this.weekListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.weekListView.HideSelection = false;
            this.weekListView.Location = new System.Drawing.Point(13, 172);
            this.weekListView.Margin = new System.Windows.Forms.Padding(4);
            this.weekListView.Name = "weekListView";
            this.weekListView.Size = new System.Drawing.Size(206, 191);
            this.weekListView.TabIndex = 12;
            this.weekListView.UseCompatibleStateImageBehavior = false;
            this.weekListView.View = System.Windows.Forms.View.Details;
            this.weekListView.MouseClick += new System.Windows.Forms.MouseEventHandler(this.weekListView_MouseClick);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Tên thư mục";
            this.columnHeader1.Width = 259;
            // 
            // folderPathInternWeekLabel
            // 
            this.folderPathInternWeekLabel.BackColor = System.Drawing.Color.Gainsboro;
            this.folderPathInternWeekLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.folderPathInternWeekLabel.Location = new System.Drawing.Point(364, 142);
            this.folderPathInternWeekLabel.Margin = new System.Windows.Forms.Padding(4);
            this.folderPathInternWeekLabel.Name = "folderPathInternWeekLabel";
            this.folderPathInternWeekLabel.ReadOnly = true;
            this.folderPathInternWeekLabel.Size = new System.Drawing.Size(206, 25);
            this.folderPathInternWeekLabel.TabIndex = 13;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(230, 421);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(120, 17);
            this.label6.TabIndex = 11;
            this.label6.Text = "Hiển thị thông báo:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(14, 146);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(161, 17);
            this.label7.TabIndex = 9;
            this.label7.Text = "Danh mục thực tập 8 tuần:";
            // 
            // deleteFolderButton
            // 
            this.deleteFolderButton.Location = new System.Drawing.Point(228, 373);
            this.deleteFolderButton.Margin = new System.Windows.Forms.Padding(4);
            this.deleteFolderButton.Name = "deleteFolderButton";
            this.deleteFolderButton.Size = new System.Drawing.Size(155, 30);
            this.deleteFolderButton.TabIndex = 14;
            this.deleteFolderButton.Text = "Xóa dữ liệu";
            this.deleteFolderButton.UseVisualStyleBackColor = true;
            this.deleteFolderButton.Click += new System.EventHandler(this.deleteFolderButton_Click);
            // 
            // helpButton
            // 
            this.helpButton.BackColor = System.Drawing.Color.DeepSkyBlue;
            this.helpButton.Dock = System.Windows.Forms.DockStyle.Left;
            this.helpButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.helpButton.Font = new System.Drawing.Font("Microsoft YaHei", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.helpButton.ForeColor = System.Drawing.Color.White;
            this.helpButton.Location = new System.Drawing.Point(0, 0);
            this.helpButton.Margin = new System.Windows.Forms.Padding(4);
            this.helpButton.Name = "helpButton";
            this.helpButton.Size = new System.Drawing.Size(11, 589);
            this.helpButton.TabIndex = 15;
            this.helpButton.Text = "i";
            this.helpButton.UseVisualStyleBackColor = false;
            this.helpButton.Click += new System.EventHandler(this.helpButton_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(18, 480);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(4);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(202, 92);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 16;
            this.pictureBox1.TabStop = false;
            // 
            // fileListView
            // 
            this.fileListView.BackColor = System.Drawing.Color.WhiteSmoke;
            this.fileListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader2});
            this.fileListView.HideSelection = false;
            this.fileListView.Location = new System.Drawing.Point(228, 172);
            this.fileListView.Margin = new System.Windows.Forms.Padding(4);
            this.fileListView.Name = "fileListView";
            this.fileListView.Size = new System.Drawing.Size(343, 191);
            this.fileListView.TabIndex = 12;
            this.fileListView.UseCompatibleStateImageBehavior = false;
            this.fileListView.View = System.Windows.Forms.View.Details;
            this.fileListView.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.fileListView_MouseDoubleClick);
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Tên File";
            this.columnHeader2.Width = 373;
            // 
            // GitLogAggregator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(584, 589);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.helpButton);
            this.Controls.Add(this.deleteFolderButton);
            this.Controls.Add(this.folderPathInternWeekLabel);
            this.Controls.Add(this.fileListView);
            this.Controls.Add(this.weekListView);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.folderPathLabel);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.aggregateButton);
            this.Controls.Add(this.resultRichTextBox);
            this.Controls.Add(this.txtInternshipDate);
            this.Controls.Add(this.selectGitFolderButton);
            this.Controls.Add(this.cboAuthorCommit);
            this.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(600, 628);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(600, 628);
            this.Name = "GitLogAggregator";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "GitLogAggregator";
            this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
            this.Load += new System.EventHandler(this.GitLogAggregator_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cboAuthorCommit;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Button selectGitFolderButton;
        private System.Windows.Forms.DateTimePicker txtInternshipDate;
        private System.Windows.Forms.RichTextBox resultRichTextBox;
        private System.Windows.Forms.Button aggregateButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox folderPathLabel;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ListView weekListView;
        private System.Windows.Forms.TextBox folderPathInternWeekLabel;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button deleteFolderButton;
        private System.Windows.Forms.Button helpButton;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.ListView fileListView;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
    }
}

