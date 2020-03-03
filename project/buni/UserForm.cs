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
using System.Net.Sockets;
using System.Net;

namespace ChatApp
{
    public partial class UserForm : Form
    {
        private Socket client;
        private byte[] buffer = new byte[8000];
        private UserData currentUser;
        private bool isPress = true;
        private static List<SocketClient> clientList = new List<SocketClient>();
        private List<UserData> uDatalist = new List<UserData>();
        private List<User> ulist;
        private SpeechRecognitionEngine speech = new SpeechRecognitionEngine();
        private SpeechSynthesizer voice = new SpeechSynthesizer();

        private SqlConnection connection = new SqlConnection(@"Data Source=RAFID;Initial Catalog=User;Integrated Security=True");
        private SqlCommand cmd;
        private SqlDataAdapter da;
        private UserRepository uRepo;

        private static List<UserForm> userFormList = new List<UserForm>();


        public UserForm(List<UserData>userList,User user,UserData ud)
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            uDatalist = userList;
           // CreatFile();
            this.currentUser = ud;
            this.panelContact.Hide();
            this.panelFront.Show();
            this.panelMain.Show();
            labelPname.Text = labelPname.Text + user.UserName;
            labelPhone.Text = user.PhoneNumber;
            this.uRepo = new UserRepository();
            this.Addimage(user);
            bunifuFlatButton5.Visible = false;
            bunifuFlatButtonEdit.Visible = false;
            bunifuFlatButtonCancel.Visible = false;
            speech.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(speechreco_SpeechRecognized);
            Loadgrammer();
            speech.SetInputToDefaultAudioDevice();
            speech.RecognizeAsync(RecognizeMode.Multiple);
           
            txtMsg.Visible = false;
            bunifuFlatButtonSend.Visible = false;
            txtLog.Visible = false;

            pictureBoxFriend.Visible = false;
            labelFrndName.Visible = false;
            labelFrndPhn.Visible = false;

            userFormList.Add(this);
        }

        public static List<SocketClient> CLientList
        {
            get { return clientList; }
        }

