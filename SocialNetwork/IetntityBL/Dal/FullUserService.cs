using IdentityBL;
using IdentityBL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IetntityBL.Interfaces;

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

        public void AddOrUpdate(FullUser fullUser)
        {
            _dynamoService.AddOrUpdate<FullUser>(fullUser);
            
        }

        public FullUser Get(string userId)
        {
            return _dynamoService.Get<FullUser>(userId);
        }

        internal IEnumerable<UserIdAndName> GetUserIdsAndNames()
        {
            var users = _dynamoService.Scan<FullUser>();
            List<UserIdAndName> ret = new List<UserIdAndName>();
            foreach (var user in users)
            {
                ret.Add(new UserIdAndName{ UserId = user.UserId,UserFullName=user.FullName });
            }
            return ret;
        }
    }
}
