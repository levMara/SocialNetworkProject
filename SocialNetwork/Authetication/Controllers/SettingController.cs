using BL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Authetication.Controllers
{
    [RoutePrefix("api/Setting")]
    public class SettingController : ApiController
    {
        IToken _tokenCheck;
        ISetting _SettingMng;//add to unity

        public SettingController(IToken tokenCheck, ISetting sttingMng)
        {
            _tokenCheck = tokenCheck;
            _SettingMng = sttingMng;
        }

        //levi
        [HttpGet]
        [Route("ChangePassword")]
        public IHttpActionResult ChangePassword(string token, string newPass)
        {
            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(newPass))
                return BadRequest("Token or new pass empty.");

            string userId = _tokenCheck.DecodeUserId(token);
            if (token == null)
                return BadRequest("Token not valid.");

            _SettingMng.ChangePassword(userId, newPass);
 
            return Ok();
        }
    }
}
