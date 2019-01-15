using IdentityBL.Exceptiones;
using IdentityBL.Models;
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

            string userId = await VerifyEndDecrypt(token);
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

        [Route("registerorupdate")]
        public async Task<IHttpActionResult> RegisterOrUpdate(FullUser user, string token)
        {
            if (string.IsNullOrEmpty(token))
                return BadRequest("Empty token");

            if (user == null)
                return BadRequest("Error, No details were provided.");

            string userId = await VerifyEndDecrypt(token);
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

            string userId = await VerifyEndDecrypt(token);
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


        private async Task<string> VerifyEndDecrypt(string token)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:49922/");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var res = client.GetAsync($"api/token/decodeUserId?token={token}").Result;
                if (res.IsSuccessStatusCode)
                {
                   return await res.Content.ReadAsAsync<string>();              
                }
                return null;
            }
        }
    }
}
