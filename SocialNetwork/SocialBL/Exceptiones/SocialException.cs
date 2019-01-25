using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialBL.Exceptiones
{
    public class SocialException : Exception
    {

        public SocialException(string msg): base(msg)
        {
            
        }
    }


}
