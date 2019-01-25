using Authetication.Db;
using Authetication.Models;
using Authetication.Utils;
using BL.Dal;
using BL.Dal.BL.Dal;
using BL.Exceptiones;
using BL.Interfaces;
using BL.Models;
using BL.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Manageres
{
    public class FacebookUserManager  //TODO use interface
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

        public async Task<string> Add(string facebookId)
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
            await AddToNeo4j(token, "Facebook("+ facebookId + ")");
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

        public async Task<string> Login(string facebookId)
        {
            if (facebookId == null)
                throw new IncorrectDetailsException("facebook Id is empty");

            FacebookUser user = _facebookUserService.Get(facebookId);
            string token;
            if (user == null)
                token = await Add(facebookId);
            else
                token = _tokenMng.CreateAndSave(user);
            return token;
        }


        private async Task<bool> AddToNeo4j(string token, string userName)
        {
            ApiAccess socialServer = new ApiAccess("http://localhost:56139/api/");

            var res = await socialServer.GetData($"UserAction/Add?token={token}&userName={userName}");
            return res.IsSuccessStatusCode;
        }
    }
}