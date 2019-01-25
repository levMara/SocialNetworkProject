using SocialBL.Exceptiones;
using SocialBL.Manager;
using SocialBL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace SocialServer.Controllers
{
    [RoutePrefix("api/UserAction")]
    public class UserActionController : ApiController
    {
        //IUserMng _userMng;
        UserActionManager _userMng;

        public UserActionController(/*IUserMng userMng*/)
        {
            //_userMng = userMng;
            _userMng = new UserActionManager();
        }

        [HttpGet]
        [Route("Add")]
        public async Task<IHttpActionResult> Add(string token, string userName)
        {
            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(userName))
                return BadRequest("Token or user name missing.");

            string userId = await Utils.VerifyAndDecrypt(token);
            if (string.IsNullOrEmpty(userId))
                return BadRequest("Token not valid.");
          
            try
            {
                User newUser = new User { Id = userId, Name = userName };
                _userMng.Add(newUser);
                return Ok();
            }
            catch (IncorrectDetailsException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                return Content(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpGet]
        [Route("Follow")]
        public async Task<IHttpActionResult> Follow(string token, string otherUserId)
        {
            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(otherUserId))
                return BadRequest("Token or other user missing");

            string userId = await Utils.VerifyAndDecrypt(token);
            if (string.IsNullOrEmpty(userId))
                return BadRequest("Token not valid.");

            try
            {
                return Ok(_userMng.Follow(userId, otherUserId));
            }
            catch (IncorrectDetailsException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                return Content(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpGet]
        [Route("Block")]
        public async Task<IHttpActionResult> Block(string token, string otherUserId)
        {
            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(otherUserId))
                return BadRequest("Token or other user missing");

            string userId = await Utils.VerifyAndDecrypt(token);
            if (string.IsNullOrEmpty(userId))
                return BadRequest("Token not valid.");

            try
            {
                _userMng.Block(userId, otherUserId);
                return Ok();
            }
            catch (IncorrectDetailsException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                return Content(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpGet]
        [Route("UnFollow")]
        public async Task<IHttpActionResult> UnFollow(string token, string otherUserId)
        {
            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(otherUserId))
                return BadRequest("Token or other user missing");

            string userId = await Utils.VerifyAndDecrypt(token);
            if (string.IsNullOrEmpty(userId))
                return BadRequest("Token not valid.");

            try
            {
                return Ok(_userMng.UnFollow(userId, otherUserId));
            }
            catch (IncorrectDetailsException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                return Content(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpGet]
        [Route("UnBlock")]
        public async Task<IHttpActionResult> UnBlock(string token, string otherUserId)
        {
            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(otherUserId))
                return BadRequest("Token or other user missing");

            string userId = await Utils.VerifyAndDecrypt(token);
            if (string.IsNullOrEmpty(userId))
                return BadRequest("Token not valid.");

            try
            {
                _userMng.UnBlock(userId, otherUserId);
                return Ok();
            }
            catch (IncorrectDetailsException e)
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