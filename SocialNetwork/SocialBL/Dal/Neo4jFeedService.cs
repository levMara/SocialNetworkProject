using Neo4jClient;
using SocialBL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialBL.Dal
{
    class Neo4jFeedService
    {
        GraphClient _client;
        string _uri = "http://ec2-18-221-16-181.us-east-2.compute.amazonaws.com:7474/db/data";
        string _user = "neo4j";
        string _pass = "lm770";

        public Neo4jFeedService()
        {
            _client = new GraphClient(new Uri(_uri), _user, _pass);
            _client.Connect();
        }


        private class PostEqComparator : IEqualityComparer<Post>
        {
            public bool Equals(Post x, Post y)
            {
                return x.Id == y.Id;
            }

            public int GetHashCode(Post obj)
            {
                return obj.Id.GetHashCode();
            }
        }

        internal List<Post> GetFeed(string userId)
        {
            List<Post> feed = new List<Post>();
            feed.AddRange(GetUserPosts(userId));
            feed.AddRange(GetFollowsPosts(userId));
            feed.AddRange(GetMentionPosts(userId));
            feed.AddRange(GetMentionComment(userId));

            //distinction by post.Id
            feed= feed.Distinct(new PostEqComparator())
            .OrderByDescending(post => post.Date).ToList();
            return feed;
            
        }

        private List<Post> GetMentionComment(string userId)
        {
            var query = _client.Cypher
               .Match("(p:Post)-[:Has_comment]->(c:Comment)-[:Mention]->(u:User)")
               .Where((User u) => u.Id == userId)
               .Return((p) => p.As<Post>())
               .Results.ToList();

            return query;
        }

        private List<Post> GetMentionPosts(string userId)
        {
            var query = _client.Cypher
                .Match("(p:Post)-[:Mention]->(u:User)")
                .Where((User u) => u.Id == userId)
                .Return((p) => p.As<Post>())
                .Results.ToList();

            return query;
        }

        private List<Post> GetUserPosts(string userId)
        {
            var query = _client.Cypher
                .Match("(u:User)-[:Write]->(p:Post)")
                .Where((User u) => u.Id == userId)
                .Return((p) => p.As<Post>())
                .Results.ToList();

            return query;
        }


        private List<Post> GetFollowsPosts(string userId)
        {
            //Block??
            var query = _client.Cypher
                 .Match("(u:User)-[:Follow]->(other:User)-[:Write]->(p:Post)")
                 .Where((User u) => u.Id == userId)
                 .Return((p) => p.As<Post>())
                 .Results.ToList();

            return query;
        }
    }
}


//var query = _client.Cypher
//              .OptionalMatch("(u:User)-[:Follow]->(follow:User)")
//              .Where((User u) => u.Id == userId)
//              .Return((follow) => new
//              {
//                  user=u,
//follows = follow.CollectAs<User>()
//              })
//              .Results.FirstOrDefault();