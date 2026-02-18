using DVLD;
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

namespace The_DVLD_Project.People.Controls
{
    public partial class clsPersonCardWithFilter : UserControl
    {
        // نعمل ايفنت في الفورم اللي بيشل هذا بمجرد عملت فايند (بحث) للشخص بيعمل حدث معين وبيرسل لك حقه الاي دي
        // Define a custom event handler delegate with parameters
        public event Action<int> OnPersonSelected;
        // Create a protected method to raise the event with a parameter
        protected virtual void PersonSelected(int PersonID)
        {
            Action<int> handler = OnPersonSelected;
            if (handler != null)
            {
                handler(PersonID); // Raise the event with the parameter
            }
        }



        private bool _ShowAddPerson = true;
        public bool ShowAddPerson
        {
            get
            {
                return _ShowAddPerson;
            }
            set
            {
                //قد تكون صح او غلط
                _ShowAddPerson = value;
                btnAddNewPerson.Visible = _ShowAddPerson;
            }
        }

        private bool _FilterEnabled = true;
        public bool FilterEnabled
        {
            get
            {
                return _FilterEnabled;
            }
            set
            {
                _FilterEnabled = value;
                gbFilters.Enabled = _FilterEnabled;
            }
        }




        public clsPersonCardWithFilter()
        {
            InitializeComponent();
        }





        private int _PersonID = -1;

        public int PersonID
        {
            get { return ctrlPersonCard1.PersonID; }
        }
        //clsPersonCardWithFilter1.SelectedPersonInfo    من برع تعمل كذا توصل للأي دي للشخص

        public clsPerson SelectedPersonInfo
        {
            get { return ctrlPersonCard1.SelectedPersonInfo; }
        }
        //clsPersonCardWithFilter1.SelectedPersonInfo    من برع تعمل كذا توصل لكل حاجة للشخص





        //clsPersonCardWithFilter1.LoadPersonInfo(11)    من برع تعمل كذا, توصل لهذه الفنكشن وتعمل بحث (فكرة حلوه)ة
        public void LoadPersonInfo(int PersonID)
        {

            cbFilterBy.SelectedIndex = 1;
            txtFilterValue.Text = PersonID.ToString();
            FindNow();

        }

        private void FindNow()
        {
            switch (cbFilterBy.Text)
            {
                case "Person ID":
                    ctrlPersonCard1.LoadPersonInfo(int.Parse(txtFilterValue.Text));

                    break;

                case "National No.":
                    ctrlPersonCard1.LoadPersonInfo(txtFilterValue.Text);
                    break;

                default:
                    break;
            }


           
            if (OnPersonSelected != null && FilterEnabled)
                // Raise the event with a parameter
                OnPersonSelected(ctrlPersonCard1.PersonID);

        }
        private void btnFind_Click(object sender, EventArgs e)
        {
            if (!this.ValidateChildren())
            {
                //Here we dont continue becuase the form is not valid
                MessageBox.Show("Some fileds are not valide!, put the mouse over the red icon(s) to see the erro", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;

            }

            FindNow();
        }


        private void clsPersonCardWithFilter_Load(object sender, EventArgs e)
        {

            cbFilterBy.SelectedIndex = 0;
            txtFilterValue.Focus();

        }


        private void btnAddNewPerson_Click(object sender, EventArgs e)
        {
            //DataBackEvent كود الديلقيشن مكتوب في حق اضافة الشخص هناك اذا كمل يعمل استدعاء الى 
            frmAddUpdatePerson frm1 = new frmAddUpdatePerson();
            frm1.DataBack += DataBackEvent; // Subscribe to the event
            frm1.ShowDialog();
        }
        private void DataBackEvent(object sender, int PersonID)
        {
            // Handle the data received

            cbFilterBy.SelectedIndex = 1;
            txtFilterValue.Text = PersonID.ToString();
            ctrlPersonCard1.LoadPersonInfo(PersonID);
        }


        public void FilterFocus()
        {
            txtFilterValue.Focus();
        }
        private void txtFilterValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Check if the pressed key is Enter (character code 13)
            if (e.KeyChar == (char)13)
            {

                btnFind.PerformClick();
            }

            //this will allow only digits if person id is selected
            if (cbFilterBy.Text == "Person ID")
                e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);


        }
        private void txtFilterValue_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtFilterValue.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtFilterValue, "This field is required!");
            }
            else
            {
                //e.Cancel = false;
                errorProvider1.SetError(txtFilterValue, null);
            }

        }


    }
}
