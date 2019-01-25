using SocialBL.Exceptiones;
using SocialBL.Manageres;
using SocialBL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace SocialServer.Controllers
{
    [RoutePrefix("api/Comment")]
    public class CommentController : ApiController
    {
        CommentManager _commentMng;

        public CommentController()
        {
            _commentMng = new CommentManager();
        }

        [HttpPost]
        [Route("Comment")]
        public async Task<IHttpActionResult> Comment(string token, string postId, [FromBody]Comment comment)
        {
            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(postId) || comment == null)
                return BadRequest("Token or post id or comment missing.");

            string userId = await Utils.VerifyAndDecrypt(token);
            if (string.IsNullOrEmpty(userId))
                return BadRequest("Token not valid.");

            try
            {
                _commentMng.AddComment(userId, postId, comment);
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
        [Route("LikeComment")]
        public async Task<IHttpActionResult> LikeComment(string token, string commentId)
        {
            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(commentId))
                return BadRequest("Token or post id missing.");

            string userId = await Utils.VerifyAndDecrypt(token);
            if (string.IsNullOrEmpty(userId))
                return BadRequest("Token not valid.");

            try
            {
                int likes = _commentMng.LikeComment(userId, commentId);
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

        [HttpGet]
        [Route("GetComments")]
        public async Task<IHttpActionResult> GetComments(string token, string postId)
        {
            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(postId))
                return BadRequest("Token or post id or comment missing.");

            string userId = await Utils.VerifyAndDecrypt(token);
            if (string.IsNullOrEmpty(userId))
                return BadRequest("Token not valid.");

            try
            {
                var comments = _commentMng.GetComments(postId);
                return Ok(comments);
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
