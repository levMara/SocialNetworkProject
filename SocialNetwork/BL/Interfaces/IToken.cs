using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Interfaces
{
    public interface IToken
    {
        bool Verify(string token);

        string DecodeUserId(string token);

        string RefreshToken(string token);
    }
}
