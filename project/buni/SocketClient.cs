using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp
{
    public class SocketClient
    {
        private Socket clientData;
        private UserData uData;

        public UserData UData
        {
            get { return uData; }
            set { uData = value; }
        }

        public Socket ClientData
        {
            get { return clientData; }
            set { clientData = value; }
        }
    }

}
