using Authetication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Interfaces
{
    public interface IUserMng
    {
        string Add(string userName, string pass);

        User Get(string userName);
        string ChangePassword(string token, string oldpassword, string newPassword);
    }
}
