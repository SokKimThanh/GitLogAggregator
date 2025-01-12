using ClosedXML.Excel;
using ET;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace DAL
{
    public class GitLogCheckCommitDAL
    {
        
        // Method to convert List<WeekData> to DataTable
        public DataTable ConvertToDataTable(List<WeekData> weekDataList)
        {
            // Create a new DataTable with specified columns
            var dataTable = new DataTable();
            dataTable.Columns.Add("Tuần", typeof(int));
            dataTable.Columns.Add("Thứ", typeof(string));
            dataTable.Columns.Add("Buổi", typeof(string));
            dataTable.Columns.Add("Điểm danh vắng", typeof(string));
            dataTable.Columns.Add("Công việc được giao", typeof(string));
            dataTable.Columns.Add("Nội dung – kết quả đạt được", typeof(string));
            dataTable.Columns.Add("Nhận xét - đề nghị của người hướng dẫn tại doanh nghiệp", typeof(string));
            dataTable.Columns.Add("Ghi chú", typeof(string));

            // Iterate through each WeekData and DayData to add rows to the DataTable
            foreach (var weekData in weekDataList)
            {
                foreach (var dayData in weekData.DayDataList)
                {
                    dataTable.Rows.Add(
                        weekData.WeekNumber,
                        dayData.DayOfWeek,
                        dayData.Session,
                        dayData.Attendance,
                        dayData.AssignedTasks,
                        dayData.AchievedResults,
                        dayData.Comments,
                        dayData.Notes
                    );
                }
            }

            return dataTable;
        }

        public void InitializeDataGridView(DataGridView dataGridViewCommits)
        {
            // Đặt chế độ tự động điều chỉnh cột để chiếm 100% không gian
            dataGridViewCommits.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Định dạng các cột
            foreach (DataGridViewColumn column in dataGridViewCommits.Columns)
            {
                column.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            }
            dataGridViewCommits.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
        }
    }
}
