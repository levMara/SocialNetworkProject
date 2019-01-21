using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;

namespace WebApplication4.BL
{
    public class ManagerBase
    {

        public class ResultBase
        {
            public bool Success { get; set; }
            public string UserErrorMessage { get; set; }

            public ResultBase() { }
            public ResultBase(bool success, string userErrorMessage)
            {
                this.Success = success;
                this.UserErrorMessage = userErrorMessage;
            }
        }

        private class BadRequestResultObject
        {
            public string Message { get; set; }
        }

        protected static async Task<string> getBadRequestMessage(HttpResponseMessage httpResponseMessage, string defaultUserErrorMessage)
        {
            string result = defaultUserErrorMessage;
            if (httpResponseMessage.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                var badRequestResultContent = await httpResponseMessage.Content.ReadAsStringAsync();
                BadRequestResultObject badRequestResultObject =
                Json.Decode<BadRequestResultObject>(badRequestResultContent);
                result = badRequestResultObject.Message;
            }
            return result;
        }
        //recieves HttpResponseMessage,  returns a ResultBase with message as in httpResponseMessage or defaultUserErrorMessage
        //depending on if status code is BadRequest or not, respectively.
        //and then casts the result to a ResultModel
        protected static async Task<ResultModel> ReturnErrorResult<ResultModel>(HttpResponseMessage httpResponseMessage, string defaultUserErrorMessage) where ResultModel : ResultBase, new()
        {
            ResultModel result = new ResultModel { Success = false, UserErrorMessage = defaultUserErrorMessage };

            if (httpResponseMessage.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                var badRequestResultContent = await httpResponseMessage.Content.ReadAsStringAsync();
                BadRequestResultObject badRequestResultObject =
                Json.Decode<BadRequestResultObject>(badRequestResultContent);
                result.UserErrorMessage = badRequestResultObject.Message;
            }
            return result;
        }
    }
}