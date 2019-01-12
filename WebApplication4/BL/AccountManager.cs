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

        public ResultBase(bool success, string userErrorMessage)
        {
            this.Success = success;
            this.UserErrorMessage = userErrorMessage;
        }
    }

    public class SignInResult: ResultBase
    {
        public string Token { get; set; }

        public SignInResult(bool success, string token,string userErrorMessage=null):base(success, userErrorMessage)
        {
            this.Token = token;
        }
    }

    public class FacebookLoginResult: ResultBase
    {
        public bool UserExisted { get; set; }
        public string Token { get; set; }
        public FacebookLoginResult(bool success, bool userExisted,string token, string userErrorMessage = null) : base(success, userErrorMessage)
        {
            this.UserExisted = userExisted;
            this.Token = token;
        }
    }

    public class RegisterResult:ResultBase
    {
        public string Token { get; set; }

        public RegisterResult(bool success, string token, string userErrorMessage = null) : base(success, userErrorMessage)
        {
            Token = token;
        }
    }

    public class ChangePasswordResult: ResultBase
    {
        public ChangePasswordResult(bool success, string userErrorMessage = null) : base(success, userErrorMessage)
        {
        }

    }

    public class ChangeUserDetailsResult:ResultBase
    {
        public ChangeUserDetailsResult(bool success, string userErrorMessage = null) : base(success, userErrorMessage)
        {
        }
    }

    #endregion

    public class AccountManager
    {
        private static WebAPIAccess authServiceAccess = new WebAPIAccess("http://localhost:50642/Account/Login");//TODO
        private static WebAPIAccess identityServiceAccess = new WebAPIAccess("http://localhost:50642/Account/Login");//TODO


        public AccountManager()
        {
            authServiceAccess = new WebAPIAccess("http://localhost:50642/Account/Login");//TODO
            identityServiceAccess = new WebAPIAccess("http://localhost:50642/Account/Login");//TODO
        }

        #region Helpers
        private static async Task<ResultBase> ReturnErrorResult(HttpResponseMessage httpResponseMessage, string defaultUserErrorMessage)
        {
            ResultBase result = new ResultBase(false, defaultUserErrorMessage);
            if (httpResponseMessage.StatusCode == System.Net.HttpStatusCode.BadRequest)
                result.UserErrorMessage = await httpResponseMessage.Content.ReadAsStringAsync();
            return result;
        }
        #endregion

        #region temporary

        private static Dictionary<string, string> userpass = new Dictionary<string, string>();
        private static Dictionary<string, UserModel> users = new Dictionary<string, UserModel>();

        private class Token
        {
            public string Username { get; set; }
            public DateTime ttl { get; set; }
            public Token(string username, DateTime ttl)
            {
                this.Username = username;
                this.ttl = ttl;
            }
            public Token() { }

            public bool Valid
            {
                get => this.ttl > DateTime.UtcNow;
            }
        }

        private static string maketoken(string username)
        {
            return Json.Encode(new Token(username, DateTime.UtcNow.AddMinutes(1)));
        }
        private static string refreshtoken(Token oldtotken)
        {
            Token newtoken = oldtotken;
            newtoken.ttl.AddMinutes(1);
            return Json.Encode(newtoken);
        }
        private static Token getToken(string token) {
            if (token == null)
                return null;
            JavaScriptSerializer x = new JavaScriptSerializer() ;
            return x.Deserialize<Token>(token);
        }
        #endregion

        internal static async Task<SignInResult> PasswordSignInAsync(string username, string password)
        {
            if (userpass.ContainsKey(username))
            {
                if (userpass[username] == password)
                    return new SignInResult(true, maketoken(username));
            }

            return new SignInResult(false, null, "username or password doesn't exist");


                var result = await authServiceAccess.GetData<string>($"login/login?username={username}&pass={password}");//TODO check
            if (result.Item1.IsSuccessStatusCode)
            {
                return new SignInResult(true, result.Item2);
            }
            else
            {
                return (SignInResult)(await (ReturnErrorResult(result.Item1,"failed to sign in")));
            }
            //throw new NotImplementedException();
        }



        //TODO check if token is returned on register
        internal static async Task<RegisterResult> RegisterAsync(string username, string password, UserModel userDetails)
        {
            if (userpass.ContainsKey(username))
            {
                return new RegisterResult(false, null, "user already exists");
            }
            else
            {
                userpass[username] = password;
                users[username] = userDetails;
                return new RegisterResult(true, maketoken(username));
                
            }

            var registerResult = await authServiceAccess.GetData<string>($"login/register?username={username}&pass={password}");//TODO check
            if (registerResult.Item1.IsSuccessStatusCode)
            {
                var userToken = registerResult.Item2;
                var changeDetailsResult = await ChangeUserDetails(userToken, userDetails);
                return new RegisterResult(changeDetailsResult.Success, userToken, changeDetailsResult.UserErrorMessage);
            }
            else
                return (RegisterResult)(await (ReturnErrorResult(registerResult.Item1, "failed to register")));

        }

        //internal void SignOut(/*string token*/)
        //{

        //    //TODO: invalidate token
        //    //throw new NotImplementedException();
        //}

        internal static async Task<ChangeUserDetailsResult> ChangeUserDetails(string userToken,UserModel newUserDetails)
        {
            var token = getToken(userToken);
            var username = token.Username;
            if (token.Valid)
            {
                users[token.Username] = newUserDetails;
                return new ChangeUserDetailsResult(true);
            }
            else
                return new ChangeUserDetailsResult(false, "token expired");

            var changeDetailsResult = await identityServiceAccess.PostData<string,UserModel>($"user/changedetails?usertoken={userToken}", newUserDetails);//TODO check
            if (changeDetailsResult.Item1.IsSuccessStatusCode)
                return new ChangeUserDetailsResult(true);
            else
                return (ChangeUserDetailsResult)(await (ReturnErrorResult(changeDetailsResult.Item1, "failed to set user info")));
        }

        internal static async Task<SignInResult> FacebookLogin(string facebookAccessToken, UserModel userDetails)
        {
            //TODO verify access token in the server

            return new SignInResult(true, "token abcdefg");

            var facebookLoginResult = await authServiceAccess.GetData<FacebookLoginResult>($"facebooklogin/login?facebookAccessToken={facebookAccessToken}");//TODO check
            if (facebookLoginResult.Item1.IsSuccessStatusCode)
            {
                bool userExisted = facebookLoginResult.Item2.UserExisted;
                string userToken = facebookLoginResult.Item2.Token;

                if (!userExisted)
                {
                    var changeDetailsResult = await ChangeUserDetails(userToken, userDetails);
                    return new SignInResult(changeDetailsResult.Success, userToken, changeDetailsResult.UserErrorMessage);
                    //throw new NotImplementedException();
                }
                return new SignInResult(true,userToken);

            }
            else
                return (SignInResult)(await (ReturnErrorResult(facebookLoginResult.Item1, "failed to login with facebook")));
        }

        internal static async Task<ChangePasswordResult> ChangePasswordAsync(string userToken, string oldPassword, string newPassword)
        {
            var token = getToken(userToken);
            if (token.Valid)
            {
                if (userpass[token.Username] != oldPassword)
                    return new ChangePasswordResult(false, "incorrect password");
                userpass[token.Username] = newPassword;
                return new ChangePasswordResult(true);
            }
            else
                return new ChangePasswordResult(false, "token expired");


            var result = await identityServiceAccess.GetData<string>($"changepassword?usertoken={userToken}&oldpassword={oldPassword}&newpassword={newPassword}");//TODO change
            if (result.Item1.IsSuccessStatusCode)
            {
                return new ChangePasswordResult(true);
            }
            else
                return (ChangePasswordResult)(await (ReturnErrorResult(result.Item1, "failed to change password")));
        }

        public static async Task<UserModel> GetUserInfoAsync(string userToken)
        {
            var token = getToken(userToken);
            if (token.Valid)
                return users[token.Username];
            return null;

            var decryptUserResult = await identityServiceAccess.GetData<UserModel>($"decrypttoken?token={userToken}");
            if (decryptUserResult.Item1.IsSuccessStatusCode)
                return decryptUserResult.Item2;
            return null;

        }
        public static UserModel GetUserInfo(string userToken)
        {
            if (userToken != null)
            {
                Task<UserModel> task = GetUserInfoAsync(userToken);
                task.Wait();
                return task.Result;
            }
            else
                return null;

        }

        public static bool IsUserAuthorized(string userToken)
        {
            if (userToken != null)
            {
                Token token = getToken(userToken);
                if (token != null)
                {
                    return token.Valid;
                }
            }
            return false;
        }

        internal static string RefreshToken(string userToken)
        {
            var token = getToken(userToken);
            if (token!=null && token.Valid)
            {
                return refreshtoken(token);
            }
            return null;
                    //TODO
        }
    }


}