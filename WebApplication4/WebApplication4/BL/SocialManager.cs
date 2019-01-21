using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using WebApplication4.Models;

namespace WebApplication4.BL
{
    public class SocialManager : ManagerBase
    {
        private static WebAPIAccess socialServiceAccess = new WebAPIAccess("http://localhost:56139/api/");//TODO

        #region Result Models

        public class GetUserFollowsResult : ResultBase
        {
            public MyFollowsViewModel UserFollows { get; set; }

            public GetUserFollowsResult() { }
            public GetUserFollowsResult(bool success, MyFollowsViewModel userFollows, string userErrorMessage = null) : base(success, userErrorMessage)
            {
                UserFollows = userFollows;
            }

        }

        public class GetUserFollowersResult : ResultBase
        {
            public MyFollowersViewModel UserFollowers { get; set; }

            public GetUserFollowersResult() { }
            public GetUserFollowersResult(bool success, MyFollowersViewModel userFollowers, string userErrorMessage = null) : base(success, userErrorMessage)
            {
                UserFollowers = userFollowers;
            }
        }
        
        public class GetBlockedUsersResult : ResultBase
        {
            public BlockedUsersViewModel BlockedUsers { get; set; }

            public GetBlockedUsersResult() { }
            public GetBlockedUsersResult(bool success, BlockedUsersViewModel blockedUsers, string userErrorMessage = null) : base(success, userErrorMessage)
            {
                BlockedUsers = blockedUsers;
            }
        }

        #endregion

        internal static async Task<bool> UnfollowUser(string userToken, string otherUserId)
        {
            var result = await socialServiceAccess.GetData($"useraction/unfollow?token={userToken}&otherUserId={otherUserId}");
            return result.IsSuccessStatusCode;
        }

        internal static async Task<bool> BlockUser(string userToken, string otherUserId)
        {
            var result = await socialServiceAccess.GetData($"useraction/block?token={userToken}&otherUserId={otherUserId}");
            return result.IsSuccessStatusCode;
        }

        internal static async Task<GetUserFollowsResult> GetUserFollows(string userToken)
        {
            //TODO change
            MyFollowsViewModel x = new MyFollowsViewModel();
            x.Follows = new UserIdAndName[] { new UserIdAndName("56565655665656556565656", "My Friend User") };
            return new GetUserFollowsResult(true, x, null);
        }

        internal static async Task<GetUserFollowersResult> GetUserFollowers(string userToken)
        {
            //TODO change
            MyFollowersViewModel x = new MyFollowersViewModel();
            x.Followers = new FollowerModel[] { new FollowerModel("487878778787997854128", "My Friend User", true), new FollowerModel("11111111111111111111111", "Another User", false) };
            return new GetUserFollowersResult(true, x, null);
        }

        internal static async Task<GetBlockedUsersResult> GetBlockedUsers(string userToken)
        {
            //TODO change
            BlockedUsersViewModel x = new BlockedUsersViewModel();
            x.Users = new UserIdAndName[] { new UserIdAndName("46464646464646464646464", "My Old Enemy"), new UserIdAndName("464646565655656565565", "My New Enemy") };
            return new GetBlockedUsersResult(true, x, null);
        }

        internal static async Task<bool> FollowUser(string userToken, string otherUserId)
        {
            var result = await socialServiceAccess.GetData($"useraction/follow?token={userToken}&otherUserId={otherUserId}");
            return result.IsSuccessStatusCode;
        }

        internal static async Task<bool> UnblockUser(string userToken, string otherUserId)
        {
            var result = await socialServiceAccess.GetData($"useraction/unblock?token={userToken}&otherUserId={otherUserId}");
            return result.IsSuccessStatusCode;
        }
    }
}