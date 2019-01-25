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
    [RoutePrefix("api/Feed")]
    public class FeedController : ApiController
    {
        FeedManager _feedMng;

        public FeedController()
        {
            _feedMng = new FeedManager();
        }

        [HttpGet]
        [Route("GetFeed")]
        public async Task<IHttpActionResult> GetFeed(string token)
        {
            if (string.IsNullOrEmpty(token))
                return BadRequest("Token missing.");

            string userId = await Utils.VerifyAndDecrypt(token);
            if (string.IsNullOrEmpty(userId))
                return BadRequest("Token not valid.");

            //string userId = "770";
            try
            {
                var feed = _feedMng.GetFeed(userId);
                return Ok(feed);
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
