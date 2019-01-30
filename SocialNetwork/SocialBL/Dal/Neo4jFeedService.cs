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
        private const int USERS_COUNT_TRIGGER=4;

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
            feed.AddRange(GetPostsFriendsLikeWithX(userId));
            feed.AddRange(GetUserPosts(userId));
            feed.AddRange(GetFollowsPosts(userId));
            feed.AddRange(GetMentionPosts(userId));
            feed.AddRange(GetMentionComment(userId));

            //distinction by post.Id
            feed= feed.Distinct(new PostEqComparator())
            .OrderByDescending(post => post.Date).ToList();
            return feed;
            
        }

        private List<Post> GetPostsFriendsLikeWithX(string userId)
        {
            var data = _client.Cypher
                .Match("(u:User {Id:'" + userId + "'})")
                .OptionalMatch("(u)-[:Follow]->(friend:User)-[lp:Like]->(p:Post)")
                .Where("p.Permission = 'all' and not (u)-[:Block]-(:User)-[:Write]->(p:Post)")
                .OptionalMatch("(u)-[:Follow]->(friend:User)-[lc:Like]->(c:Comment)<-[:Has_comment]-(p1:Post)")
                .Where("p1.Permission = 'all' and not (u)-[:Block]-(:User)-[:Write]->(p1:Post)-[:Has_comment]->(c:Comment)")
                .ReturnDistinct((lp, lc, p, p1) => new
                {
                    numOfPostLike = lp.Count(),
                    numOfCommentLike = lc.Count(),
                    posts = p.CollectAs<Post>(),
                    postsComment = p1.CollectAs<Post>()
                })
                .Results.FirstOrDefault();

            return CheckX(data.numOfPostLike, data.numOfCommentLike, data.posts, data.postsComment,USERS_COUNT_TRIGGER)??new List<Post>();
        }

        private List<Post> CheckX(long numOfPostLike, long numOfCommentLike, IEnumerable<Post> posts, IEnumerable<Post> postsComment, int x)
        {
            if (numOfPostLike < x & numOfCommentLike < x)
                return null;

            List<Post> postsList = new List<Post>();

            if (numOfPostLike > x & numOfCommentLike > x)
            {
                postsList.AddRange(posts);
                postsList.AddRange(postsComment);
            }
            else if (numOfPostLike > x)
                postsList.AddRange(posts);
            else
                postsList.AddRange(postsComment);

            return postsList;
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