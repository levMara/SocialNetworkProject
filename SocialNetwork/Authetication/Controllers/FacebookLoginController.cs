using Authetication.Models;
using BL.Exceptiones;
using BL.Manageres;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Authetication.Controllers
{

    public class FacebookLoginController : ApiController
    {
        //   // ILogin _loginMng;
         FacebookUserManager _userMng;//TODO use interfaces

        //TODO use interfaces
        public FacebookLoginController(/*ILogin loginMng, IUserMng userMng*/)
        {
            //_loginMng = loginMng;
            //TODO use interfaces
            _userMng = new FacebookUserManager(null);
        }


        private class FacebookLoginResponse
        {
            public bool UserExisted { get; set; }
            public string Token { get; set; }
        }

        [HttpGet]
        public async Task<IHttpActionResult> Login(string facebookToken)
        {
            
            //TODO verify access token in the server
            string appToken = "523392351508330|uhPdjs-O_mQJI8aKCvguC3vhMbM";
            Utils.ApiAccess api = new Utils.ApiAccess("https://graph.facebook.com/");
            var response = await api.GetData<dynamic>($"debug_token?input_token={facebookToken}&access_token={appToken}");
            if (response.Item1.IsSuccessStatusCode)
            {
                bool isTokenValid = response.Item2["data"]["is_valid"];
                string facebookUserId = response.Item2["data"]["user_id"];

                FacebookUser user = null;
                string userToken = null;
                try
                {
                    user = _userMng.Get(facebookUserId);
                    userToken = _userMng.Login(facebookUserId);
                }
                catch (EntityNotExistsException)
                {
                    userToken = _userMng.Add(facebookUserId);
                }

                if (userToken != null)
                    return Ok(userToken);

            }
            return BadRequest("failed to login");
        }
    }
}