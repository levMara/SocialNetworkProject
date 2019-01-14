using IdentityBL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IetntityBL.Interfaces
{
    public interface IIdentityMng
    {
        FullUser Get(string userId);

        FullUser Register(string userId ,FullUser user);
        FullUser Update(string userId, FullUser updateUser);
    }
}
