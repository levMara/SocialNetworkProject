
namespace Authetication.Controllers
{
    using BL.Exceptiones;
    using BL.Interfaces;
    using BL.Manageres;
    using System;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Http;

    namespace Authetication.Controllers
    {
        public class FacebookLoginController : ApiController
        {
            //   // ILogin _loginMng;
            private FacebookUserManager _facebookUserMng;//TODO use interfaces

            //TODO use interfaces
            public FacebookLoginController(ILogin loginMng, IUserMng userMng)
            {
                //_loginMng = loginMng;
                //TODO use interfaces
                _facebookUserMng = new FacebookUserManager(null);
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
                    
                    string userToken = null;
                    try
                    {
                        userToken = await _facebookUserMng.Login(facebookUserId);
                    }
                    catch (UserException ex)
                    {
                        return BadRequest(ex.Message);
                    }
                    catch (Exception ex) {
                        return Content(System.Net.HttpStatusCode.InternalServerError, ex.Message);
                    }

                    if (userToken != null)
                        return Ok(userToken);

                }
                return BadRequest("failed to login");
            }


        }
    }
    //public class FacebookLoginController : ApiController
    //{
    //   // ILogin _loginMng;
    //  //  IUserMng _userMng;

    //   //public FacebookLoginController(ILogin loginMng, IUserMng userMng)
    //   // {
    //   //     _loginMng = loginMng;
    //   //     _userMng = userMng;
    //   // }

    //    [HttpGet]
    //    [Route("api/FacebookLogin/Login/{facebookToken}")]
    //    public IHttpActionResult Login(string facebookToken)
    //    {
    //        if (string.IsNullOrEmpty(facebookToken))
    //        {
    //            return BadRequest("Token missing");
    //        }

    //        string token;
    //        try
    //        {
    //           // Verify(token);
    //           // token = _loginMng.Login(userName, pass);
    //        }
    //        catch (Exception e)
    //        {
    //            return Content(HttpStatusCode.InternalServerError, e.Message);
    //        }

    //       // return Content(HttpStatusCode.OK, token);
    //    }

    //    [HttpPost]
    //    [Route("api/FacebookLogin/Register/{facebookToken}")]
    //    public IHttpActionResult Register(string facebookToken)
    //    {
    //        if (string.IsNullOrEmpty(facebookToken))
    //        {
    //            return BadRequest("Token missing");
    //        }

    //        string token;
    //        try
    //        {
    //           // token = _userMng.Add(userName, pass);
    //        }

    //        catch (IncorrectDetailsException e)
    //        {
    //            return BadRequest(e.Message);
    //        }

    //        catch (DetailsNotValidException e)
    //        {
    //            return BadRequest(e.Message);
    //        }

    //        catch (Exception e)
    //        {
    //            return Content(HttpStatusCode.InternalServerError, e.Message);
    //        }

    //        if (string.IsNullOrEmpty(token))
    //        {
    //            return BadRequest("Registration to database failed.");
    //        }

    //        return Content(HttpStatusCode.OK, token);
    //    }

    //    //private bool Verify(string facebookToken)
    //    //{
    //    //    using (var client = new HttpClient())
    //    //    {
    //    //       // client.BaseAddress;
    //    //    }
    //    //}
    //}
}