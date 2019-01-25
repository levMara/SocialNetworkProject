using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialBL.Exceptiones
{
    [Serializable]
    public class EntityNotExistsException : SocialException
    {

        public EntityNotExistsException(string msg)
                : base(msg)
        {

        }
    }
}
