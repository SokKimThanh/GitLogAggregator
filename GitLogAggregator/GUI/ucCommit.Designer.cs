namespace GitLogAggregator.GUI
{
    partial class ucCommit
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
            this.comboBoxWeeks = new System.Windows.Forms.ComboBox();
            this.txtWeekStartDate = new System.Windows.Forms.DateTimePicker();
            this.txtWeekEndDate = new System.Windows.Forms.DateTimePicker();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.panelContainer = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // comboBoxWeeks
            // 
            this.comboBoxWeeks.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxWeeks.FormattingEnabled = true;
            this.comboBoxWeeks.Location = new System.Drawing.Point(0, 0);
            this.comboBoxWeeks.Margin = new System.Windows.Forms.Padding(0);
            this.comboBoxWeeks.Name = "comboBoxWeeks";
            this.comboBoxWeeks.Size = new System.Drawing.Size(299, 21);
            this.comboBoxWeeks.TabIndex = 20;
            this.comboBoxWeeks.SelectedIndexChanged += new System.EventHandler(this.ComboBoxWeeks_SelectedIndexChanged);
            // 
            // txtWeekStartDate
            // 
            this.txtWeekStartDate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtWeekStartDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.txtWeekStartDate.Location = new System.Drawing.Point(299, 0);
            this.txtWeekStartDate.Margin = new System.Windows.Forms.Padding(0);
            this.txtWeekStartDate.Name = "txtWeekStartDate";
            this.txtWeekStartDate.Size = new System.Drawing.Size(300, 20);
            this.txtWeekStartDate.TabIndex = 6;
            // 
            // txtWeekEndDate
            // 
            this.txtWeekEndDate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtWeekEndDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.txtWeekEndDate.Location = new System.Drawing.Point(599, 0);
            this.txtWeekEndDate.Margin = new System.Windows.Forms.Padding(0);
            this.txtWeekEndDate.Name = "txtWeekEndDate";
            this.txtWeekEndDate.Size = new System.Drawing.Size(301, 20);
            this.txtWeekEndDate.TabIndex = 7;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panelContainer, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(900, 609);
            this.tableLayoutPanel1.TabIndex = 21;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 3;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
            this.tableLayoutPanel2.Controls.Add(this.comboBoxWeeks, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.txtWeekStartDate, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.txtWeekEndDate, 2, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.Size = new System.Drawing.Size(900, 31);
            this.tableLayoutPanel2.TabIndex = 22;
            // 
            // panelContainer
            // 
            this.panelContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelContainer.Location = new System.Drawing.Point(0, 31);
            this.panelContainer.Margin = new System.Windows.Forms.Padding(0);
            this.panelContainer.Name = "panelContainer";
            this.panelContainer.Size = new System.Drawing.Size(900, 578);
            this.panelContainer.TabIndex = 23;
            // 
            // ucCommit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "ucCommit";
            this.Size = new System.Drawing.Size(900, 609);
            this.Load += new System.EventHandler(this.UcCommit_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ComboBox comboBoxWeeks;
        private System.Windows.Forms.DateTimePicker txtWeekStartDate;
        private System.Windows.Forms.DateTimePicker txtWeekEndDate;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Panel panelContainer;
    }
}
