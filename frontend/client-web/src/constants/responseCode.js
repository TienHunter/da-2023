const ResponseCode = {

   Success: 0,
   Warning: 1,
   UsernameTaken: 2,
   EmailTaken: 3,
   Error: 99,
   Exception: 999,
   WrongLogin: 4,
   UsernameConflict: 6,
   EmailConflict: 7,
   NotFoundUser: 8,
   NotFoundComputer: 9,
   ConflicComputerRoomName: 10,
   NotFoundComputerRoom: 11,
   UserPending: 12,
   UserRevoked: 13,
   Forbidden: 14,
   ConflicMacAddress: 15,
   MaxCapacityComputerRoom: 16,
   ConflicNameComputer: 17,
   ConflicRowColComputerRooom: 18,
   ConflicRowColComputer: 19,
   NotFoundSoftwareModel: 20,
   ConflicFileVersion: 21,
   NotFoundFile: 22,
   InValidFileName: 23,
   ConflicMonitorSessionTime: 24,
   ConflicSoftwareName: 25,
   ConflicSoftwareProcess: 26,
   InValidRowColComputer: 27,
   /// <summary>
   /// trùng config option name
   /// </summary>
   ConflicOptionName: 28,

   /// <summary>
   /// không thể sửa option name hệ thông
   /// </summary>
   CantEditOptionNameSystem: 29,

}
export default ResponseCode;