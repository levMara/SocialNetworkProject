using BL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;

namespace Authetication.Controllers
{

    [RoutePrefix("api/Token")]
    public class TokenController : ApiController
    {
        IToken _tokenCheck;

        public TokenController(IToken tokenMng)
        {
            _tokenCheck = tokenMng;
        }

        [HttpGet]
        [Route("Verify")]
        public IHttpActionResult Verify(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return BadRequest("Empty token");
            }
            
            try
            {
                bool isValid = _tokenCheck.Verify(token);
                return Ok(isValid);
            }
            catch (Exception e)
            {
                return Content(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpGet]
        [Route("Refresh")]
        public IHttpActionResult Refresh(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return BadRequest("Empty token");
            }

            try
            {
                string newToken = _tokenCheck.RefreshToken(token);
                if (string.IsNullOrEmpty(newToken))
                    return BadRequest("Error, faild get user id");
                return Ok(newToken);
            }

            catch (Exception e)
            {
                return Content(HttpStatusCode.BadRequest, e.Message);
            }

        }
        [HttpGet]
        [Route("DecodeUserId")]
        public IHttpActionResult DecodeUserId(string token)
        {
            if (string.IsNullOrEmpty(token))          
                return BadRequest("Empty token");            

            try
            {
                string userId = _tokenCheck.DecodeUserId(token);
                if (string.IsNullOrEmpty(userId))
                    return BadRequest("Error, faild get user id");
                return Ok(userId); 
            }

            catch (Exception e)
            {
                return Content(HttpStatusCode.BadRequest, e.Message);
            }
        }
    }
}