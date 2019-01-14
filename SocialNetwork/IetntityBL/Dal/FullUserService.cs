using IdentityBL;
using IdentityBL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityBL.Dal
{
    class FullUserService
    {
        //config aws
        DynamoService _dynamoService;
        
        public FullUserService()
        {
            _dynamoService = new DynamoService();
        }

        public FullUser Add(FullUser fullUser)
        {
            _dynamoService.Add<FullUser>(fullUser);
            return fullUser;
        }

        public FullUser Get(string userId)
        {
            return _dynamoService.Get<FullUser>(userId);
        }

        public FullUser Update(string userId, FullUser updateUser)
        {
            return _dynamoService.Update<FullUser>(userId, updateUser);
        }
    }
}
