using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Exceptiones
{
    [Serializable]
    public class DetailsNotValidException : UserException
    {

        public DetailsNotValidException(string msg)
            : base(msg)
        {
                
        }
    }
}
