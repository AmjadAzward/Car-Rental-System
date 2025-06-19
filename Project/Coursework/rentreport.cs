using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Coursework
{
    public partial class rentreport : Form
    {
        public rentreport()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void crystalReportViewer1_Load(object sender, EventArgs e)
        {
            crystalReportViewer1.ReportSource = @"C:\Users\USER\Desktop\Final\Coursework\Coursework\CrystalReport1.rpt";
            crystalReportViewer1.RefreshReport();

        }
    }
}
