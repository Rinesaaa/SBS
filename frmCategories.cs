using AnyStore.BILLL;
using AnyStore.BLL;
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
    public partial class frmCategories : Form
    {
        public frmCategories()
        {
            InitializeComponent();
        }

        private void frmCategories_Load(object sender, EventArgs e)
        {
            DataTable dt = dal.Select();
            dgvCategories.DataSource = dt;
        }

        private void pictureBoxClose_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        categoriesBLL c = new categoriesBLL();
        categoriesDAL dal = new categoriesDAL();
        UserDAL udal = new UserDAL();


        private void btnADD_Click(object sender, EventArgs e)
        {
            //Get the values from Category form

            c.title = txtTitle.Text;
            c.description = txtDescription.Text;
            c.added_date = DateTime.Now;

            // Getting ID Added by field
            string loggedUser = frmLogin.loggedIn;
            UserBLL usr = udal.GetIdFromUsername(loggedUser);
            // Passign the id of logged in user in added by field
            c.added_by = usr.id;
            // Creating Boolean Method To Insert data into database
            bool success = dal.Insert(c);
            // If the catagory is inserted successfully then the value of the success will be the truth 
            if (success == true)
            {
                //new Category inserted Successfully
                MessageBox.Show("New Category Inserted Succesfully,");
                Clear();
                //refresh datagrid view
                DataTable dt = dal.Select();
                dgvCategories.DataSource = dt;
            }
            else
            {
                // FAiled to insert New Category 
                MessageBox.Show("Failed to insert New Category");

            }


        }
        public void Clear()
        {
            txtCategoryID.Text = "";
            txtTitle.Text = "";
            txtDescription.Text = "";
            txtSearch.Text = "";



        }

        private void dgvCategories_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            //Finding the Row Index of the row Clicked on data grid View
            int RowIndex = e.RowIndex;
            txtCategoryID.Text = dgvCategories.Rows[RowIndex].Cells[0].Value.ToString();
            txtTitle.Text = dgvCategories.Rows[RowIndex].Cells[1].Value.ToString();
            txtDescription.Text = dgvCategories.Rows[RowIndex].Cells[2].Value.ToString();

        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            //get the values from the category form
            c.id = int.Parse(txtCategoryID.Text);
            c.title = txtTitle.Text;
            c.description = txtDescription.Text;
            c.added_date = DateTime.Now;
            // Getting ID Added by field
            string loggedUser = frmLogin.loggedIn;
            UserBLL usr = udal.GetIdFromUsername(loggedUser);
            // Passign the id of logged in user in added by field
            c.added_by = usr.id;

            //Creating Boolean Variable to update categories and checks
            bool success = dal.Update(c);
            //If the category is updated successfully then the value of successs will be true or false
            if (success == true)
            {
                //Category update Successfully
                MessageBox.Show("category Update Successfully");
                Clear();
                //Refresh Data Gid View
                DataTable dt = dal.Select();
                dgvCategories.DataSource = dt;


            }
            else
            {
                //Failed to update Category

                MessageBox.Show("Failed to Update Category"); 
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            //get the id of the caftegory which we want to felete
            c.id = int.Parse(txtCategoryID.Text);

            //creating boolean variable to delete the cattegory
            bool success = dal.Delete(c);

            //if the category id deleted succesfully  then the value of success will be true else it will be false
            if(success==true)
            {
                ///category deleted succesfully
                MessageBox.Show("Category Deleted Succcesfully");
                Clear();
                //refreshing data grid view
                DataTable dt = dal.Select();
                 dgvCategories.DataSource = dt;
            }
            else
            {
                //failed to delete caftegory
                MessageBox.Show("failed to Delete Category");
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            //Get the keywords
            string keywords = txtSearch.Text;

            //File the categories based on keywords
            if (keywords != null)
            { //use search methode to display Categories
                DataTable dt = dal.Search(keywords);
                dgvCategories.DataSource = dt;
            }
            else
            {
                //Use select method to display all categories
                DataTable dt = dal.Select();
                dgvCategories.DataSource = dt;
            }


        }
    }
}
    
