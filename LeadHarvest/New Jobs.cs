using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LeadHarvest
{
    public partial class New_Jobs : Form
    {
        DataTable dt=new DataTable();
        public New_Jobs(DataTable dtJobs)
        {
            InitializeComponent();
            dt=dtJobs;

            dataGridOpp.DataSource=dt;            

            this.dataGridOpp.Columns["searchid"].Visible=false;
            this.dataGridOpp.Columns["oppid"].Visible=false;
            this.dataGridOpp.Columns["orgid"].Visible=false;
        }

        private void New_Jobs_Load(object sender, EventArgs e)
        {

        }
    }
}
