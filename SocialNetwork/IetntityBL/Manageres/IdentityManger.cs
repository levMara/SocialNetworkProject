
using IdentityBL.Dal;
using IdentityBL.Exceptiones;
using IdentityBL.Models;
using IetntityBL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IetntityBL.Manageres
{
    public class IdentityManger : IIdentityMng
    {
        private FullUserService _fullUserService=null;

        public IdentityManger()
        {
            _fullUserService = new FullUserService();
        }

        public FullUser Get(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                throw new IncorrectDetailsException("User id empty.");

            FullUser user = _fullUserService.Get(userId);
            if (user == null)
                throw new EntityNotExistsException("User not found.");
            return user;
        }

        public IEnumerable<UserIdAndName> GetUserIdsAndNames()
        {
            return _fullUserService.GetUserIdsAndNames();
        }

        public void RegisterOrUpdate(string userId, FullUser user)
        {
            if (string.IsNullOrEmpty(userId) || user == null)
                throw new IncorrectDetailsException("User details or user id missing.");

            user.UserId = userId;
            _fullUserService.AddOrUpdate(user);
        }
        
    }
}
