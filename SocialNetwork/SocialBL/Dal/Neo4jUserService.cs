using Neo4jClient;
using Neo4jClient.Cypher;
using SocialBL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace SocialBL.Dal
{
    class Neo4jUserService
    {
        //singlton????
        protected GraphClient _client;
        protected string _uri = "http://ec2-18-221-16-181.us-east-2.compute.amazonaws.com:7474/db/data";
        string _user = "neo4j";
        string _pass = "lm770";

        public Neo4jUserService()
        {
            _client = new GraphClient(new Uri(_uri), _user, _pass);
            _client.Connect();
        }

        #region actin func
        public void Add(User newUser)
        {
            _client.Cypher
                .Merge("(user:User { Id: {id}})")
                .OnCreate()
                .Set("user = {newUser}")
                .WithParams(new
                {
                    id = newUser.Id,
                    newUser
                })
                .ExecuteWithoutResults();
        }

        //returns new number of followers of other user
        public int Follow(string userId, string otherUserId)
        {
            
            var data = _client.Cypher
                .Match("(u1:User)", "(u2:User)")
                .Where((User u1) => u1.Id == userId)
                .AndWhere((User u2) => u2.Id == otherUserId)
                .Merge("(u1)-[:Follow]->(u2)")
                .OnCreate()
                .Set("u2.FollowersCount = u2.FollowersCount + 1")
                .Return((u2) => u2.As<User>())
                .Results.FirstOrDefault();

            return data.FollowersCount;

            //_client.Cypher
            //    .Match("(u1:User)", "(u2:User)")
            //    .Where((User u1) => u1.Id == userId)
            //    .AndWhere((User u2) => u2.Id == otherUserId)
            //    .CreateUnique("(u1)-[:Follow]->(u2)")
            //    .ExecuteWithoutResults();
        }

        //returns new number of followers of other user
        public int UnFollow(string userId, string otherUserId)
        {
            var user =
               _client.Cypher
               .Match("(u2: User)")
               .Where((User u2) => u2.Id == otherUserId)
               .Return((u2) => u2.As<User>())
               .Results.FirstOrDefault();

            if (IsFollowerOf(userId, otherUserId)){
                _client.Cypher
                .Match("(u1:User)-[r:Follow]->(u2: User)")
                .Where((User u1) => u1.Id == userId)
                .AndWhere((User u2) => u2.Id == otherUserId)
                .Delete("r")
                .Set("u2.FollowersCount = u2.FollowersCount - 1")
                .ExecuteWithoutResults();
                return user.FollowersCount - 1;
            }
            else {
                return user.FollowersCount;
            }
        }

        internal int GetFollowersCount(string userId)
        {
            var data = _client.Cypher
            .Match("(u:User)")
            .Where((User u) => u.Id == userId)
            .Return((u) => u.As<User>())
            .Results.FirstOrDefault();
            return data.FollowersCount;
        }

        public void Block(string userId, string otherUserId)
        {
            _client.Cypher
                .Match("(u1:User)", "(u2:User)")
                .Where((User u1) => u1.Id == userId)
                .AndWhere((User u2) => u2.Id == otherUserId)
                .CreateUnique("(u1)-[:Block]->(u2)")
                .ExecuteWithoutResults();
            
                UnFollow(userId, otherUserId);
                UnFollow(otherUserId, userId);
        }

        public void UnBlock(string userId, string otherUserId)
        {
            _client.Cypher
                .Match("(u1:User)-[r:Block]->(u2: User)")
                .Where((User u1) => u1.Id == userId)
                .AndWhere((User u2) => u2.Id == otherUserId)
                .Delete("r")
                .ExecuteWithoutResults();
        }
        #endregion

        #region queries func
        public List<User> GetBlocked(string userId)
        {
            var query = _client.Cypher
                           .OptionalMatch("(u:User)-[:Block]->(blocked:User)")
                           .Where((User u) => u.Id == userId)
                           .Return((blocked) => new
                           {
                               blockedList = blocked.CollectAs<User>()
                           })
                           .Results.FirstOrDefault();

            return query.blockedList.ToList();
        }

        public List<UserFollowerModel> GetFollowers(string userId)
        {
            var query = _client.Cypher
                .Match("(user:User)<-[:Follow]-(follower:User)")
                .Where((User user) => user.Id == userId)
                .With("follower ,user as u,follower as f")
                .OptionalMatch("(u:User)-[r:Follow]->(f:User)")
                .Return((follower, r) => new UserFollowerModel
                {
                    Id = follower.As<User>().Id,
                    Name = follower.As<User>().Name,
                    UserFollowsHim = r != null
                }).Results.ToList();

            return query;
        }

        public List<User> GetFollows(string userId)
        {
            var query = _client.Cypher
                          .OptionalMatch("(u:User)-[:Follow]->(follow:User)")
                          .Where((User u) => u.Id == userId)
                          .Return((follow) => new
                          {
                              follows = follow.CollectAs<User>()
                          })
                          .Results.FirstOrDefault();

            return query.follows.ToList();
        }

        public List<User> GetProfileVisitableUsers(string userId)
        {
            return _client.Cypher
                        .Match("(u1:User)", "(u2:User)")
                        .Where((User u1) => u1.Id == userId)
                        .AndWhere("not (u2)-[:Block]-(u1)")
                        .Return((u2) => u2.As<User>())
                        .Results.ToList();
        }

        internal bool IsFollowerOf(string userId, string otherUserId)
        {
            var data = _client.Cypher
            .Match("(u:User {Id :'" + userId + "'})")
            .OptionalMatch("(u)-[f:Follow]->(other:User)")
            .Where("other.Id = '" + otherUserId + "'")
            .Return((u, other) => new
            {
                isFollow = Return.As<bool>("NOT (f IS NULL)")
            })
            .Results.FirstOrDefault();

            return data.isFollow;
            }
        #endregion
    }
}
