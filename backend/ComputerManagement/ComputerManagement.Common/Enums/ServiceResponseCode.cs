using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerManagement.Common.Enums
{
    public enum ServiceResponseCode
    {
        Success = 0,
        Warning = 1,
        UsernameTaken = 2,
        EmailTaken = 3,
        Error = 99,
        Exception = 999,
        NotUserLogin = 4,
        WrongPassword=5,
        UsernameConflict = 6,
        EmailConflict = 7,

    }
}
