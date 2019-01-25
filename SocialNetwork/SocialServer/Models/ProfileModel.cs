using SocialBL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocialServer.Models
{
    public class ProfileModel
    {
        public FullUser FullUser { get; set; }

        //if the one who requested the profile follows user
        public bool GetterFollowsUser { get; set; }

        public int FollowersCount { get; set; }

        public List<Post> Posts { get; set; }

        public List<UserFollowerModel> Followers { get; set; }

        public List<User> Follows { get; set; }
    }
}