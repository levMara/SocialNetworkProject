using SocialBL.Dal;
using SocialBL.Exceptiones;
using SocialBL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialBL.Manageres
{
    public class FeedManager
    {
        Neo4jFeedService _feedService;

        public FeedManager()
        {
            _feedService = new Neo4jFeedService();
        }


        public List<Post> GetFeed(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                throw new IncorrectDetailsException("User id empty");

           return _feedService.GetFeed(userId);
        }
    }
}
