using SocialBL.Models;
using SocialBL.Utils;
using SocialServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace SocialServer
{
    public static class Utils
    {
        public static async Task<FullUser> UserFullDetails(string token, string otherUserId)
        {
            ApiAccess identityAccsess = new ApiAccess("http://localhost:63485/api/");

            var res = await identityAccsess.GetData<FullUser>($"Identity/GetOther?token={token}&otherUserId={otherUserId}");
            if (res.Item1.IsSuccessStatusCode)
                return res.Item2;
            return null;
        }

        public static async Task<string> VerifyAndDecrypt(string token)
        {
            ApiAccess authenticationAccess = new ApiAccess("http://localhost:49922/api/");

            var res = await authenticationAccess.GetData<string>($"token/decodeUserId?token={ token}");
            if (res.Item1.IsSuccessStatusCode)
                return res.Item2;
            return null;
        }
        
    }
}
