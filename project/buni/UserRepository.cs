using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data;

namespace ChatApp
{
    public class UserRepository
    {
        public bool Insert(User a)
        {
            try
            {
                string query = "INSERT into users VALUES ('" + a.PhoneNumber + "', '" + a.UserName + "', '" + a.Password + "')";
                DataBaseConnection dcc = new DataBaseConnection();
                dcc.ConnectWithDB();
                int x = dcc.ExecuteSQL(query);
                dcc.CloseConnection();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool Delete(string phoneNumber)
        {
            try
            {
                string query = "DELETE from users WHERE phoneNumber = '" + phoneNumber + "'";
                DataBaseConnection dcc = new DataBaseConnection();
                dcc.ConnectWithDB();
                int x = dcc.ExecuteSQL(query);
                dcc.CloseConnection();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public User GetAccount(string phoneNumber)
        {
            string query = "SELECT * from users WHERE phoneNumber= '" + phoneNumber + "'";
            User a = null;
            DataBaseConnection dcc = new DataBaseConnection();
            dcc.ConnectWithDB();
            SqlDataReader sdr = dcc.GetData(query);
            if (sdr.Read())
            {
                a = new User();
                a.PhoneNumber = sdr["phoneNumber"].ToString();
                a.UserName = sdr["userName"].ToString();
                a.Password = sdr["password"].ToString();
            }
            dcc.CloseConnection();
            return a;
        }

        public List<User> GetAllUsers()
        {
            string query = "SELECT * from users";
            List<User> aList = new List<User>();
            DataBaseConnection dcc = new DataBaseConnection();
            dcc.ConnectWithDB();
            SqlDataReader sdr = dcc.GetData(query);
            while (sdr.Read())
            {
                User a = new User();
                a.PhoneNumber = sdr["phoneNumber"].ToString();
                a.UserName = sdr["userName"].ToString();
                a.Password = sdr["password"].ToString();
                a.ImageBinary = sdr["image"].ToString();
                aList.Add(a);
            }
            return aList;
        }

        public bool UserLoginVerification(User a)
        {
            string query = "SELECT * from users WHERE phoneNumber= '" + a.PhoneNumber + "' AND password= '" + a.Password + "'";
            DataBaseConnection dcc = new DataBaseConnection();
            dcc.ConnectWithDB();
            SqlDataReader sdr = dcc.GetData(query);

            if (sdr.Read())
            {
                a.PhoneNumber = sdr["phoneNumber"].ToString();
                a.UserName = sdr["userName"].ToString();
                a.Password = sdr["password"].ToString();
                dcc.CloseConnection();
                return true;
            }
            else
            {
                dcc.CloseConnection();
                return false;
            }
        }



    }
}
