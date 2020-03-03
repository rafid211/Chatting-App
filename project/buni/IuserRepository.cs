using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp
{
    interface IuserRepository
    {
        bool Insert(User a);
        bool Update(User a);
        bool Delete(string userName);
        User GetAccount(string userName);
        List<User> GetAllUsers();
    }
}
