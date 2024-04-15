import { ComputerKey, UserRole, UserState } from "@/constants";

const util = {

   /**
   * gen ra màu cho trường state
   * @param {*} state
   */
   genColorState(key, value) {
      let color = "green";
      if (key == 'state') {
         switch (value) {
            case 0:
               color = "volcano";
               break;
            case 1:
               color = "green";
               break;
            case 2:
               color = "geekblue";
               break;
            default:
               break;
         }
      }
      return color
   },

   /**
    * gen text cho trường state
    * @param {*} state
    */
   genTextState(key, value) {
      let text = "Hỏng";
      if (key == 'state') {
         switch (value) {
            case 0:
               text = "Hỏng";
               break;
            case 1:
               text = "Tốt";
               break;
            case 2:
               text = "Bảo trì";
               break;
            default:
               break;
         }
      }

      return text;
   },

   /**
    * get text va color cho trường role của user
    * @param {*} role 
    * @returns 
    */
   getViewUserRole(role) {
      let colorUserRole = '';
      let textUserRole = '';
      switch (role) {
         case UserRole.Admin:
            colorUserRole = 'green';
            textUserRole = "Admin";
            break;
         case UserRole.Teacher:
            colorUserRole = 'blue';
            textUserRole = "Teacher";
         default:
            break;
      }
      return { colorUserRole, textUserRole };
   },

   /**
    * get text và color cho trường state của user
    * @param {*} state 
    * @returns 
    */
   getViewUserState(state) {
      let colorUserState = '';
      let textUserState = '';
      switch (state) {
         case UserState.Active:
            colorUserState = 'green';
            textUserState = "Đã kích hoạt";
            break;
         case UserState.Pending:
            colorUserState = 'orange';
            textUserState = "Đang chờ duyệt";
            break;
         case UserState.Revoke:
            colorUserState = 'red';
            textUserState = "Đã thu hồi";
         default:
            break;
      }
      return { colorUserState, textUserState };
   },

   /**
    * get text và color cho trường state của computer
    * @param {*} state 
    * @returns 
    */
   getViewComputerState(state) {
      let colorComputerState = '';
      let textComputerState = '';
      switch (state) {
         case ComputerKey.State.On:
            colorComputerState = 'green';
            textComputerState = "Bật";
            break;
         case ComputerKey.State.Off:
            colorComputerState = 'orange';
            textComputerState = "Tắt";
            break;
         default:
            break;
      }
      return { colorComputerState, textComputerState };
   },

   /**
    * get text và color cho trường state của computer
    * @param {*} state 
    * @returns 
    */
   getViewComputerCondition(condition) {
      let colorComputerCondition = '';
      let textComputerCondition = '';
      switch (condition) {
         case ComputerKey.Condition.Perfect:
            colorComputerCondition = 'green';
            textComputerCondition = "Hoàn hảo";
            break;
         case ComputerKey.Condition.Good:
            colorComputerCondition = 'blue';
            textComputerCondition = "Tốt";
            break;
         case ComputerKey.Condition.Normal:
            colorComputerCondition = 'cyan';
            textComputerCondition = "Bình thường";
            break;
         case ComputerKey.Condition.Bad:
            colorComputerCondition = 'red';
            textComputerCondition = "Hỏng";
            break;
         default:
            break;
      }
      return { colorComputerCondition, textComputerCondition };
   },
}

export default util