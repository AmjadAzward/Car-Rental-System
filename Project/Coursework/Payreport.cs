using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.IO;


namespace Coursework
{
    public partial class Payreport : Form
    {
        public Payreport()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;

        }
        private void crystalReportViewer1_Load(object sender, EventArgs e)
        {

            crystalReportViewer1.ReportSource = @"C:\Users\USER\Desktop\Final\Coursework\Coursework\CrystalReport2.rpt";
            crystalReportViewer1.RefreshReport();
        }
    }
    
}
