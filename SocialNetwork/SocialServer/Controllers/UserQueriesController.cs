using SocialBL.Exceptiones;
using SocialBL.Manager;
using SocialBL.Manageres;
using SocialBL.Models;
using SocialBL.Utils;
using SocialServer.Models;
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
    [RoutePrefix("api/UserQueries")]
    public class UserQueriesController : ApiController
    {
        //IUserMng _userMng;
        UserQueriesMangaer _userMng;
        PostManager _postMng;

        public UserQueriesController()
        {
            _userMng = new UserQueriesMangaer();
            _postMng = new PostManager();
        }

        [HttpGet]
        [Route("GetMyFollowers")]
        public async Task<IHttpActionResult> GetMyFollowers(string token)
        {
            if (string.IsNullOrEmpty(token))
                return BadRequest("Token missing");

            string userId = await Utils.VerifyAndDecrypt(token);
            if (string.IsNullOrEmpty(userId))
                return BadRequest("Token not valid.");

            
            try
            {
                IEnumerable<UserFollowerModel> followers = _userMng.GetFollowers(userId);
                return Ok(followers);
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
        [Route("GetMyFollows")]
        public async Task<IHttpActionResult> GetMyFollows(string token)
        {
            if (string.IsNullOrEmpty(token))
                return BadRequest("Token missing");

            string userId = await Utils.VerifyAndDecrypt(token);
            if (string.IsNullOrEmpty(userId))
                return BadRequest("Token not valid.");

            // string userId = "770";
            try
            {
                IEnumerable<User> follows = _userMng.GetFollows(userId);
                return Ok(follows);
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
        [Route("GetProfileVisitableUsers")]
        public async Task<IHttpActionResult> GetProfileVisitableUsers(string token)
        {
            if (string.IsNullOrEmpty(token))
                return BadRequest("Token missing");

            string userId = await Utils.VerifyAndDecrypt(token);
            if (string.IsNullOrEmpty(userId))
                return BadRequest("Token not valid.");

            try
            {
                IEnumerable<User> users = _userMng.GetProfileVisitableUsers(userId);
                return Ok(users);
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
        [Route("GetBlocked")]
        public async Task<IHttpActionResult> GetBlocked(string token)
        {
            if (string.IsNullOrEmpty(token))
                return BadRequest("Token missing");

            string userId = await Utils.VerifyAndDecrypt(token);
            if (string.IsNullOrEmpty(userId))
                return BadRequest("Token not valid.");

            // string userId = "770";
            try
            {
                IEnumerable<User> Blocked = _userMng.GetBlocked(userId);
                return Ok(Blocked);
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
        [Route("GetProfile")]
        public async Task<IHttpActionResult> GetProfile(string token, string otherUserId)
        {
            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(otherUserId))
                return BadRequest("Token missing");

            string userId = await Utils.VerifyAndDecrypt(token);
            if (string.IsNullOrEmpty(userId))
                return BadRequest("Token not valid.");
            
            try
            {
                var fullUser = await Utils.UserFullDetails(token, otherUserId);
                

                ProfileModel profile = new ProfileModel
                {
                    FullUser = fullUser,
                    GetterFollowsUser=_userMng.IsFollowerOf(userId,otherUserId),
                    Posts = _postMng.GetMyPosts(otherUserId),
                    Followers = _userMng.GetFollowers(otherUserId).ToList(),
                    FollowersCount = _userMng.GetFollowersCount(otherUserId),
                    Follows = _userMng.GetFollows(otherUserId).ToList()
                };

                return Ok(profile);
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
        [Route("IsFollowerOf")]
        public async Task<IHttpActionResult> IsFollowerOf(/*string token, string otherUserId*/)
        {
            //if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(otherUserId))
            //    return BadRequest("Token or fo.. missing");

            //string userId = await Utils.VerifyEndDecrypt(token);
            //if (string.IsNullOrEmpty(userId))
            //    return BadRequest("Token not valid.");

            string userId = "660", otherUserId = "3893ea5a-0083-4e39-9839-59b417da597a";
            try
            {
                bool isFollows = _userMng.IsFollowerOf(userId, otherUserId);
                return Ok(isFollows);
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