        private void CreatFile()
        {
            try
            {
                uRepo = new UserRepository();
                ulist = new List<User>();
                ulist = uRepo.GetAllUsers();
                foreach (var item in ulist)
                {
                    string pathString2 = @"c:\Users\Supto\Desktop\bleh\project ui\History";
                    System.IO.Directory.CreateDirectory(pathString2);
                    string fileName = item.UserName+".txt";
                    string pathString = Path.Combine(pathString2, fileName);
                    Console.WriteLine("Path to my file: {0}\n", pathString);
                    if (!System.IO.File.Exists(pathString))
                    {
                        System.IO.FileStream fs = System.IO.File.Create(pathString);
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /*.............................................//voice//.........................................................*/

        private void Loadgrammer()
        {
            try
            {
                Choices txt = new Choices();
                string[] line = File.ReadAllLines(Environment.CurrentDirectory + "\\commands.txt");
                txt.Add(line);
                Grammar Wordlist = new Grammar(new GrammarBuilder(txt));
                speech.LoadGrammar(Wordlist);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void speechreco_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            //txtMsg.Text = txtMsg.Text + "  " + e.Result.Text;
            string speech = e.Result.Text;
            //txtMsg.Text = txtMsg.Text + "  " + speech;
            switch (speech)
            {
                case ("write text"):
                    voice.Speak("writing text");
                    txtMsg.Visible = true;
                    bunifuFlatButtonSend.Visible = true;
                    txtLog.Visible = true;
                    speech = "";
                    txtMsg.Text = txtMsg.Text + speech;
                    txtMsg.Text = "";
                
                    break;
                case ("close text"):
                    voice.Speak("closing text");
                    txtMsg.Visible = false;
                    bunifuFlatButtonSend.Visible = false;
                    txtLog.Visible = false;
                    speech = "";
                    txtMsg.Text = txtMsg.Text + speech;
                    txtMsg.Text = "";
                    break;
                case ("tom"):
                    voice.Speak("sending text");
                    speech = "";
                    txtMsg.Text = txtMsg.Text+speech;
                    this.bunifuFlatButtonSend.PerformClick();
                    txtMsg.Text = "";
                    break;
                case ("hello"):
                    voice.Speak("hello");
                    break;
                case ("who are you"):
                    voice.Speak("who are you");
                    break;
                case ("who created you"):
                    voice.Speak("who created you");
                    break;
                case ("hi"):
                    voice.Speak("hi");
                    break;
                case ("how are you"):
                    voice.Speak("how are you");
                    break;
                case ("i am fine"):
                    voice.Speak("i am fine");
                    break;
                case ("i am fine you"):
                    voice.Speak("i am fine you");
                    break;

                case ("thank you"):
                    voice.Speak("thats my pleasure");
                    break;

                case ("by"):
                    voice.Speak("by");
                    break;

                case ("fullscreen"):
                    WindowState = FormWindowState.Maximized;
                    break;
                case ("minimize"):
                    WindowState = FormWindowState.Minimized;
                    break;
                case ("normal"):
                    WindowState = FormWindowState.Normal;
                    break;

                case ("cleantext"):
                    voice.Speak("Cleaning text");
                    speech = "";
                    txtMsg.Text = txtMsg.Text + speech;
                    txtMsg.Text = "";
                    break;

            }
            txtMsg.Text = txtMsg.Text + "  " + speech;
        }

        private void  Addimage(User user)
        {
            try
            {
                string query = "SELECT * from users where phoneNumber='" + user.PhoneNumber + "'";
                cmd = new SqlCommand(query, connection);
                da = new SqlDataAdapter(cmd);

                DataTable table = new DataTable();
                da.Fill(table);
                //label1.Text = "Phone Number :" + table.Rows[0][0].ToString();
                //label2.Text = "User Name :" + table.Rows[0][1].ToString();
                byte[] img = (byte[])table.Rows[0][3];
                //MessageBox.Show(img.ToString() + " ok");
                MemoryStream ms = new MemoryStream(img);
                pictureBoxProfile.Image = Image.FromStream(ms);
                da.Dispose();
            }
            catch {
                MessageBox.Show("picture can not load, please edit");
            }
        }

        private void AddimageToChatBox(User user)
        {
            try
            {
                
                string query = "SELECT * from users where phoneNumber='" + user.PhoneNumber + "'";
                cmd = new SqlCommand(query, connection);
                da = new SqlDataAdapter(cmd);

                DataTable table = new DataTable();
                da.Fill(table);
                //label1.Text = "Phone Number :" + table.Rows[0][0].ToString();
                //label2.Text = "User Name :" + table.Rows[0][1].ToString();
                byte[] img = (byte[])table.Rows[0][3];
                //MessageBox.Show(img.ToString() + " ok");
                MemoryStream ms = new MemoryStream(img);
                pictureBoxFriend.Image = Image.FromStream(ms);
                da.Dispose();
            }
            catch
            {
                MessageBox.Show("picture can not load, please edit");
            }
        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
           
        
        private void Form1_Load(object sender, EventArgs e)
        {
           
            
        }

        private void bunifuFlatButton4_Click(object sender, EventArgs e)
        {

            
            Login.LoginRef.WindowState = FormWindowState.Normal;
           
            for(int i = 0; i < userFormList.Count; i++)
            {
                if(userFormList[i]==this)
                {
                   userFormList[i] = null;
                }
                else if(userFormList[i]!=null)
                {
                    userFormList[i].uDatalist.Remove(this.currentUser);
                }
            }
            this.Visible = false;
        }

        private void pictureBox8_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void bunifuFlatButton5_Click(string phoneNumber)
        {
 

        }
       
        private void bunifuFlatButtonEdit_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            EditProfile ep = new EditProfile();
            ep.Visible = true;
        }

        private void bunifuiOSSwitch2_OnValueChange(object sender, EventArgs e)
        {
            if (bunifuiOSSwitch2.Enabled)
            {
                bunifuiOSSwitch2.Enabled = false;
                labelOnline.Text = "Offline";
            }
            else
            {               
                bunifuiOSSwitch2.Enabled = true;
                labelOnline.Text = "Online";
            }
            
           
        }

        private void bunifuFlatButton5_Click(object sender, EventArgs e)
        {
            try
            {
               
                connection.Open();
                string query = "DELETE FROM  users WHERE phoneNumber='" + labelPhone.Text + "'";
                cmd = new SqlCommand(query, connection);

                cmd.ExecuteNonQuery();
                connection.Close();
                for (int i=0;i<userFormList.Count;i++)
                {
                    if (userFormList[i] == this)
                    {
                        userFormList[i] = null;
                    }
                }
                MessageBox.Show("Successfully Deleted hope to see you Again");                
                this.Visible = false;
                Login.LoginRef.Visible = true;
                Login.LoginRef.WindowState = FormWindowState.Normal;
            }
            catch
            {
                MessageBox.Show("can not change your name and password");
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            bunifuFlatButtonCancel.Visible = true;
            
            try
            {
                string query = "SELECT * from users where userName='" + textBox1.Text + "'";
                cmd = new SqlCommand(query, connection);
                da = new SqlDataAdapter(cmd);

                DataTable table = new DataTable();
                da.Fill(table);
                labelFrndName.Text = "Phone Number :" + table.Rows[0][0].ToString();
                labelFrndName.Text = "User Name :" + table.Rows[0][1].ToString();



                byte[] img = (byte[])table.Rows[0][3];
                //MessageBox.Show(img.ToString() + " ok");
                MemoryStream ms = new MemoryStream(img);
                pictureBoxFriend.Image = Image.FromStream(ms);
                da.Dispose();
            }
            catch
            {
                MessageBox.Show("NO USER IS  FOUND");
            }
            textBox1.Text = "";
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            bunifuFlatButton5.Visible = true;
            bunifuFlatButtonEdit.Visible = true;
            bunifuFlatButton4.Visible = false;
            labelProfile.Text = "";
            label3.Visible = true;
            textBox1.Visible = true;
            pictureBox2.Visible = true;
            pictureBoxFriend.Visible = true;
            labelFrndName.Visible = true;
            labelFrndPhn.Visible = true;
            pictureBoxFriend.Image = null;
            labelFrndName.Text = "";
            labelFrndPhn.Text = "";
            bunifuFlatButtonCancel.Visible = false;
            bunifuiOSSwitch2.Enabled = true;
            txtLog.Visible = false;
            panelContact.Visible = false;
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            bunifuFlatButton5.Visible = false;
            bunifuFlatButtonEdit.Visible = false;
            bunifuFlatButton4.Visible = true;
            labelProfile.Text = "";
            label3.Visible = true;
            textBox1.Visible = true;
            pictureBox2.Visible = true;
            pictureBoxFriend.Visible = true;
            labelFrndName.Visible = true;
            labelFrndPhn.Visible = true;
            pictureBoxFriend.Image = null;
            labelFrndName.Text = "";
            labelFrndPhn.Text = "";
            bunifuFlatButtonCancel.Visible = false;
            bunifuiOSSwitch2.Enabled = true;
            txtLog.Visible = false;
            panelContact.Visible = false;
          
        }



        private void pictureBox6_Click_1(object sender, EventArgs e)
        {
            //this.user = user;
            // MessageBox.Show("picture can not load please edit");
            labelProfile.Text = "YOUR PROFILE";
            bunifuFlatButton4.Visible = false;
            bunifuFlatButton5.Visible = false;
            bunifuFlatButtonEdit.Visible = false;
            label3.Visible = false;
            textBox1.Visible = false;
            pictureBox2.Visible = false;
            pictureBoxFriend.Visible = false;
            labelFrndName.Visible = false;
            labelFrndPhn.Visible = false;
            bunifuFlatButtonCancel.Visible = false;
            bunifuiOSSwitch2.Enabled = true;
            txtLog.Visible = false;
            panelContact.Visible = false;
        }

        private void bunifuFlatButtonCancel_Click(object sender, EventArgs e)
        {
            pictureBoxFriend.Image = null;
            labelFrndName.Text = "";
            labelFrndPhn.Text = "";
            bunifuFlatButtonCancel.Visible = false;
            bunifuiOSSwitch2.Enabled = true;

        }

        private void btnCntcMsngr_Click(object sender, EventArgs e)
        {
            isPress = true;
            if (this.panelContact.Visible)
            {
                this.panelContact.Visible = false;
            }
            else
            {
                
                this.listBoxContact.Items.Clear();
                this.panelContact.Show();
                AddFriend();
                this.panelContact.Show();
            }
            
        }
        private static void AddFriend()
        {
            foreach (var item in userFormList)
            {
                if (item != null)
                {
                    for (int i = 0; i < item.uDatalist.Count; i++)
                    {
                        if (item.uDatalist[i]!=null&&item.uDatalist[i].Name != item.currentUser.Name)
                        {
                            item.listBoxContact.Items.Add(item.uDatalist[i].Name);
                        }
                    }
                }
                
            }
        }


        private void btnStart_Click(object sender, EventArgs e)
        {
            try
            {
                if (listBoxContact.Items.Count > 0)
                {
                    this.ulist = new List<User>();
                    ulist = uRepo.GetAllUsers();
                    pictureBoxFriend.Visible = true;
                    labelFrndName.Visible = true;
                    labelFrndPhn.Visible = true;
                    bunifuFlatButtonCancel.Visible = true;
                    User u = null;
                    string item = listBoxContact.SelectedItem.ToString();
                    item.Trim();
                    //Console.WriteLine("selected name: " + item);
                    foreach (var user in ulist)
                    {
                        if (user.UserName == item)
                        {
                            u = user;
                            break;
                        }
                    }
                    Console.WriteLine("selected name: " + u.UserName);
                    this.AddimageToChatBox(u);
                    this.labelFrndName.Text += " " + u.UserName;
                    this.labelFrndPhn.Text += " " + u.PhoneNumber;
                }
            }catch(Exception ex)
            {
                MessageBox.Show("No User Selected!");
            }
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            if (this.panelContact.Visible)
            {
                this.panelContact.Visible = false;
            }
            else
            {
                isPress = false;
                this.listBoxContact.Items.Clear();
                this.ulist = new List<User>();
                ulist = uRepo.GetAllUsers();
                foreach (var item in ulist)
                {
                    if (this.currentUser.Name != item.UserName)
                    {
                        listBoxContact.Items.Add(item.UserName);
                    }

                }
                this.btnStart.Enabled = true;
                this.panelContact.Show();
            }

        }

        /*.............................................//Client_Server//.........................................................*/

        internal void ConnetcToServer()
        {
            Console.WriteLine("connect to server..");
            this.client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPAddress ip = IPAddress.Parse(this.GetLocalIP());
            int port = 8000;
            this.client.Connect(new IPEndPoint(ip, port));
            SocketClient sc = new SocketClient();
            sc.ClientData = client;
            sc.UData = this.currentUser;
            clientList.Add(sc);
           // WriteToLog("Connected..." + ip.ToString(), "");
            WaitingForData(client);
        }

        private void WaitingForData(Socket cl)
        {
            SocketPacket packet = new SocketPacket(cl);
            cl.BeginReceive(packet.Buffer, 0, packet.Buffer.Length, SocketFlags.None, new AsyncCallback(OnReceive), packet);
        }

        private void OnReceive(IAsyncResult ar)
        {
            SocketPacket p = (SocketPacket)ar.AsyncState;
            string msg = Encoding.Unicode.GetString(p.Buffer);
            //string[] s = msg.Split('#');
            //string recfrom = s[1];            
   
            string name = GetSenderName(p);
            this.WriteToLog(msg,name);
            this.WaitingForData(p.Client);

        }

        private string GetSenderName(SocketPacket p)
        {

            foreach (var item in clientList)
            {
                if (item.ClientData.LocalEndPoint.ToString() == p.Client.LocalEndPoint.ToString())
                {

                   // Console.WriteLine(item.ClientData.LocalEndPoint + " equal " + p.Client.LocalEndPoint+"..."+item.UData.Name);
                    //return item.UData.Name.ToString();
                }

            }
            return "user";
        }

        private void WriteToLog(string msg, string name)
        {
            MethodInvoker invoker = new MethodInvoker(delegate
            {
               
                txtLog.Text += name + ": " + msg;
                txtLog.Text += "\r\n";
            });
            this.BeginInvoke(invoker);

        }

        private string GetLocalIP()
        {
            IPHostEntry host;
            host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }

            }
            return "127.0.0.1";
        }

        private void btnVoiceManual_Click(object sender, EventArgs e)
        {
            this.txtMsg.Visible = true;
            this.bunifuFlatButtonSend.Visible = true;
            this.txtLog.Visible = true;
            this.txtMsg.Text = "";
        }

        private void bunifuFlatButtonSend_Click(object sender, EventArgs e)
        {

            try
            {
                string name = listBoxContact.SelectedItem.ToString();
                string ph = "";
                foreach (var item in uDatalist)
                {
                    if (item.Name != currentUser.Name && item.Name == name)
                    {
                        ph = item.PhoneNum;
                        break;
                    }
                }
                string msg = txtMsg.Text + "#" + ph + "#" + this.currentUser.PhoneNum;
                //string msg = txtMsg.Text;
                buffer = Encoding.Unicode.GetBytes(msg);
                this.client.Send(buffer);
                Console.WriteLine("client send: " + msg);
                this.txtLog.Text += "ME: " + txtMsg.Text;
                this.txtLog.Text += "\r\n";
                this.txtMsg.Text = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show("No User is Selected!");
            }
        }

        private void UserForm_closing(object sender, FormClosingEventArgs e)
        {
            this.Close();
        }
    }
}
