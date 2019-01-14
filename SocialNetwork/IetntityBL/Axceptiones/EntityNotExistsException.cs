using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityBL.Exceptiones
{
    [Serializable]
    public class EntityNotExistsException : IdentityException
    {

        public EntityNotExistsException(string msg)
                : base(msg)
        {

        }
    }
}
