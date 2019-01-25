using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication4.Models
{

    public class BlockedUsersViewModel
    {
        public BlockedUsersViewModel()
        {
        }

        public BlockedUsersViewModel(IEnumerable<UserIdAndName> users)
        {
            Users = users;
        }

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
        public FollowerModel(string userId, string userFullName,bool userFollowsHim) :base(userId,userFullName)
        {
            UserFollowsHim = userFollowsHim;
        }

        //I follow him
        public bool UserFollowsHim { get; set; }
    }

    public class MyFollowersViewModel
    {
        public MyFollowersViewModel()
        {
        }

        public MyFollowersViewModel(IEnumerable<FollowerModel> followers)
        {
            Followers = followers;
        }

        public MyFollowersAction FollowingAction { get; set; }
        public string FollowBackUserId { get; set; }
        public string UnfollowUserId { get; set; }
        public string BlockUserId { get; set; }
        public IEnumerable<FollowerModel> Followers { get; set; }
    }

    public class MyFollowsViewModel
    {
        public MyFollowsViewModel()
        {
        }

        public MyFollowsViewModel( IEnumerable<UserIdAndName> follows)
        {
            Follows = follows;
        }

        public string UnfollowUserId { get; set; }
        public IEnumerable<UserIdAndName> Follows { get; set; }
    }
}