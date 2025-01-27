using BUS;
using CrystalDecisions.CrystalReports.Engine;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GitLogAggregator.GUI
{
    public partial class ucReport : UserControl
    {
        public ucReport()
        {
            InitializeComponent();
        }

        private void uReport_Load(object sender, EventArgs e)
        {
            try
            {
                ReportDocument report = new();
                report.Load(Path.Combine(Application.StartupPath, "Reports\\CrystalReport1.rpt"));

                report.SetDataSource(new ReportBUS().GetWorkHistoryData());

                crystalReportViewer1.ReportSource = report;
                crystalReportViewer1.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
