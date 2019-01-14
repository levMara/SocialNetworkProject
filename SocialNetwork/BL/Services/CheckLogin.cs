using Authetication.Models;
using BL.Dal;
using BL.Exceptiones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services
{
    class CheckLogin
    {
        UserService _userService;

        public CheckLogin()
        {
            _userService = new UserService();
        }
        public User Authentication(string userName, string pass)
        {
            User tmp = _userService.Get(userName);
            if (tmp == null)
                throw new IncorrectDetailsException("Incorrect user name or pass");
            
            if (tmp.Password!= pass)
                throw new IncorrectDetailsException("Incorrect user name or pass");

            return tmp;
        }   
    }
}
