using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp
{
    public class SocketPacket
    {
        public Socket Client { get; set; }

        public byte[] Buffer { get; set; }

        public SocketPacket(Socket client)
        {
            this.Client = client;
            this.Buffer = new byte[8000];

        }
    }
}
