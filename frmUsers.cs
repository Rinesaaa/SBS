using AnyStore.BILLL;
using AnyStore.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AnyStore.Ul
{
    public partial class frmUsers : Form
    {
        public frmUsers()
        {
            InitializeComponent();
        }

         UserBLL u = new UserBLL();
         UserDAL dal = new UserDAL();

        private void pictureBoxClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label3_Click(object sender, EventArgs e)
        {
            Console.WriteLine(txtFirstName.Text);
        }

        private void label4_Click(object sender, EventArgs e)
        {
            /* //getting data from UI
            u.first_name = txtFirstName.Text;
            u.last_name = txtLastName.Text;
            u.email = txtEmail.Text;
            u.username = txtUsername.Text;
            u.password = txtPassword.Text;
            u.contact = txtContact.Text;
            u.address = txtAdress.Text;
            u.gender = cmbGender.Text;
            u.user_type = cmbUserType.Text;
            u.added_date = DateTime.Now;
            u.added_by = 1;

            Console.WriteLine(u);

            //inserting data into database
            bool success = dal.Insert(u);
            //if the data is successfully inserted then the value of success will be true else it will be false
            if(success==true)
            {
                //data successfully inserted
                MessageBox.Show("User successfully created.");
                clear();
            }
            else
            {
                //failed to insert data
                MessageBox.Show("Failed to add new user");
            }
            //refreshing data grid view
            DataTable dt = dal.Select();
            dgvUsers.DataSource = dt; */
        }

        private void frmUsers_Load(object sender, EventArgs e)
        {
            DataTable dt = dal.Select();
            dgvUsers.DataSource = dt;
        }
        private void clear()
        {
            txtUserID.Text = "";
            txtFirstName.Text = "";
            txtLastName.Text = "";
            txtEmail.Text = "";
            txtUsername.Text = "";
            txtPassword.Text = "";
            txtContact.Text = "";
            txtAdress.Text = "";
            cmbGender.Text = "";
            cmbUserType.Text = "";
        }

        private void dgvUsers_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            //get the index of particular row
            int rowIndex = e.RowIndex;
            txtUserID.Text = dgvUsers.Rows[rowIndex].Cells[0].Value.ToString();
            txtFirstName.Text = dgvUsers.Rows[rowIndex].Cells[1].Value.ToString();
            txtLastName.Text = dgvUsers.Rows[rowIndex].Cells[2].Value.ToString();
            txtEmail.Text = dgvUsers.Rows[rowIndex].Cells[3].Value.ToString();
            txtUsername.Text = dgvUsers.Rows[rowIndex].Cells[4].Value.ToString();
            txtPassword.Text = dgvUsers.Rows[rowIndex].Cells[5].Value.ToString();
            txtContact.Text = dgvUsers.Rows[rowIndex].Cells[6].Value.ToString();
            txtAdress.Text = dgvUsers.Rows[rowIndex].Cells[7].Value.ToString();
            cmbGender.Text = dgvUsers.Rows[rowIndex].Cells[8].Value.ToString();
            cmbUserType.Text = dgvUsers.Rows[rowIndex].Cells[9].Value.ToString();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            //get the values from user UI
            u.id = Convert.ToInt32(txtUserID.Text);
            u.first_name = txtFirstName.Text;
            u.last_name = txtLastName.Text;
            u.email = txtEmail.Text;
            u.username = txtUsername.Text;
            u.password = txtPassword.Text;
            u.contact = txtContact.Text;
            u.address = txtAdress.Text;
            u.gender = cmbGender.Text;
            u.user_type = cmbUserType.Text;
            u.added_date = DateTime.Now;
            u.added_by = 1;

            //updating dta into database
            bool success = dal.Update(u);
            //if data is updated successfully then the value of success will be true else it will be false
            if (success == true)
            {
                //data updated successfully
                MessageBox.Show("user successfully updated");
                clear();
            }
            else
            {
                //failed to update user
                MessageBox.Show("failed to update user");
            }
            //refreshing data to grid view
            DataTable dt = dal.Select();
            dgvUsers.DataSource = dt;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            //getting user id from form
            u.id = Convert.ToInt32(txtUserID.Text);

            bool success = dal.Delete(u);
            //if data is deleted then the value of success will be true else it will be false
            if (success == true)
            {
                //user deleted successfully
                MessageBox.Show("user deleted successfully");
                clear();
            }
            else
            {
                //failed to delete user
                MessageBox.Show("failed to delete user");
            }
            //refreshing datagrid view
            DataTable dt = dal.Select();
            dgvUsers.DataSource = dt;

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {

            //getting data from UI
            u.first_name = txtFirstName.Text;
            u.last_name = txtLastName.Text;
            u.email = txtEmail.Text;
            u.username = txtUsername.Text;
            u.password = txtPassword.Text;
            u.contact = txtContact.Text;
            u.address = txtAdress.Text;
            u.gender = cmbGender.Text;
            u.user_type = cmbUserType.Text;
            u.added_date = DateTime.Now;

            //getting username of he logged in user
            string loggedUser = frmLogin.loggedIn;

            UserBLL usr =dal.GetIdFromUsername(loggedUser);
            u.added_by = usr.id;

            Console.WriteLine(u);

            //inserting data into database
            bool success = dal.Insert(u);
            //if the data is successfully inserted then the value of success will be true else it will be false
            if (success == true)
            {
                //data successfully inserted
                MessageBox.Show("User successfully created.");
                clear();
            }
            else
            {
                //failed to insert data
                MessageBox.Show("Failed to add new user");
            }
            //refreshing data grid view
            DataTable dt = dal.Select();
            dgvUsers.DataSource = dt;

        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            //get keyword from text box
            string keywords = txtSearch.Text;

            //chec if the keywords has value or not
            if(keywords!=null)
            {
                //show user based on keywords
                DataTable dt = dal.Search(keywords);
                dgvUsers.DataSource = dt;
            }
            else
            {
                //show all users from the database
                DataTable dt = dal.Select();
                dgvUsers.DataSource = dt;
            }
        }

        private void lblUserID_Click(object sender, EventArgs e)
        {

        }
    }
}
