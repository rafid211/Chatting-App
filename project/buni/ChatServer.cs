using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp
{
    public class ChatServer
    {
        private Socket server;
        private List<SocketClient> clients;
        private List<SocketClient> clienList = new List<SocketClient>();
        private string sendTo = "", from = "", message = "";
        public string[] msg;
        private string recvMsg = "";
        SocketPacket p;
        public ChatServer()
        {
            this.clients = new List<SocketClient>();
            this.ServerStart();
        }

        public void ServerStart()
        {
            Console.WriteLine("server start...");
            this.server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            int port = 8000;
            IPAddress ip = IPAddress.Parse(this.GetLocalIP());
            this.server.Bind(new IPEndPoint(ip, port));
            //this.server.Bind(new IPEndPoint(IPAddress.Any, port));
            this.server.Listen(10);
            this.server.BeginAccept(new AsyncCallback(OnClientConnect), null);
        }

        private void OnClientConnect(IAsyncResult ar)
        {
            Socket client = server.EndAccept(ar);
            SocketClient sc = new SocketClient();
            sc.ClientData = client;

            this.clients.Add(sc);
            Console.WriteLine("client connected..");
            this.server.BeginAccept(new AsyncCallback(OnClientConnect), null);
            ReadyForData(client);

        }

        private void ReadyForData(Socket client)
        {

            SocketPacket packet = new SocketPacket(client);
            client.BeginReceive(packet.Buffer, 0, packet.Buffer.Length, SocketFlags.None, new AsyncCallback(OnReceive), packet);
        }

        private void OnReceive(IAsyncResult ar)
        {
            SocketPacket packet = (SocketPacket)ar.AsyncState;

            // Console.WriteLine("on recienve: "+packet.Client.RemoteEndPoint);
            recvMsg = Encoding.Unicode.GetString(packet.Buffer);
            Console.WriteLine("Server receive: " + recvMsg);
            this.WriteToLog(recvMsg);
            ReadyForData(packet.Client);
            //packet.Client.EndReceive(ar);
            packet.Buffer = null;

        }

        private void WriteToLog(string m)
        {
            msg = m.Split('#');
            message = msg[0];
            sendTo = msg[1];
            from = msg[2];
            clienList = UserForm.CLientList;
            //Console.WriteLine("number of user: "+clients.Count);
            for (int i = 0; i < clienList.Count; i++)
            {
                if (clienList[i].ClientData.LocalEndPoint.ToString() == clients[i].ClientData.RemoteEndPoint.ToString())
                {
                    //Console.WriteLine("Match");
                    //Console.WriteLine("Write: "+clients[i].RemoteEndPoint+" ... "+cl.RemoteEndPoint);
                    //clienList[i].ClientData = clients[i];
                    clients[i].UData = clienList[i].UData;
                }
            }
            foreach (var item in clients)
            {
                
                if (item.UData.PhoneNum==sendTo)
                {
                    Console.WriteLine("send by:" + item.UData.PhoneNum.ToString());
                    SendToClient(message, item.ClientData);
                    break;
                }
            }
            msg = null;
        }

        private void SendToClient(string m, Socket cl)
        {
            string s = m;
            byte[] buffer = Encoding.Unicode.GetBytes(s);
            this.p = new SocketPacket(cl);
            p.Buffer = buffer;
            cl.BeginSend(p.Buffer, 0, p.Buffer.Length, SocketFlags.None, new AsyncCallback(SendMessage), p);

            //if (cl.Connected)
            //{
            //    cl.BeginSend(p.Buffer, 0, p.Buffer.Length, SocketFlags.None,new AsyncCallback(SendMessage), p);
            //    return;
            //}          
            //msg = null;sendTo = null;from = null;
            //foreach (Socket client in this.clients)
            //{
            //    if (client.Connected&&client==cl)
            //    {
            //        client.Send(buffer);
            //        //Console.WriteLine("msg send");
            //        break;
            //    }
            //}
        }

        private void SendMessage(IAsyncResult ar)
        {
            SocketPacket sp = (SocketPacket)ar.AsyncState;
            Console.WriteLine("Server send : " + Encoding.ASCII.GetString(sp.Buffer));
            //sp.Client.BeginSend(p.Buffer, 0, p.Buffer.Length, SocketFlags.None, new AsyncCallback(SendMessage), sp);
            sp.Client.EndSend(ar);
        }

        private string GetLocalIP()
        {

            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }

            }
            return "127.0.0.1";
        }
    }
}
