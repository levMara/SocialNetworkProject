using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Interfaces
{
    public interface IRegister
    {
        bool IsUserNameValid(string userName);

        bool IsUserNameExist(string userName);

        bool IsPassValid(string pass);
    }
}
