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

        public class GetProfileVisitableUsersResult : ResultBase
        {
            public IEnumerable<UserIdAndName> Users { get; set; }

            public GetProfileVisitableUsersResult() { }
            public GetProfileVisitableUsersResult(bool success, IEnumerable<UserIdAndName> users, string userErrorMessage = null) : base(success, userErrorMessage)
            {
                Users = users;
            }

        }

        public class GetUserProfileResult : ResultBase
        {
            public ProfileViewModel Profile { get; set; }
            public GetUserProfileResult()
            {
            }

            public GetUserProfileResult(bool success, ProfileViewModel profile, string userErrorMesage = null) : base(success, userErrorMesage)
            {
                this.Profile = profile;
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

        public class FollowUserResult : ResultBase
        {
            public int NewFollowersCount { get; set; }

            public FollowUserResult() { }
            public FollowUserResult(bool success, int newFollowsResult, string userErrorMessage = null) : base(success, userErrorMessage)
            {
                NewFollowersCount = newFollowsResult;
            }
        }

        public class UnfollowUserResult : FollowUserResult
        {
            public UnfollowUserResult() { }
            public UnfollowUserResult(bool success, int newFollowersResult, string userErrorMessage = null) : base(success, newFollowersResult, userErrorMessage)
            {
            }
        }

        #endregion


        internal static async Task<bool> BlockUser(string userToken, string otherUserId)
        {
            var result = await socialServiceAccess.GetData($"useraction/block?token={userToken}&otherUserId={otherUserId}");
            return result.IsSuccessStatusCode;
        }

        internal static async Task<GetUserFollowsResult> GetUserFollows(string userToken)
        {
            var result = await socialServiceAccess.GetData<IEnumerable<UserIdAndName>>($"UserQueries/GetMyFollows?token={userToken}");
            if (result.Item1.IsSuccessStatusCode)
            {
                await ReplaceUsersNamesWithNamesFromIdentity(result.Item2);
                return new GetUserFollowsResult(true, new MyFollowsViewModel(result.Item2));
            }
            else
                return (await (ReturnErrorResult<GetUserFollowsResult>(result.Item1, "failed to get follows")));
        }

        internal static async Task<GetUserFollowersResult> GetUserFollowers(string userToken)
        {
            var result = await socialServiceAccess.GetData<IEnumerable<FollowerModel>>($"UserQueries/GetMyFollowers?token={userToken}");
            if (result.Item1.IsSuccessStatusCode)
            {
                await ReplaceUsersNamesWithNamesFromIdentity(result.Item2);
                return new GetUserFollowersResult(true, new MyFollowersViewModel(result.Item2));
            }
            else
                return (await (ReturnErrorResult<GetUserFollowersResult>(result.Item1, "failed to get followers")));
        }

        internal static async Task<GetBlockedUsersResult> GetBlockedUsers(string userToken)
        {
            var result = await socialServiceAccess.GetData<IEnumerable<UserIdAndName>>($"UserQueries/GetBlocked?token={userToken}");
            if (result.Item1.IsSuccessStatusCode)
            {
                await ReplaceUsersNamesWithNamesFromIdentity(result.Item2);
                return new GetBlockedUsersResult(true, new BlockedUsersViewModel(result.Item2));
            }
            else
                return (await (ReturnErrorResult<GetBlockedUsersResult>(result.Item1, "failed to get blocked users")));
        }

        internal static async Task<UnfollowUserResult> UnfollowUser(string userToken, string otherUserId)
        {
            var result = await socialServiceAccess.GetData<int>($"useraction/unfollow?token={userToken}&otherUserId={otherUserId}");
            if (result.Item1.IsSuccessStatusCode)
                return new UnfollowUserResult(true, result.Item2);
            else
                return new UnfollowUserResult(false, 0, "failed to get follow user");
        }

        internal static async Task<FollowUserResult> FollowUser(string userToken, string otherUserId)
        {
            var result = await socialServiceAccess.GetData<int>($"useraction/follow?token={userToken}&otherUserId={otherUserId}");
            if (result.Item1.IsSuccessStatusCode)
            {
                await NotificationManager.UserFollowed(userToken, otherUserId);
                return new FollowUserResult(true, result.Item2);
            }
            else
                return new FollowUserResult(false, 0, "failed to get no. of follows");
        }

        internal static async Task<bool> UnblockUser(string userToken, string otherUserId)
        {
            var result = await socialServiceAccess.GetData($"useraction/unblock?token={userToken}&otherUserId={otherUserId}");
            return result.IsSuccessStatusCode;
        }

        internal static async Task<GetProfileVisitableUsersResult> GetProfileVisitableUsers(string userToken)
        {
            var result = await socialServiceAccess.GetData<IEnumerable<UserIdAndName>>($"userqueries/GetProfileVisitableUsers?token={userToken}");

            if (result.Item1.IsSuccessStatusCode)
            {
                await ReplaceUsersNamesWithNamesFromIdentity(result.Item2);
                return new GetProfileVisitableUsersResult(true, result.Item2);
            }
            else
                return (await (ReturnErrorResult<GetProfileVisitableUsersResult>(result.Item1, "failed to get profile-visitable users")));
        }

        internal static async Task<GetUserProfileResult> GetUserProfile(string userToken, string userId)
        {

            var result = await socialServiceAccess.GetData<ProfileViewModel>($"UserQueries/GetProfile?token={userToken}&otherUserId={userId}");
            if (result.Item1.IsSuccessStatusCode)
            {
                var posts = result.Item2.Posts;
                await PostsManager.FillPostsWithComments(userToken, posts);
                await ReplaceUsersNamesWithNamesFromIdentity(result.Item2.Follows);
                await ReplaceUsersNamesWithNamesFromIdentity(result.Item2.Followers);
                return new GetUserProfileResult(true, result.Item2);
            }
            else
                return (await (ReturnErrorResult<GetUserProfileResult>(result.Item1, "failed to get user profile")));
        }

        private static async Task<bool> ReplaceUsersNamesWithNamesFromIdentity(IEnumerable<UserIdAndName> users)
        {
            var allUsersResult = await AccountManager.GetUserIdsAndNames();
            if (allUsersResult.Success)
            {
                foreach (var user in users)
                {
                    user.Name = allUsersResult.UserIdsAndNames.First(userIdAndName => userIdAndName.Id == user.Id).Name;
                }

                return true;
            }
            else
                return false;
        }
    }

}