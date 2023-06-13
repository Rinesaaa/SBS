using AnyStore.BILLL;
using AnyStore.BLL;
using AnyStore.DAL;
using DGVPrinterHelper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows.Forms;

namespace AnyStore.Ul
{
    public partial class frmPurchaseandSales : Form
    {
        public frmPurchaseandSales()
        {
            InitializeComponent();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
        DeaCustDAL dcDAL = new DeaCustDAL();
        productsDAL pDAL = new productsDAL();
        userDAL uDAL = new userDAL();
        transactionDAL tDAL = new transactionDAL();
        transactionDetailDAL tdDAL = new transactionDetailDAL();


        DataTable transactionDT = new DataTable();
        private productsDAL pDal;
        private object txtProductName;
       // private bool tdDAL;

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void txtAddress_TextChanged(object sender, EventArgs e)
        {

        }

        private void pnlCalculations_Paint(object sender, PaintEventArgs e)
        {

        }

        private void frmPurchaseandSales_Load(object sender, EventArgs e)
        {
            string type = frmUserDashboard.transactionType;
            lblTop.Text = type;

            transactionDT.Columns.Add("Product Name");
            transactionDT.Columns.Add("Rate");
            transactionDT.Columns.Add("Qty");
            transactionDT.Columns.Add("Total");
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
           
        }

        private void txtSearchProduct_TextChanged(object sender, EventArgs e)
        {
            string keyword = txtSearchProduct.Text;

            if (keyword == "")
            {
                txtSearchProduct.Text = "";
                txtInventory.Text = "";
                txtRate.Text = "";
                txtQty.Text = "";
                return;
            }
            productsBLL p = pDAL.GetProductsForTransaction(keyword);

            txtNameProduct.Text = p.name;
            txtInventory.Text = p.qty.ToString();
         txtRate.Text = p.rate.ToString();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string ProductName = txtNameProduct.Text;
            decimal Rate = decimal.Parse(txtRate.Text);
            decimal Qty = decimal.Parse(txtQty.Text);

            decimal Total = Rate * Qty;

            decimal subTotal = decimal.Parse(txtSubTotal.Text);
            subTotal = subTotal + Total;

            if (ProductName == "")
            {
                MessageBox.Show("Select the product first.Try Again.");
            }
            else
            {
                transactionDT.Rows.Add(ProductName, Rate, Qty, Total);

                dgvAddedProducts.DataSource = transactionDT;

                txtSubTotal.Text = subTotal.ToString();

                txtSearchProduct.Text = "";
                txtNameProduct.Text = "";
                txtInventory.Text = "0.00";
                txtRate.Text = "0.00";
                txtQty.Text = "0.00";
            }

        }

        private void txtDiscount_TextChanged(object sender, EventArgs e)
        {
            string value = txtDiscount.Text;

            if (value=="")
            {
                MessageBox.Show("Please Add Discount Fisrt");
            }
            else
            {
                decimal subTotal = decimal.Parse(txtSubTotal.Text);
                decimal discount = decimal.Parse(txtDiscount.Text);

                decimal grandTotal = ((100 - discount) / 100) * subTotal;

                txtGrandTotal.Text = grandTotal.ToString();
            }
        }

        private void txtVat_TextChanged(object sender, EventArgs e)
        {
            string check = txtGrandTotal.Text;
            if (check == "")
            {
                MessageBox.Show("Calculate the discount and set the Grand Total First.");

            }
            else
            {
                decimal previousGT = decimal.Parse(txtGrandTotal.Text);
                decimal vat = decimal.Parse(txtVat.Text);
                decimal grandTotalWithVat = ((100 + vat) / 100) * previousGT;

                txtGrandTotal.Text = grandTotalWithVat.ToString();
            }
        }

        private void txtPaidAmount_TextChanged(object sender, EventArgs e)
        {
            decimal grandTotal = decimal.Parse(txtGrandTotal.Text);
            decimal paidamount = decimal.Parse(txtPaidAmount.Text);

            decimal returnamount = paidamount - grandTotal;

            txtReturnAmount.Text = returnamount.ToString();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            TransactionBLL transaction = new TransactionBLL();

            transaction.type = lblTop.Text;

            string deaCustName = txtName.Text;
            DeaCustBLL dc = dcDAL.GetDeaCustIDFromName(deaCustName);

            transaction.dea_cust_id = dc.id;
            transaction.grandTotal = Math.Round(decimal.Parse(txtGrandTotal.Text),2);
            transaction.transaction_date = DateTime.Now;
            transaction.tax = decimal.Parse(txtVat.Text);
            transaction.discount = decimal.Parse(txtDiscount.Text);

            string username = frmLogin.loggedIn;
            UserBLL u = uDAL.GetIdFromUsername(username);

            transaction.added_by = u.id;
            transaction.TransactionDetail = transactionDT;
            bool success = false;

            using(TransactionScope scope = new TransactionScope())
            {
                int transactionID = -1;

                bool w = tDAL.Insert_Transaction(transaction, out transactionID);

                for (int i = 0; i < transactionDT.Rows.Count; i++)
                {
                    TransactionDetailBLL transactionDetail = new TransactionDetailBLL();

                    string ProductName = transactionDT.Rows[i][0].ToString();
                    productsBLL p = pDAL.GetProductIDFromName(ProductName);

                    transactionDetail.product_id = p.id;
                    transactionDetail.rate = decimal.Parse(transactionDT.Rows[1][1].ToString());
                    transactionDetail.qty = decimal.Parse(transactionDT.Rows[1][2].ToString());
                    transactionDetail.total = Math.Round(decimal.Parse(transactionDT.Rows[1][3].ToString()), 2);
                    transactionDetail.dea_cust_id = dc.id;
                    transactionDetail.added_date = DateTime.Now;
                    transactionDetail.added_by = u.id;

                    string transactionType = lblTop.Text;
                    bool x = false;
                    if(transactionType =="Purchase")
                    {
                        x = pDAL.IncreaseProduct(transactionDetail.product_id, transactionDetail.qty);
                 
                    }
                    else if(transactionType=="Sales")
                    {
                         x = pDAL.DecreaseProduct(transactionDetail.product_id, transactionDetail.qty);
                    }

                    bool y = tdDAL.InsertTransactionDetail(transactionDetail);
                    success = w && x && y;
                }


                    if (success == true)
                        {
                    scope.Complete();


                    DGVPrinter printer = new DGVPrinter();
                    printer.Title = "\r\n\r\n\r\n ANYSTORE PVT. LTD.  \r\n\r\n";
                    printer.SubTitle = "Loni Doors, Fushe Kosova \r\n Phone:045XXXXXXX";
                    printer.SubTitleFormatFlags = StringFormatFlags.LineLimit | StringFormatFlags.NoClip;
                    printer.PageNumbers = true;
                    printer.PageNumberInHeader = false;
                    printer.PorportionalColumns = true;
                    printer.HeaderCellAlignment = StringAlignment.Near;
                    printer.Footer = "Discount:" + txtDiscount.Text + "% \r\n" + "VAT:" + txtVat.Text + "% \r\n" + "Grand Total:"+txtGrandTotal.Text + "\r\n" + "Thank you for doing business with us";
                    printer.FooterSpacing = 15;
                    printer.PrintDataGridView(dgvAddedProducts);







                            MessageBox.Show("Transaction Completed Successfully");

                            dgvAddedProducts.DataSource = null;
                            dgvAddedProducts.Rows.Clear();

                            txtSearch.Text = "";
                            txtName.Text = "";
                            txtEmail.Text = "";
                            txtContact.Text = "";
                            txtAddress.Text = "";
                            txtSearchProduct.Text = "";
                            txtNameProduct.Text = "";
                            txtInventory.Text = "0";
                            txtRate.Text = "0";
                            txtQty.Text = "0";
                            txtSubTotal.Text = "0";
                            txtDiscount.Text = "0";
                            txtVat.Text = "0";
                            txtGrandTotal.Text = "0";
                            txtPaidAmount.Text = "0";
                            txtReturnAmount.Text = "0";

                        }
                        else
                        {
                            MessageBox.Show("Transaction failed");
                        }


                    }


                }
            }
       
        
        
        }

    

