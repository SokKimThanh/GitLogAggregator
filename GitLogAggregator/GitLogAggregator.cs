﻿using GitLogAggregator.GUI;
using GitLogAggregator.Utilities;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace GitLogAggregator
{
    /// <summary>
    /// Ứng dụng Windows Forms (C#) này tổng hợp và phân tích dữ liệu commit từ Git dự án. 
    /// Nó tạo báo cáo commit theo tuần từ ngày bắt đầu dự án cho đến hết 8 tuần thực tập.
    /// </summary>
    public partial class GitLogAggregator : Form
    {


        public GitLogAggregator()
        {
            InitializeComponent();
            this.IsMdiContainer = true; // Thiết lập form cha là MDI Container
        }


        private void GitLogAggregator_Load(object sender, EventArgs e)
        {

            // Load hình ảnh vào ToolStrip
            LoadImageToolStrip();
            OpenChildFormFullScreen();
        }
        /// <summary>
        /// Load hình ảnh vào ToolStrip
        /// </summary>
        private void LoadImageToolStrip()
        {
            // Ví dụ sử dụng cho Crud Icon
            var sortIcon = ButtonImageMap.GetButtonImage(toolStripImageList, BtnIconActionMDIEnum.SortIcon);

            // Ví dụ sử dụng cho MDI Action Icon
            var closeAllIcon = ButtonImageMap.GetButtonImage(toolStripImageList, BtnIconActionMDIEnum.CloseAllIcon);

            // Gán hình cho CloseAllToolStripMenuItem
            CloseAllToolStripMenuItem.Image = closeAllIcon;

            // Gán hình cho TileVerticalToolStripMenuItem
            TileVerticalToolStripMenuItem.Image = sortIcon;
        }

        // Sắp xếp các form con theo kiểu Tile (trong form cha)
        private void TileVerticalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.LayoutMdi(MdiLayout.TileVertical);
        }

        // Đóng tất cả form con
        private void CloseAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form childForm in this.MdiChildren)
            {
                childForm.Close();
            }
        }

        private void MainFormToolStripItem_Click(object sender, EventArgs e)
        {
            OpenChildFormFullScreen();
        }

        private void OpenChildFormFullScreen()
        {
            // Kiểm tra xem UserControl đã được thêm vào Panel chưa
            var existingControl = panelContainer.Controls.OfType<ucMainForm>().FirstOrDefault();

            if (existingControl == null)
            {
                // Tạo mới UserControl
                ucMainForm mainFormControl = new()
                {
                    Dock = DockStyle.Fill // Lấp đầy Panel
                };

                // Thêm UserControl vào Panel
                panelContainer.Controls.Add(mainFormControl);
            }
            else
            {
                // Đưa UserControl lên trước nếu đã tồn tại
                existingControl.BringToFront();
            }
        }

        private void BtnUpdateCommit_Click(object sender, EventArgs e)
        {
            OpenOrBringToFrontControl<ucCommit>();
        }

        private void BtnExportReport_Click(object sender, EventArgs e)
        {
            OpenOrBringToFrontControl<ucReport>();
        }

        private void OpenOrBringToFrontControl<TControl>() where TControl : UserControl, new()
        {
            // Kiểm tra xem UserControl đã tồn tại trong panelContainer chưa
            var existingControl = panelContainer.Controls.OfType<TControl>().FirstOrDefault();

            if (existingControl == null)
            {
                // Xóa tất cả các UserControl hiện có trong panelContainer
                foreach (Control controll in panelContainer.Controls.OfType<UserControl>().ToList())
                {
                    panelContainer.Controls.Remove(controll);
                    controll.Dispose(); // Giải phóng tài nguyên
                }

                // Tạo mới UserControl
                TControl control = new TControl();
                control.Dock = DockStyle.Fill; // Lấp đầy Panel

                // Thêm UserControl vào Panel
                panelContainer.Controls.Add(control);
            }
            else
            {
                // Đưa UserControl lên trước nếu đã tồn tại
                existingControl.BringToFront();
            }
        }


    }
}

