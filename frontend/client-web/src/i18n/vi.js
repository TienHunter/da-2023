export default {
   UnKnowError: "Đã xảy ra lỗi",
   Add: "Thêm",
   Save: "Lưu",
   SaveAndAdd: "Lưu và thêm",
   Cancel: "Hủy",
   Edit: "Sửa",
   NotFound: "Không tìm thấy tài nguyên",
   SaveSuccess: "Lưu thành công",
   DetailInfo: "Thông tin chi tiết",
   Validate: {
      Required: "Vui lòng nhập {0}",
   },
   auth: {
      Login: "Đăng nhập",
      UsernameHint: "Tài khoản hoặc email",
      UsernameRequired: "Vui lòng nhập tài khoản hoặc email",
      FullnameRequired: "Vui lòng nhập Họ và tên",
      UsernameRegsiterRequired: "Vui lòng nhập tài khoản",
      EmailRequired: "Vui lòng nhập Email",
      PasswordHint: "Mật khẩu",
      PasswordRequired: "Vui lòng nhập mật khẩu",
      ConfirmPasswordRequired: "Vui lòng nhập Xác nhận mật khẩu",
      WrongConfirmPassword: "Xác nhận mật khẩu không khớp",
      Forgotpassword: "Quên mật khẩu",
      YouDontHaveAccount: "Bạn chưa có tài khoản ?",
      Register: "Đăng ký",
      Or: "Hoặc",
      LoginWithGoogle: "Đăng nhập với Google",
      Fullname: "Họ và tên",
      UsernameHint: "Tài khoản",
      EmailHint: "Email",
      ConfirmPasswordHint: "Xác nhận mật khẩu",
      YouHaveAccount: "Bạn có tài khoản ? ",
      Profile: "Hồ sơ",
      Logout: "Đăng xuất",
      WrongLogin: "Sai tài khoản hoặc mật khẩu",
      LoginSuccess: "Đăng nhập thành công",
      RegisterSuccess: "Đăng ký tài khoản thành công .Vui lòng liên hệ admin để xác thực tài khoản",
      UsernameConflict: "Tài khoản đã tồn tại",
      EmailConflict: "Email đã được sử dụng",
      EmailInvalid: "Email không đúng định dạng",
      UserPending: "Tài khoản đang chờ duyệt, Vui lòng liên hệ quản trị viên",
      UserRevoked: "Tài khoản đã bị thu hồi, Vui lòng liên hệ quản trị viên"
   },
   ComputerRoom: {
      Name: "Tên phòng máy",
      Row: "Số hàng",
      Col: "Số dãy",
      MaxCapacity: "Số lượng máy tối đa",
      State: "Tình trang",
      StateHint: "Vui lòng chọn tình trạng phòng máy",
      Validate: {
         NameRequired: "Vui lòng nhập tên phòng máy",
         NameConflic: "Tên phòng máy đã tồn tại",
         MaxCapacityRequired: "Vui lòng nhập sô lượng máy tối đa",
         MaxCapacityRange: "Só lượng máy tối đa phải trong khoảng 1 - 100",
         StateRequired: "Vui lòng chọn tình trạng phòng máy",
         NotFound: "Không tìm thấy phòng máy",
         RowRequired: "Vui lòng nhập số lượng hàng",
         ColRequired: "Vui lòng nhập số lượng dãy",
         RowRange: "Số hàng trong khoảng {0} - {1}",
         ColRange: "Số dãy trên 1 dãy trong khoảng {0} - {1}",
         IntegerType: "{0} là kiểu số nguyên",
         Required: "Vui lòng nhập {0}",
         Range: "{0} trong khoảng {1} - {2}"

      },
      DeleteSuccess: "Xóa phòng máy {0} thành công"
   },
   User: {
      Username: "Tài khoản",
      Email: "Email",
      Fullname: "Họ và tên",
      Role: "Quyền",
      State: "Trạng thái tài khoản",
      NotFoundUser: "Không tìm thấy người dùng",
      RoleAdmin: "Quản trị viên",
      RoleTeacher: "Giảng viên",
      RoleNo: "Không có quyền",
      UserPending: "Chờ duyệt",
      UserActive: "Hoạt động",
      UserRevoked: "Đã thu hồi",
      UserState: {
         Pending: "Chờ duyệt",
         Active: "Kích hoạt",
         Revoke: "Thu hồi"
      }
   },
   Computer: {
      ComputerRoomName: "Tên phòng máy",
      Name: "Tên máy",
      MacAddress: "Địa chỉ mac",
      Condition: "Tình trạng máy",
      ComputerAddTitle: "Thêm máy tính",
      ComputerInfoTitle: "Thông tin máy",
      Row: "Vị trí hàng",
      Col: "Vị trí cột",
      Condition: "Tình trạng máy",
      ConditionKey: {
         Perfect: "Hoàn hảo",
         Good: "Tốt",
         Normal: "Bình thường",
         Bad: "Hỏng"
      },
      ComputerError: {
         Perfect: "Tốt",
         Hardware: "Lỗi phần cứng",
         Software: "Lỗi phần mềm",
         Network: "Lỗi mạng",
         OS: "Lỗi hệ điều hành",
         Unknow: "Không rõ nguyên nhân"
      },
      Validate: {
         ConflicNameComputer: "Tên máy đã tồn tại trong phòng máy",
         ConflicMacAddressComputer: "Địa chỉ mac address bị trùng",
         ConflicRowColComputer: "Vị trí không hợp lệ"
      }
   },

   Software: {
      Name: "Tên phần mềm",
      IsUpdate: "Cập nhật",
      IsInstall: "Cài đặt",
      CreatedAt: "Ngày tạo",
      UpdatedAt: "Ngày cập nhật",
      FileList: "Danh sách file cài",
      DeleteSuccess: "Xóa phần mềm {0} thành công"

   },
   File: {
      SoftwareName: "Tên phần mềm",
      Version: "Phiên bản",
      FileSource: "Chọn tệp",
      FileName: "Tên file",
      FileSize: "Dung lượng file",
      CreatedAt: "Ngày tạo",
      UpdatedAt: "Ngày sửa",
      AddFileTitle: "Thêm file cài đặt",
      DeleteSuccess: "Xóa file {0} thành công",
      Validate: {
         FileRequired: "Vui lòng tải file cài đặt lên"
      }
   }

}