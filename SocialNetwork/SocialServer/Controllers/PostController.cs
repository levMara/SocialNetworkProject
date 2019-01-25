using SocialBL.Exceptiones;
using SocialBL.Manageres;
using SocialBL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;

namespace SocialServer.Controllers
{
    [RoutePrefix("api/Post")]
    public class PostController : ApiController
    {
        PostManager _postMng;

        public PostController()
        {
            _postMng = new PostManager();
        }

        [HttpPost]
        [Route("Add")]
        public async Task<IHttpActionResult> Add(string token, Post post)
        {
            if (string.IsNullOrEmpty(token) || post == null)
                return BadRequest("Token or post missing.");

            string userId = await Utils.VerifyAndDecrypt(token);
            if (string.IsNullOrEmpty(userId))
                return BadRequest("Token not valid.");

            try
            {
                _postMng.AddPost(userId, post);
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
        [Route("Get")]
        public async Task<IHttpActionResult> Get(string token, string postId)
        {
            if (string.IsNullOrEmpty(token) || postId == null)
                return BadRequest("Token or post id missing.");

            string userId = await Utils.VerifyAndDecrypt(token);
            if (string.IsNullOrEmpty(userId))
                return BadRequest("Token not valid.");

            try
            {
                var post= _postMng.GetPost(postId);
                return Ok(post);
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
        [Route("LikePost")]
        public async Task<IHttpActionResult> LikePost(string token, string postId)
        {
            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(postId))
                return BadRequest("Token or post id missing.");

            string userId = await Utils.VerifyAndDecrypt(token);
            if (string.IsNullOrEmpty(userId))
                return BadRequest("Token not valid.");
         
            try
            {
                int likes =_postMng.LikePost(userId, postId);
                return Ok(likes);
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
