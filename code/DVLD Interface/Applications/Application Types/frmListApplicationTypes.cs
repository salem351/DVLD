using DVLD_MyBusiness;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.Applications
{
    public partial class frmListApplicationTypes : Form
    {

        private DataTable _dtAllApplicationTypes;

        public frmListApplicationTypes()
        {
            InitializeComponent();
        }

        //private void _RefreshApplicationTypesList()
        //{
        //    //dataGridViewApplications.DataSource = clsApplicationType.GetAllApplicationTypes();
        //    //dataGridViewApplications.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

        //}

        private void frmListApplicationTypes_Load(object sender, EventArgs e)
        {

            _dtAllApplicationTypes = clsApplicationType.GetAllApplicationTypes();
            dgvApplicationTypes.DataSource = _dtAllApplicationTypes;
            lblRecordsCount.Text = dgvApplicationTypes.Rows.Count.ToString();

            dgvApplicationTypes.Columns[0].HeaderText = "ID";
            dgvApplicationTypes.Columns[0].Width = 110;

            dgvApplicationTypes.Columns[1].HeaderText = "Title";
            dgvApplicationTypes.Columns[1].Width = 400;

            dgvApplicationTypes.Columns[2].HeaderText = "Fees";
            dgvApplicationTypes.Columns[2].Width = 100;



            //DataTable dt = clsApplicationType.GetAllApplicationTypes();

            //dataGridViewApplications.DataSource = dt;
            //dataGridViewApplications.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            //lbl_RecordApplications.Text = dt.Rows.Count.ToString();

            //_RefreshApplicationTypesList();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void editApplicationTypeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmEditApplicationType UpdateApplicationType = new frmEditApplicationType((int)dgvApplicationTypes.CurrentRow.Cells[0].Value);
            UpdateApplicationType.ShowDialog();


            frmListApplicationTypes_Load(null, null);


            //_RefreshApplicationTypesList();
        }
    }
}
