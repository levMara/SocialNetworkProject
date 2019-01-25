using SocialBL.Dal;
using SocialBL.Exceptiones;
using SocialBL.Interfaces;
using SocialBL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialBL.Manager
{
    public class UserActionManager : IUserSMng
    {
        //interface?  //singlton?
        Neo4jUserService _neo4jService;

        public UserActionManager()
        {
            _neo4jService = new Neo4jUserService();
        }


        public void Add(User user)
        {
            if (user == null)
                throw new IncorrectDetailsException("User empty");

            _neo4jService.Add(user);
        }

        public int Follow(string userId, string otherUserId)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(otherUserId))
                throw new IncorrectDetailsException("User id or other user id is empty");

            return _neo4jService.Follow(userId, otherUserId);
        }

        public int UnFollow(string userId, string otherUserId)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(otherUserId))
                throw new IncorrectDetailsException("User id or other user id is empty");

            return _neo4jService.UnFollow(userId, otherUserId);
        }

        public void Block(string userId, string otherUserId)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(otherUserId))
                throw new IncorrectDetailsException("User id or other user id is empty");

            _neo4jService.Block(userId, otherUserId);
        }

        public void UnBlock(string userId, string otherUserId)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(otherUserId))
                throw new IncorrectDetailsException("User id or other user id is empty");

            _neo4jService.UnBlock(userId, otherUserId);
        }


        
    }
}
