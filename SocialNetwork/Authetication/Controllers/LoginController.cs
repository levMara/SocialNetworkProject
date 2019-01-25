using Authetication.Models;
using BL.Exceptiones;
using BL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Authetication.Controllers
{
    [RoutePrefix("api/Login")]
    public class LoginController : ApiController
    {
        ILogin _loginMng;
        IUserMng _userMng;

        public LoginController(ILogin loginMng, IUserMng userMng)
        {
            _loginMng = loginMng;
            _userMng = userMng;
        }

        [HttpGet]
        [Route("Login")]
        public IHttpActionResult Login(string userName, string pass)
        {
            if (userName == null || pass == null)
            {
                return BadRequest("User name or password missing");
            }

            string token;
            try
            {
                token = _loginMng.Login(userName, pass);
                if (string.IsNullOrEmpty(token))
                    return BadRequest("Registration to database failed.");

                return Content(HttpStatusCode.OK, token);
            }

            catch (UserException e)
            {
                return BadRequest(e.Message);
            }

            catch (Exception e)
            {
                return Content(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpGet]
        [Route("Register")]
        public async Task<IHttpActionResult> Register(string userName, string pass)
        {
            if (userName == null || pass == null)
            {
                return BadRequest("usr name or pass missing");
            }

            string token;
            try
            {
                token = await _userMng.Add(userName, pass);
                if (string.IsNullOrEmpty(token))
                    return BadRequest("Registration to database failed.");
                
                return Content(HttpStatusCode.OK, token);
            }

            catch (UserException e)
            {
                return BadRequest(e.Message);
            }

            catch (Exception e)
            {
                return Content(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        //saed
        [HttpGet]
        [Route("ChangePassword")]
        public IHttpActionResult ChangePassword(string token, string oldpassword, string newPassword)
        {
            if (token == null || newPassword == null)
            {
                return BadRequest("user token or pass missing");
            }

            string newToken;
            try
            {
                newToken = _userMng.ChangePassword(token, oldpassword, newPassword);
                if (string.IsNullOrEmpty(newToken))
                    return BadRequest("failed to change password");

                return Content(HttpStatusCode.OK, newToken);
            }

            catch (UserException e)
            {
                return BadRequest(e.Message);
            }

            catch (Exception e)
            {
                return Content(HttpStatusCode.InternalServerError, e.Message);
            }
        }
    }
}
