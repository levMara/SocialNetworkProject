
using IdentityBL.Exceptiones;
using IdentityBL.Models;
using IdentityBL.Utils;
using IetntityBL.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;

namespace Identity.Controllers
{
    [RoutePrefix("api/Identity")]
    public class IdentityController : ApiController
    {
        IIdentityMng _identityMng;

        public IdentityController(IIdentityMng identityMng)
        {
            _identityMng = identityMng;
        }


        [Route("Get")]
        public async Task<IHttpActionResult> Get(string token)
        {
            if (string.IsNullOrEmpty(token))
                return BadRequest("Empty token");

            string userId = await VerifyAndDecrypt(token);
            if (string.IsNullOrEmpty(userId))
                return BadRequest("Token not valid");

            try
            {
                FullUser user = _identityMng.Get(userId);
                if (user == null)
                    return BadRequest("Faild get user from db");
                return Ok(user);
            }
            catch (IdentityException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                return Content(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [Route("GetOther")]
        public async Task<IHttpActionResult> GetOther(string token, string otherUserId)
        {
            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(otherUserId))
                return BadRequest("Token or other user id empty.");

            string userId = await VerifyAndDecrypt(token);
            if (string.IsNullOrEmpty(userId))
                return BadRequest("Token not valid.");

            try
            {
                FullUser user = _identityMng.Get(otherUserId);
                if (user == null)
                    return BadRequest("Faild get user from db.");
                return Ok(user);
            }
            catch (IdentityException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                return Content(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [Route("GetOtherUserName")]
        public async Task<IHttpActionResult> GetOtherUserName(string token, string otherUserId)
        {
            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(otherUserId))
                return BadRequest("Token or other user id empty.");

            string userId = await VerifyAndDecrypt(token);
            if (string.IsNullOrEmpty(userId))
                return BadRequest("Token not valid.");

            try
            {
                FullUser user = _identityMng.Get(otherUserId);
                if (user == null)
                    return BadRequest("Faild get user from db.");
                return Ok(user.FullName);
            }
            catch (IdentityException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                return Content(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [Route("registerorupdate")]
        public async Task<IHttpActionResult> RegisterOrUpdate(FullUser user, string token)
        {
            if (string.IsNullOrEmpty(token))
                return BadRequest("Empty token");

            if (user == null)
                return BadRequest("Error, No details were provided.");

            string userId = await VerifyAndDecrypt(token);
            if (string.IsNullOrEmpty(userId))
                return BadRequest("Token not valid.");

            try
            {
                _identityMng.RegisterOrUpdate(userId, user);
                return Ok();
            }
            catch (IdentityException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                return Content(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [Route("SetUserDetailsIfNotExists")]
        public async Task<IHttpActionResult> SetUserDetailsIfNotExists(FullUser user, string token)
        {
            if (string.IsNullOrEmpty(token))
                return BadRequest("Empty token");

            if (user == null)
                return BadRequest("Error, No details were provided.");

            string userId = await VerifyAndDecrypt(token);
            if (string.IsNullOrEmpty(userId))
                return BadRequest("Token not valid.");

            try
            {
                _identityMng.Get(userId);
            }
            catch (IdentityException e)
            {
                _identityMng.RegisterOrUpdate(userId, user);
            }
            catch (Exception e)
            {
                return Content(HttpStatusCode.InternalServerError, e.Message);
            }
            return Ok();

        }

        //cache return value for 15 minutes
        [System.Web.Mvc.OutputCache(Duration = 15 * 60)]
        public IHttpActionResult GetUserIdsAndNames()
        {
            try
            {
                return Ok(_identityMng.GetUserIdsAndNames());
            }
            catch (IdentityException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        private async Task<string> VerifyAndDecrypt(string token)
        {
            ApiAccess _identityServerAccess = new ApiAccess("http://localhost:49922/api/");

            var res = await _identityServerAccess.GetData<string>($"token/decodeUserId?token={token}");
            if (res.Item1.IsSuccessStatusCode)
            {
                return res.Item2;
            }
            return null;
        }
    }

    //[Route("Get")]
    //public IHttpActionResult Get(string token)
    //{
    //    if (string.IsNullOrEmpty(token))
    //        return BadRequest("Empty token");

    //    string userId = VerifyAndDecrypt(token);
    //    if (string.IsNullOrEmpty(userId))
    //        return BadRequest("Token not valid");

    //    try
    //    {
    //        FullUser user = _identityMng.Get(userId);
    //        if (user == null)
    //            return BadRequest("Faild get user from db");
    //        return Ok(user);
    //    }
    //    catch (IdentityException e)
    //    {
    //        return BadRequest(e.Message);
    //    }
    //    catch (Exception e)
    //    {
    //        return Content(HttpStatusCode.InternalServerError, e.Message);
    //    }
    //}

    //[Route("GetOther")]
    //public IHttpActionResult GetOther(string token, string otherUserId)
    //{
    //    if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(otherUserId))
    //        return BadRequest("Token or other user id empty.");

    //    string userId = VerifyAndDecrypt(token);
    //    if (string.IsNullOrEmpty(userId))
    //        return BadRequest("Token not valid.");

    //    try
    //    {
    //        FullUser user = _identityMng.Get(otherUserId
    //            );
    //        if (user == null)
    //            return BadRequest("Faild get user from db.");
    //        return Ok(user);
    //    }
    //    catch (IdentityException e)
    //    {
    //        return BadRequest(e.Message);
    //    }
    //    catch (Exception e)
    //    {
    //        return Content(HttpStatusCode.InternalServerError, e.Message);
    //    }
    //}

    //[Route("Register")]
    //public IHttpActionResult Register(string token, [FromBody]FullUser user)
    //{
    //    if (string.IsNullOrEmpty(token))
    //        return BadRequest("Empty token");

    //    if (user == null)
    //        return BadRequest("Error, No details were provided.");

    //    string userId = VerifyAndDecrypt(token);
    //    if (string.IsNullOrEmpty(userId))
    //        return BadRequest("Token not valid.");

    //    try
    //    {
    //        FullUser newUser = _identityMng.Register(userId ,user);
    //        if (newUser == null)
    //            return BadRequest("Registration to database failed.");
    //        return Ok(newUser);
    //    }
    //    catch (IdentityException e)
    //    {
    //        return BadRequest(e.Message);
    //    }                     
    //    catch(Exception e)
    //    {
    //        return Content(HttpStatusCode.InternalServerError, e.Message);
    //    }        
    //}

    //[Route("Update")]
    //public IHttpActionResult Update(string token, [FromBody]FullUser updateUser)
    //{
    //    if (string.IsNullOrEmpty(token))
    //        return BadRequest("Empty token");

    //    if (updateUser == null)
    //        return BadRequest("Error, No details were provided");

    //    string userId = VerifyAndDecrypt(token);
    //    if (string.IsNullOrEmpty(userId))
    //        return BadRequest("Token not valid");

    //    try
    //    {
    //        FullUser updatedUser =_identityMng.Update(userId, updateUser);
    //        if (updatedUser == null)
    //            return BadRequest("Update failed.");
    //        return Ok(updatedUser);
    //    }
    //    catch (IdentityException e)
    //    {
    //        return BadRequest(e.Message);
    //    }
    //    catch(Exception e)
    //    {
    //        return Content(HttpStatusCode.InternalServerError, e.Message);
    //    }
    //}


    //private string VerifyAndDecrypt(string token)
    //{
    //    using (HttpClient client = new HttpClient())
    //    {
    //        client.BaseAddress = new Uri("http://localhost:49922/");
    //        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

    //        var task = client.GetAsync($"api/token/decodeUserId?token={token}");
    //        task.Wait();
    //        var res = task.Result;
    //        if (res.IsSuccessStatusCode)
    //        {
    //            return res.Content.ReadAsAsync<string>().Result;
    //        }
    //        return null;
    //    }
    //}

    //private async Task<string> VerifyAndDecrypt1(string token)
    //{
    //    using (HttpClient client = new HttpClient())
    //    {
    //        client.BaseAddress = new Uri("http://localhost:49922/");
    //        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

    //        var res = await client.GetAsync($"api/token/decodeUserId?token={token}");
    //        if (res.IsSuccessStatusCode)
    //        {
    //            return await res.Content.ReadAsAsync<string>();
    //        }
    //        return null;
    //    }
    //}
}

