using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using WebApplication4.Models;
using System.Net.Http;
using System.Web.Helpers;
using System.Web.Script.Serialization;

namespace WebApplication4.BL
{

    #region Result Models
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

    public class SignInResult : ResultBase
    {
        public string Token { get; set; }
        public SignInResult() { }
        public SignInResult(bool success, string token, string userErrorMessage = null) : base(success, userErrorMessage)
        {
            this.Token = token;
        }

    }


    public class RegisterResult : ResultBase
    {
        public string Token { get; set; }
        public RegisterResult() { }
        public RegisterResult(bool success, string token, string userErrorMessage = null) : base(success, userErrorMessage)
        {
            Token = token;
        }
    }

    public class ChangePasswordResult : ResultBase
    {
        public string Token { get; set; }
        public ChangePasswordResult() { }
        public ChangePasswordResult(bool success,string token, string userErrorMessage = null) : base(success, userErrorMessage)
        {
            Token = token;
        }

    }

    public class ChangeUserDetailsResult : ResultBase
    {
        public ChangeUserDetailsResult() { }
        public ChangeUserDetailsResult(bool success, string userErrorMessage = null) : base(success, userErrorMessage)
        {
        }
    }

    #endregion

    public class AccountManager
    {
        private static WebAPIAccess authServiceAccess = new WebAPIAccess("http://localhost:49922/api/");//TODO
        private static WebAPIAccess identityServiceAccess = new WebAPIAccess("http://localhost:63485/api/");//TODO


        private class BadRequestResultObject
        {
            public string Message { get; set; }
        }

        #region Helpers

        //recieves HttpResponseMessage,  returns a ResultBase with message as in httpResponseMessage or defaultUserErrorMessage
        //depending on if status code is BadRequest or not, respectively.
        //and then casts the result to a ResultModel
        private static async Task<ResultModel> ReturnErrorResult<ResultModel>(HttpResponseMessage httpResponseMessage, string defaultUserErrorMessage) where ResultModel : ResultBase, new()
        {
            ResultModel result = new ResultModel { Success= false, UserErrorMessage = defaultUserErrorMessage};

            if (httpResponseMessage.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                var badRequestResultContent = await httpResponseMessage.Content.ReadAsStringAsync();
                BadRequestResultObject badRequestResultObject=
                Json.Decode<BadRequestResultObject>(badRequestResultContent);
                result.UserErrorMessage = badRequestResultObject.Message;
            }
            return result;
        }
        #endregion
        
        internal static async Task<SignInResult> PasswordSignInAsync(string username, string password)
        {

            var result = await authServiceAccess.GetData<string>($"login/login?username={username}&pass={password}");//TODO check
            if (result.Item1.IsSuccessStatusCode)
            {
                return new SignInResult(true, result.Item2);
            }
            else
            {
                return (await (ReturnErrorResult<SignInResult>(result.Item1, "failed to sign in")));
            }
        }


        internal static async Task<RegisterResult> RegisterAsync(string username, string password, FullUser userDetails)
        {
            var registerResult = await authServiceAccess.GetData<string>($"login/register?username={username}&pass={password}");//TODO check
            if (registerResult.Item1.IsSuccessStatusCode)
            {
                var userToken = registerResult.Item2;
                var changeDetailsResult = await ChangeUserDetailsAsync(userToken, userDetails);
                return new RegisterResult(changeDetailsResult.Success, userToken, changeDetailsResult.UserErrorMessage);
            }
            else
                return (await (ReturnErrorResult< RegisterResult>(registerResult.Item1, "failed to register")));

        }



        internal static async Task<ChangeUserDetailsResult> ChangeUserDetailsAsync(string userToken, FullUser userDetails)
        {

            Tuple<HttpResponseMessage, FullUser> changeDetailsResult;

            changeDetailsResult = await identityServiceAccess.PostData<FullUser, FullUser>($"identity/registerorupdate?token={userToken}", userDetails);//TODO check

            if (changeDetailsResult.Item1.IsSuccessStatusCode)
                return new ChangeUserDetailsResult(true);
            else
                return (await (ReturnErrorResult<ChangeUserDetailsResult>(changeDetailsResult.Item1, "failed to set user info")));


        }

        internal static async Task<ChangeUserDetailsResult> SetUserDetailsIfNotExistsAsync(string userToken, FullUser userDetails)
        {
            
            var changeDetailsResult = await identityServiceAccess.PostData<FullUser>($"identity/SetUserDetailsIfNotExists?token={userToken}", userDetails);//TODO check

            if (changeDetailsResult.IsSuccessStatusCode)
                return new ChangeUserDetailsResult(true);
            else
                return (await (ReturnErrorResult<ChangeUserDetailsResult>(changeDetailsResult, "failed to set user info")));

        }


        internal static async Task<SignInResult> FacebookLoginAsync(string facebookAccessToken, FullUser userDetails)
        {
            
            var facebookLoginResult = await authServiceAccess.GetData<string>($"facebooklogin/login?facebookToken={facebookAccessToken}");//TODO check
            if (facebookLoginResult.Item1.IsSuccessStatusCode)
            {
                //bool userExisted = facebookLoginResult.Item2.UserExisted;
                string userToken = facebookLoginResult.Item2;

                //if (!userExisted)
                //{
                    var changeDetailsResult = await SetUserDetailsIfNotExistsAsync(userToken, userDetails);
                    return new SignInResult(changeDetailsResult.Success, userToken, changeDetailsResult.UserErrorMessage);
                    //throw new NotImplementedException();
                //}
                //return new SignInResult(true, userToken);

            }
            else
                return (await (ReturnErrorResult<SignInResult>(facebookLoginResult.Item1, "failed to login with facebook")));
        }

        internal static async Task<ChangePasswordResult> ChangePasswordAsync(string userToken, string oldPassword, string newPassword)
        {
            var result = await authServiceAccess.GetData<string>($"login/changepassword?token={userToken}&oldpassword={oldPassword}&newpassword={newPassword}");//TODO change
            if (result.Item1.IsSuccessStatusCode)
            {
                return new ChangePasswordResult(true,result.Item2);
            }
            else
                return (await (ReturnErrorResult<ChangePasswordResult>(result.Item1, "failed to change password")));
            
        }

        public static async Task<FullUser> GetUserInfoAsync(string userToken)
        {
            var decryptUserResult = await identityServiceAccess.GetData<FullUser>($"identity/get?token={userToken}");
            if (decryptUserResult.Item1.IsSuccessStatusCode)
                return decryptUserResult.Item2;
            return null;

        }

        public async static Task<bool> IsUserAuthorizedAsync(string userToken)
        {
            var isTokenValidResult = await authServiceAccess.GetData<bool>($"token/Verify?token={userToken}");
            if (isTokenValidResult.Item1.IsSuccessStatusCode)
                return isTokenValidResult.Item2;
            return false;

        }

        internal static async Task<string> RefreshTokenAsync(string userToken)
        {

            var newTokenResult = await authServiceAccess.GetData<string>($"token/refresh?token={userToken}");
            if (newTokenResult.Item1.IsSuccessStatusCode)
                return newTokenResult.Item2;
            return null;

        }
    }


}