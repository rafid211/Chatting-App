using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;
namespace ChatApp
{
    public partial class EditProfile : Form
    {
        
        public EditProfile()
        {
            InitializeComponent();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            Login.LoginRef.Visible = true;
            Login.LoginRef.WindowState = FormWindowState.Normal;
        }
        SqlConnection connection = new SqlConnection(@"Data Source=RAFID;Initial Catalog=User;Integrated Security=True");
        SqlCommand cmd;

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void bunifuFlatButton1_Click(object sender, EventArgs e)
        {

            if (textBox1.Text != "")
            {
                try
                {
                    connection.Open();
                    string query = "UPDATE users SET userName='" + textBox2.Text + "',password='" + textBox3.Text + "' where phonenumber='" + textBox1.Text + "'";
                    cmd = new SqlCommand(query, connection);

                    cmd.ExecuteNonQuery();
                    connection.Close();
                    MessageBox.Show("Successfully Change and please login");
                    this.Visible = false;
                    Login.LoginRef.Visible = true;
                    Login.LoginRef.WindowState = FormWindowState.Normal;
                }
                catch
                {
                    MessageBox.Show("Can not change your name and password");
                }
            }
            else {
                MessageBox.Show("PLease enter your phoneNumber to change");
            }

        }
    }
}
