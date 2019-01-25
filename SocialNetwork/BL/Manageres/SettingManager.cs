using Authetication.Models;
using BL.Dal;
using BL.Exceptiones;
using BL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Manageres
{
    public class SettingManager : ISetting
    {
        IPassCheck _passCheck;//????? & unity 
        UserService _userService;

        public SettingManager(IPassCheck passCheck)
        {
            _passCheck = passCheck;
            _userService = new UserService();
        }

        //levi
        public void ChangePassword(string userId, string newPass)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(newPass))
                throw new IncorrectDetailsException("User id or new pass empty.");
         
            bool isValid = _passCheck.IsPassValid(newPass);
            if (isValid == false)
                throw new IncorrectDetailsException("New pass not valid.");

            User user =_userService.Get(userId);
            if (user == null)
                throw new IncorrectDetailsException("User not found");

            bool isNew = _passCheck.IsNewPass(user.Password, newPass);
            if (isNew == false)
                throw new IncorrectDetailsException("The new pass should be different from the old");

            user.Password = newPass;
            _userService.Update(user);

            //messge ??
        }

       
    }
}
