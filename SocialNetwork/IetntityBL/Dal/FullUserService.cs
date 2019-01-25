using IdentityBL;
using IdentityBL.Models;
using IetntityBL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityBL.Dal
{
    public class FullUserService
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

        public void AddOrUpdate(FullUser fullUser)
        {
            _dynamoService.AddOrUpdate<FullUser>(fullUser);
        }

        public FullUser Get(string userId)
        {
            return _dynamoService.Get<FullUser>(userId);
        }

        public FullUser Update(string userId, FullUser updateUser)
        {
            return _dynamoService.Update<FullUser>(userId, updateUser);
        }

        public IEnumerable<UserIdAndName> GetUserIdsAndNames()
        {
            var users = _dynamoService.Scan<FullUser>();
            List<UserIdAndName> ret = new List<UserIdAndName>();
            foreach (var user in users)
            {
                ret.Add(new UserIdAndName { Id = user.UserId, Name = user.FullName });
            }
            return ret;
        }

    }
}
