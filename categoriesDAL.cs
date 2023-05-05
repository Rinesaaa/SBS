using AnyStore.BLL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AnyStore.DAL
{
    class categoriesDAL
    {
        static string myconnstrng = ConfigurationManager.ConnectionStrings["connstrng"].ConnectionString;

        public DataTable Search(string keywords)
        {
            //SQL Connection For Database Connection
            SqlConnection conn = new SqlConnection(myconnstrng);

            //Creating Data Table to hold the data from database temporarily
            DataTable dt = new DataTable();

            try
            {

                //SQL Query to search Catagories from Database
                String sql = "SELECT* FROM tbl_categories WHERE id LIKE '%" + keywords + "%' OR title LIKE '%" + keywords + "%' OR description LIKE '%" + keywords + "%'";
                //Creating SQL Commnad to execute the query
                SqlCommand cmd = new SqlCommand(sql, conn);

                //Getting Database Database
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);

                //open databaseConnection
                conn.Open();
                // Passing values from adapter to data tabel dt
                adapter.Fill(dt);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);


            }

            finally
            {
                conn.Close();
            }

            return dt;
        }

        // add insert update select 
    }
}




