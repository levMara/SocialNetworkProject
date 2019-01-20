using IdentityBL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IetntityBL.Interfaces
{

    public class UserIdAndName
    {
        public string UserId { get; set; }
        public string UserFullName { get; set; }
    }

    public interface IIdentityMng
    {
        FullUser Get(string userId);

        void RegisterOrUpdate(string userId ,FullUser user);
        IEnumerable<UserIdAndName> GetUserIdsAndNames();
    }
}
