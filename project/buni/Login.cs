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
using System.Speech.Recognition;
using System.Speech.Synthesis;
using System.Web;
using System.Net;
namespace ChatApp
{
    public partial class Login : Form
    {
       
        private static List<UserData> userList;
        private string passnumber;
        private UserData uData;
        private static Login loginRef=null;
        public Login()
        {
            InitializeComponent();
            userList = new List<UserData>();
            loginRef = this;
            
        }

        public static Login LoginRef
        {
            get
            {
                if (loginRef == null) { loginRef = new Login(); }
                return loginRef;
            }
        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Application.Exit();

        }

        private void bunifuFlatButton1_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.txtPhone.Text))
            {
                this.uData = new UserData();
                User usr = new User();
                usr.PhoneNumber = this.txtPhone.Text;
                usr.Password = this.txtPassward.Text;

                UserRepository userepo = new UserRepository();

                if (userepo.UserLoginVerification(usr))
                {   
                    uData.Name = usr.UserName;
                    uData.PhoneNum = usr.PhoneNumber;
                    userList.Add(uData);
                    passnumber = this.txtPhone.Text;
                    WindowState = FormWindowState.Minimized;
                    UserForm uf = new UserForm(userList,usr,uData);
                    uf.Show();
                    uf.ConnetcToServer();
                    this.txtPhone.Text = string.Empty;
                    this.txtPassward.Text = string.Empty;
                }
                else
                {
                    MessageBox.Show("Invalid Phone number or Password", "Login Failed");
                }

            }
        }
        private void Login_Load(object sender, EventArgs e)
        {
            ChatServer s = new ChatServer();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Visible = false;
            Signup sup = new Signup();
            sup.Visible = true;
        }
    }
}
