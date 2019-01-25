using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Interfaces
{
    public interface ISetting
    {
        void ChangePassword(string userId, string newPass);
    }
}
