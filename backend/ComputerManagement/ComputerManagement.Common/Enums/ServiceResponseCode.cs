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
        WrongLogin= 4,
        UsernameConflict = 6,
        EmailConflict = 7,
        NotFoundUser = 8,
        NotFoundComputer = 9,
        ComputerRoomNameConflic = 10,
        NotFoundComputerRoom=11,
        UserPending=12,
        UserRevoked=13,
        Forbidden = 14,
        ConflicMacAddress = 15,
        MaxCapacityComputerRoom=16,
        ConflicNameComputer = 17,
        ConflicRowColComputerRooom = 18,
        ConflicRowColComputer = 19,
        NotFoundSoftwareModel = 20,
        ConflicFileVersion = 21,
        NotFoundFile = 22,
        InValidFileName = 23,
        ConflicMonitorSessionTime = 24,
        ConflicSoftwareName=25,
        ConflicSoftwareProcess=26,

    }
}
