using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace The_DVLD_Project.People
{
    public partial class frmFindPerson : Form
    {

        //PersonID عملنا ديليقيشن هنا, يمكن في فورم يدعي هذا الفورم واذا كمل رجع له     
        // Declare a delegate
        public delegate void DataBackEventHandler(object sender, int PersonID);

        // Declare an event using the delegate
        public event DataBackEventHandler DataBack;


        public frmFindPerson()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            // Trigger the event to send data back to the caller form.
            DataBack?.Invoke(this, ctrlPersonCardWithFilter1.PersonID);
        }


        //هذا الايفنت اذا تريد تستخدمه
        //private void ctrlPersonCardWithFilter1_OnPersonSelected(int obj)
        //{
        //    MessageBox.Show("HI");
        //}
    }
}
