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
    public class RegisterService : IUserNameCheck, IPassCheck
    {
        UserService _userService;

        public RegisterService()
        {
            _userService = new UserService();
        }

        public void NewUserCheck(string userName, string pass)
        {
            bool isValid = IsUserNameValid(userName);
            if (!isValid)
              throw new DetailsNotValidException("User name not valid");

            bool isExist = IsUserNameExist(userName);
            if (isExist)
                throw new DetailsNotValidException("User name occupied");

            bool isPassValid = IsPassValid(pass);
            if (!isPassValid)
                throw new DetailsNotValidException("Password not valid");
        }

        public bool IsPassValid(string pass)
        {
            if (pass.Length < 3)
                return false;
            return true;
        }

        public bool IsUserNameExist(string userName)
        {
            var user = _userService.Get(userName);

            if (user == null)
                return false;
            return true;
        }

        public bool IsUserNameValid(string userName)
        {
            if (userName.Length < 3)
                return false;
            return true;
        }

        public bool IsNewPass(string oldPass, string newPass)
        {
            if (oldPass == newPass)
                return false;
            return true;
        }
    }
}
