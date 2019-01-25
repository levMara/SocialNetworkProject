using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialBL.Models
{
    public class UserFollowerModel : User
    {
        public bool UserFollowsHim { get; set; }
    }
}
