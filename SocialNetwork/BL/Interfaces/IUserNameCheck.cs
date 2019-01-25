﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Interfaces
{
    public interface IUserNameCheck
    {
        bool IsUserNameValid(string userName);

        bool IsUserNameExist(string userName);        
    }
}
