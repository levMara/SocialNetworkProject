using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialBL.Exceptiones
{
    [Serializable]
    public class IncorrectDetailsException: SocialException
    {

        public IncorrectDetailsException(string msg)
                : base(msg)
        {

        }
    }
}
