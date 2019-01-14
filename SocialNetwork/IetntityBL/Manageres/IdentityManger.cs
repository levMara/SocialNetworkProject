
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

        public FullUser Register(string userId, FullUser user)
        {
            if (string.IsNullOrEmpty(userId) || user == null)
                throw new IncorrectDetailsException("User details or user id missing.");

            user.UserId = userId;
            FullUser addedUser =_fullUserService.Add(user);
            if(addedUser == null)
                throw new IdentityException("faild register to db.");
            return addedUser;
        }

        public FullUser Update(string userId, FullUser updateUser)
        {
            if (string.IsNullOrEmpty(userId) || updateUser == null)
                throw new IncorrectDetailsException("User details or user id missing.");

            FullUser updatedUser = _fullUserService.Update(userId, updateUser);
            if (updatedUser == null)
                throw new IdentityException("faild to update.");
            return updatedUser;
        }
    }
}
