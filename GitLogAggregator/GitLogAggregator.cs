using GitLogAggregator.GUI;
using GitLogAggregator.Utilities;
using System;
using System.Collections.Generic;
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
        private readonly Dictionary<Type, UserControl> cachedControls = [];


        public GitLogAggregator()
        {
            InitializeComponent();
            this.IsMdiContainer = true; // Thiết lập form cha là MDI Container
        }


        private void GitLogAggregator_Load(object sender, EventArgs e)
        {

            // Load hình ảnh vào ToolStrip
            LoadImageToolStrip();
            OpenOrBringToFrontControl<ucMainForm>();
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

        private void BtnUpdateCommit_Click(object sender, EventArgs e)
        {
            OpenOrBringToFrontControl<ucCommit>();
        }
        private void BtnMainForm_Click(object sender, EventArgs e)
        {
            OpenOrBringToFrontControl<ucMainForm>();
        }
        private void BtnExportReport_Click(object sender, EventArgs e)
        {
            OpenOrBringToFrontControl<ucReport>();
        }


        private void OpenOrBringToFrontControl<TControl>() where TControl : UserControl, new()
        {
            Type controlType = typeof(TControl);

            // Kiểm tra xem UserControl đã được cache chưa
            if (!cachedControls.TryGetValue(controlType, out UserControl control))
            {
                // Tạo mới UserControl và cache lại
                control = new TControl
                {
                    Dock = DockStyle.Fill
                };
                cachedControls.Add(controlType, control);

                // Thêm UserControl vào Panel (nhưng ẩn đi)
                panelContainer.Controls.Add(control);
                control.Visible = false;
            }

            // Ẩn tất cả các UserControl khác
            foreach (UserControl uc in cachedControls.Values)
            {
                uc.Visible = false;
            }

            // Hiển thị UserControl mong muốn
            control.Visible = true;
            control.BringToFront();
            control.BackColor = SystemColors.ControlLight;
        }
    }
}

