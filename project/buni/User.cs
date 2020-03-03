using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
namespace ChatApp
{
    public class User
    {

        private string phoneNumber;
        private string userName;
        private string password;
        private string imageBinary;

        public string PhoneNumber
        {
            get { return phoneNumber; }
            set { phoneNumber = value; }
        }
        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }
        public string Password
        {
            get { return password; }
            set { password = value; }
        }
        public string ImageBinary
        {
            get { return imageBinary; }
            set { imageBinary = value; }
        }

    }
}
