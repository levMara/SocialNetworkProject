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

    public class UserQueriesMangaer
    {
        Neo4jUserService _neo4jUserService;

        public UserQueriesMangaer()
        {
            _neo4jUserService = new Neo4jUserService();
        }
        

        public IEnumerable<UserFollowerModel> GetFollowers(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                throw new IncorrectDetailsException("User id empty");

            IEnumerable<UserFollowerModel> followers = _neo4jUserService.GetFollowers(userId);
            return followers;
        }

        public IEnumerable<User> GetFollows(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                throw new IncorrectDetailsException("User id empty");

            IEnumerable<User> follows = _neo4jUserService.GetFollows(userId);
            return follows;
        }

        public IEnumerable<User> GetBlocked(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                throw new IncorrectDetailsException("User id empty");

            IEnumerable<User> Blocked = _neo4jUserService.GetBlocked(userId);
            return Blocked;
        }

        public IEnumerable<User> GetProfileVisitableUsers(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                throw new IncorrectDetailsException("User id empty");

            IEnumerable<User> users = _neo4jUserService.GetProfileVisitableUsers(userId);

            return users;
        }

        public bool IsFollowerOf(string userId, string otherUserId)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(otherUserId))
                throw new IncorrectDetailsException("User id or other user id is empty");

            return _neo4jUserService.IsFollowerOf(userId, otherUserId);
        }

        public int GetFollowersCount(string userId)
        {
            if ( string.IsNullOrEmpty(userId))
                throw new IncorrectDetailsException("User id empty");

            return _neo4jUserService.GetFollowersCount(userId);
        }
    }
}
