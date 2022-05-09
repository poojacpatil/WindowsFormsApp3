using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient; //add namespace

namespace WindowsFormsApp3
{
    public partial class Form1 : Form
    {
        SqlConnection con;
        SqlCommand cmd;
        SqlDataReader dr;

        public void ClearAll()
        {
            txtId.Clear();
            txtName.Clear();
            txtPrice.Clear();
        }
    
        public Form1()
        {
            InitializeComponent();
            con = new SqlConnection(@"Server=DESKTOP-AFRADQ0\SQLEXPRESS;database=TQ;Integrated Security=True");
        }

        //Save
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                //while writing query follow the col sequence
                string qry = "insert into product values(@id,@name,@price)";
                //this configuration is toassign query & connection details to command
                //so that qry will be executed on the given connection
                cmd = new SqlCommand(qry, con);
                //assign values to the parameter
                //no need to follow the sequence
                cmd.Parameters.AddWithValue("@id", Convert.ToInt32(txtId.Text));
                cmd.Parameters.AddWithValue("@name", txtName.Text);
                cmd.Parameters.AddWithValue("@price", Convert.ToInt32(txtPrice.Text));
                //open DB connection
                con.Open();
                //first the query
                int res = cmd.ExecuteNonQuery();
                if (res == 1)
                {
                    MessageBox.Show("Record inserted");
                    ClearAll();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                con.Close();
            }
        }

        //Search
        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                string qry = "select * from product where Id=@id";
                cmd = new SqlCommand(qry, con);
                cmd.Parameters.AddWithValue("@id", Convert.ToInt32(txtId.Text));
                con.Open();
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        txtName.Text = dr["Name"].ToString(); //["Name"]should match col name
                        txtPrice.Text = dr["Price"].ToString();
                    }
                }
                else
                {
                    MessageBox.Show("Record not found");
                }

            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                con.Close();
            }
        }

        //Update
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                string qry = "update product set Name=@name,Price=@price where Id=@id";
                cmd = new SqlCommand(qry, con);
                cmd.Parameters.AddWithValue("@id", Convert.ToInt32(txtId.Text));
                cmd.Parameters.AddWithValue("@name", txtName.Text);
                cmd.Parameters.AddWithValue("@price", Convert.ToInt32(txtPrice.Text));
                con.Open();
                int res = cmd.ExecuteNonQuery();
                if (res == 1)
                {
                    MessageBox.Show("Record inserted");
                }
                else
                {
                    MessageBox.Show("Record not found");
                }

            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                con.Close();
            }
        }

        //Add New
        private void btnAddNew_Click(object sender, EventArgs e)
        {
            try
            {
                string qry = "select max(Id) from product";
                cmd=new SqlCommand(qry, con);
                con.Open();
                object obj=cmd.ExecuteScalar();
                if(obj==DBNull.Value)//when obj is null or obj does not have value
                {
                    txtId.Text = "2";
                }
                else
                {
                    int id=Convert.ToInt32(obj);
                    id++;
                    txtId.Text = id.ToString(); 
                }
                txtId.Enabled = false;
                txtName.Clear();
                txtPrice.Clear();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally 
            {
                con.Close();
            }
        }

        //Delet
        private void btnDelet_Click(object sender, EventArgs e)
        {
            try
            {
                string qry = "select max(Id)from product";
                cmd=new SqlCommand(qry,con);
                con.Open();
                dr=cmd.ExecuteReader();
                if(dr.HasRows)
                {
                    while(dr.Read())
                    {
                        txtName.Text = dr["Name"].ToString();
                        txtPrice.Text=dr["Price"].ToString();
                    }
                }
                else
                {
                    MessageBox.Show("Record not found");
                    ClearAll();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                con.Close();
            }
        }

        //Show All Product
        private void btnShowAllProduct_Click(object sender, EventArgs e)
        {
            try
            {
                string qry = "select * from product";
                cmd = new SqlCommand(qry, con);
                con.Open();
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                   DataTable table=new DataTable();
                    table.Load(dr);
                    dataGridView1.DataSource = table;   
                }
                else
                {
                    MessageBox.Show("Record not found");
                    ClearAll();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                con.Close();        
            }
        }

        private void dataGridView1_Click(object sender, EventArgs e)
        {
            txtId.Text =dataGridView1.CurrentRow.Cells[0].Value.ToString(); 
            txtName.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            txtPrice.Text=dataGridView1.CurrentRow.Cells[2].Value.ToString();   
        }
    }
}

