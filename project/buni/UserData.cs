using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp
{
    public class UserData
    {
        private string name;
        private string phoneNum;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public string PhoneNum
        {
            get { return phoneNum; }
            set { phoneNum = value; }
        }
    }
}
