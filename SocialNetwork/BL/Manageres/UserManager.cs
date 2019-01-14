using Authetication.Db;
using Authetication.Models;
using BL.Dal;
using BL.Exceptiones;
using BL.Interfaces;
using BL.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Manageres
{
    public class UserManager : ILogin, IUserMng
    {
        //interface
        private readonly UserService _userService;
        private readonly RegisterService _registerService;
        private readonly CheckLogin _checkLogin;
        ITokenMng _tokenMng;

        public UserManager(ITokenMng tokenMng)
        {
            _userService = new UserService();
            _registerService = new RegisterService();
            _checkLogin = new CheckLogin();
            _tokenMng = tokenMng; 
        }

        public string Add(string userName, string pass)
        {
            if (userName == null || pass == null)
                throw new IncorrectDetailsException("User name or pass empty");

            _registerService.NewUserCheck(userName, pass);

            User newUser = new User
            {
                UserName = userName,
                Password = pass,
                UserId = Guid.NewGuid().ToString()
            };

            _userService.Add(newUser);
            string token = _tokenMng.CreateAndSave(newUser);
            return token;
        }

        public string ChangePassword(string token, string oldpassword, string newPassword)
        {
            TokenManager tm = new TokenManager();
                 if (string.IsNullOrEmpty(token) || newPassword == null)
                    throw new IncorrectDetailsException("User token or password missing.");
            string userName = tm.DecodeUserName(token);
            if (userName == null)
                throw new DetailsNotValidException("invalid user token");

            User user = _userService.Get(userName);
            if (user.Password != oldpassword)
                throw new DetailsNotValidException("incorrect password");

            user.UserName = userName;
            user.Password = newPassword;
            _userService.Update(user);
            return tm.GenerateJWT(user);

        }

        public User Get(string userName)
        {
            if (userName == null)
                throw new IncorrectDetailsException("The user name is empty");

            User tmp = _userService.Get(userName);
            if (tmp == null)
                throw new EntityNotExistsException("User not found");
            return tmp;
        }

        public string Login(string userName, string pass)
        {
            if (userName == null || pass == null)
                throw new IncorrectDetailsException("User name or pass empty");

            User user = _checkLogin.Authentication(userName, pass);

            string token = _tokenMng.CreateAndSave(user);
            return token;
        }
    }
}
