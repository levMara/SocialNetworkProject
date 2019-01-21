using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication4.Models
{

    public class BlockedUsersViewModel
    {
        public string UnblockUserId { get; set; }
        public IEnumerable<UserIdAndName> Users { get; set; }
    }

    public enum MyFollowersAction
    {
        FollowBack = 1,
        Unfollow = 2,
        Block = 3
    }
    //a user who follows me
    public class FollowerModel: UserIdAndName
    {
        public FollowerModel() { }
        public FollowerModel(string userId, string userFullName,bool followed):base(userId,userFullName)
        {
            Followed = followed;
        }

        //I follow him
        public bool Followed { get; set; }
    }
    public class MyFollowersViewModel
    {
        public MyFollowersAction FollowingAction { get; set; }
        public string FollowBackUserId { get; set; }
        public string UnfollowUserId { get; set; }
        public string BlockUserId { get; set; }
        public IEnumerable<FollowerModel> Followers { get; set; }
    }

    public class MyFollowsViewModel
    {
        public string UnfollowUserId { get; set; }
        public IEnumerable<UserIdAndName> Follows { get; set; }
    }
}