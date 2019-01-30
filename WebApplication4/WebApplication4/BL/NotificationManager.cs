using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace WebApplication4.BL
{
    public class NotificationManager:ManagerBase
    {
        //userToken: token of follower
        public static async Task UserFollowed(string userToken, string destUserId)
        {
            var user = await AccountManager.GetUserInfoAsync(userToken);
            NotificationHub.SendMessage($"You have a new follower: {user.FullName}", destUserId);
        }

        //userToken: token of liker
        public static async Task PostLiked(string userToken, string destUserId)
        {
            var user = await AccountManager.GetUserInfoAsync(userToken);
            NotificationHub.SendMessage($"{user.FullName} liked your post", destUserId);
        }
    }
}