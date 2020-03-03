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
    public partial class Signup : Form
    {
        private int x;
        public Signup()
        {
            InitializeComponent();
            groupBoxVerification.Visible = false;
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            Login.LoginRef.Visible = true;
            Login.LoginRef.WindowState = FormWindowState.Normal;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
        SqlConnection connection=new SqlConnection(@"Data Source=RAFID;Initial Catalog=User;Integrated Security=True");
        string imgLocation="";
        SqlCommand cmd;

        private void bunifuFlatButton1_Click(object sender, EventArgs e)
        {
            if (this.textBox1.Text != "" && this.textBox2.Text != "" && this.textBox3.Text != "")
            {
               
                if(NumberVerification(textBox1.Text,out x))
                {
                    groupBoxVerification.Visible = true;
                    bunifuFlatButton1.Enabled = false;
                    //MessageBox.Show(x.ToString());
                }
                
            }
            else {
                MessageBox.Show(" Not Complete", "Error");
                }
           
            }

        private void bunifuFlatButton2_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "png files(*.png)|*.png|jpg files(*.jpg)|*.jpg|All files(*.*)|*.*";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                imgLocation = dialog.FileName.ToString();
                pictureBox3.ImageLocation = imgLocation;
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Visible = false;
            Login.LoginRef.Show();
        }

        private void Signup_Load(object sender, EventArgs e)
        {

        }

        private bool NumberVerification(string num, out int code)
        {
            Random rand = new Random();
            int x = rand.Next(1000, 9999);
            code = x;
            bool ok = true;
            using (System.Net.WebClient client = new System.Net.WebClient())
            {
                try
                {
                    string url = "http://smsc.vianett.no/v3/send.ashx?" +
                        "src=+8801717906036&" +
                        "dst=+" + num + "&" +
                        "msg=" + System.Web.HttpUtility.UrlEncode(x.ToString(), Encoding.GetEncoding("ISO-8859-1")) + "&" +
                        "username=" + System.Web.HttpUtility.UrlEncode("rss.rafid@gmail.com") + "&" +
                        "password=" + System.Web.HttpUtility.UrlEncode("o459");
                    string result = client.DownloadString(url);
                    if (result.Contains("OK")) { MessageBox.Show("Verification Code Send"); ok = true; }
                }
                catch (Exception ex)
                {
                    //MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK);
                    ok = false;
                }
            }
            if (ok) { return true; }
            else { return false; }
        }

        private void bunifuFlatButtonVerification_Click(object sender, EventArgs e)
        {
            if (txtVer.Text.Equals(x.ToString()))
            {
                try
                {
                    byte[] images = null;
                    FileStream streem = new FileStream(imgLocation, FileMode.Open, FileAccess.Read);
                    BinaryReader brs = new BinaryReader(streem);
                    images = brs.ReadBytes((int)streem.Length);

                    connection.Open();
                    string query = "Insert into users(phoneNumber,userName,password,image)Values('" + textBox1.Text + "','" + textBox2.Text + "','" + textBox3.Text + "',@images) ";
                    cmd = new SqlCommand(query, connection);
                    cmd.Parameters.Add(new SqlParameter("@images", images));
                    int N = cmd.ExecuteNonQuery();
                    connection.Close();
                    MessageBox.Show("Account Created Successfully", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Visible = false;
                    Login.LoginRef.Visible = true;
                    Login.LoginRef.WindowState = FormWindowState.Normal;
                }
                catch
                {
                    MessageBox.Show("This PhoneNumber is already taken", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Wrong Verification code!", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }
    }
}

