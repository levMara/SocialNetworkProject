using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication4.Models
{
    public class ProfileViewModel
    {
        
        public FullUser FullUser { get; set; }

        //if the one who requested the profile follows user
        public bool GetterFollowsUser { get; set; }

        public int FollowersCount { get; set; }

        public List<PostWithCommentsModel> Posts { get; set; }

        public List<FollowerModel> Followers { get; set; }

        public List<UserIdAndName> Follows { get; set; }
    }
}