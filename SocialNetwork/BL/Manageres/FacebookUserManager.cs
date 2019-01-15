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
    public class FacebookUserManager /*: ILogin , IUserMng*/ //TODO use interface
    {
        //interface
        private readonly FacebookUserService _facebookUserService;
        private readonly RegisterService _registerService;
        private readonly CheckLogin _checkLogin;
        FacebookTokenManager _tokenMng;//TODO use interface

        public FacebookUserManager(ITokenMng tokenMng)
        {
            _facebookUserService = new FacebookUserService();
            _registerService = new RegisterService();
            _checkLogin = new CheckLogin();
            _tokenMng = new FacebookTokenManager();//TODO use interface
        }

        public string Add(string facebookId)
        {

            if (facebookId == null)
                throw new IncorrectDetailsException("facebook Id is empty");

            FacebookUser newUser = new FacebookUser
            {
                FacebookId = facebookId,
                UserId = Guid.NewGuid().ToString()
            };

            _facebookUserService.Add(newUser);
            string token = _tokenMng.CreateAndSave(newUser);
            return token;
        }
        

        public FacebookUser Get(string facebookId)
        {
            if (facebookId == null)
                throw new IncorrectDetailsException("The facebook Id is empty");

            FacebookUser tmp = _facebookUserService.Get(facebookId);
            if (tmp == null)
                throw new EntityNotExistsException("User not found");
            return tmp;
        }

        public string Login(string facebookId)
        {
            if (facebookId == null)
                throw new IncorrectDetailsException("facebook Id is empty");

            FacebookUser user = _facebookUserService.Get(facebookId);

            string token = _tokenMng.CreateAndSave(user);
            return token;
        }
    }
}
