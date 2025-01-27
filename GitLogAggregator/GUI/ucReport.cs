using BUS;
using CrystalDecisions.CrystalReports.Engine;
using System;
using System.IO;
using System.Windows.Forms;

namespace GitLogAggregator.GUI
{
    public partial class ucReport : UserControl
    {
        public ucReport()
        {
            InitializeComponent();
        }

        private void ucReport_Load(object sender, EventArgs e)
        {
            LoadReport();
        }

        private void LoadReport()
        {
            try
            {
                string reportPath = LoadReportPath();
                if (string.IsNullOrEmpty(reportPath))
                {
                    MessageBox.Show("Vui lòng chọn file báo cáo.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                using (ReportDocument report = new ReportDocument())
                {
                    report.Load(reportPath);
                    report.SetDataSource(new ReportBUS().GetWorkHistoryData());

                    crystalReportViewer1.ReportSource = report;
                    crystalReportViewer1.Refresh();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải báo cáo: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string LoadReportPath()
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = Path.Combine(Application.StartupPath, "Reports");
                openFileDialog.Filter = "Crystal Reports files (*.rpt)|*.rpt|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 1;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    return openFileDialog.FileName;
                }
            }

            return null;
        }
    }
}