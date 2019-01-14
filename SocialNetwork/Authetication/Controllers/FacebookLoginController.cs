using BL.Exceptiones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace Authetication.Controllers
{
 
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