using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GitLogAggregator.Utilities
{
    public class DataGridViewHelper
    {
        private DataGridView _dataGridView;

        public DataGridViewHelper(DataGridView dataGridView)
        {
            _dataGridView = dataGridView;
        }

        // Cấu hình hiển thị đơn giản (chỉ hiển thị CommitMessage)
        public void ConfigureSimpleView()
        {
            foreach (DataGridViewColumn column in _dataGridView.Columns)
            {
                column.Visible = false;
            }
            _dataGridView.Columns["CommitMessage"].Visible = true;
            _dataGridView.Columns["ProjectWeekName"].Visible = true;

            _dataGridView.Columns["ProjectWeekName"].Width = 100;
        }

        // Cấu hình hiển thị đầy đủ
        public void ConfigureFullView()
        {
            foreach (DataGridViewColumn column in _dataGridView.Columns)
            {
                column.Visible = true;
            }
        }

        // Cấu hình chung cho DataGridView
        public void ConfigureDataGridView()
        {
            _dataGridView.MultiSelect = true; // Cho phép chọn nhiều dòng
            _dataGridView.ReadOnly = true; // Không cho phép chỉnh sửa
            _dataGridView.AllowUserToAddRows = false; // Ẩn dòng trống cuối cùng
            _dataGridView.AllowUserToDeleteRows = false; // Không cho phép xóa dòng
            _dataGridView.AllowUserToResizeRows = false; // Không cho phép thay đổi kích thước dòng
            _dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill; // Tự động điều chỉnh kích thước cột
            _dataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect; // Chọn cả dòng
            _dataGridView.DefaultCellStyle.SelectionBackColor = Color.LightBlue; // Màu nền khi chọn
            _dataGridView.DefaultCellStyle.SelectionForeColor = Color.Black; // Màu chữ khi chọn
        }
    }
}
