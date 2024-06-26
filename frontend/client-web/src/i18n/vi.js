export default {
   UnknownError: "Đã xảy ra lỗi",
   Add: "Thêm",
   Save: "Lưu",
   SaveAndAdd: "Lưu và thêm",
   Cancel: "Hủy",
   Edit: "Sửa",
   NotFound: "Không tìm thấy tài nguyên",
   SaveSuccess: "Lưu thành công",
   DetailInfo: "Thông tin chi tiết",
   ViewDetail: "Xem chi tiết",
   Close: "Đóng",
   UnSelect: "Bỏ chọn",
   SelectAllComputerRoom: "Chọn tất cả phòng máy",
   SelectAll: "Chọn tất cả",
   SelectCount: "Đã chọn {0} bản ghi",
   Delete: "Xóa",
   DeleteSuccess: "Xóa thành công",
   On: "Bật",
   Off: "Tắt",
   NotPermission: "Bạn không có quyền thực hiện chức năng này",
   SearchHinht: "Nhập từ khóa...",
   NotPerrmissionPage: "Bạn không có quyền truy cập trang này",
   BackHome: "Trờ lại trang chủ",
   Validate: {
      Required: "Vui lòng nhập {0}",
      AlreadyExist: "{0} đã tồn tại",
      InValid: "{0 không hợp lệ",
      Range: "{0} trong khoảng {1} - {2}",
      IntegerType: "{0} là kiểu số nguyên",
   },
   Action: "Chức năng",
   Yes: "Đồng ý",
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
      OldPassword: "Mật khẩu cũ",
      NewPassword: "Mật khẩu mới",
      ConfirmPasswordHint: "Xác nhận mật khẩu",
      YouHaveAccount: "Bạn có tài khoản ? ",
      Profile: "Hồ sơ",
      ChangePassword: "Đổi mật khẩu",
      Logout: "Đăng xuất",
      WrongLogin: "Sai tài khoản hoặc mật khẩu",
      LoginSuccess: "Đăng nhập thành công",
      RegisterSuccess: "Đăng ký tài khoản thành công .Vui lòng liên hệ admin để xác thực tài khoản",
      UsernameConflict: "Tài khoản đã tồn tại",
      EmailConflict: "Email đã được sử dụng",
      EmailInvalid: "Email không đúng định dạng",
      UserPending: "Tài khoản đang chờ duyệt, Vui lòng liên hệ quản trị viên",
      UserRevoked: "Tài khoản đã bị thu hồi, Vui lòng liên hệ quản trị viên",
      ChangePasswordSuccess: "Đổi mật khẩu thành công"
   },
   ComputerRoom: {
      Name: "Tên phòng máy",
      Row: "Số hàng",
      Col: "Số dãy",
      MaxCapacity: "Số lượng máy tối đa",
      State: "Tình trang",
      StateHint: "Vui lòng chọn tình trạng phòng máy",
      SearchListHint: "Nhập tên phòng máy...",
      CurrentCapacity: "Số máy",
      CreatedAt: "Ngày tạo",
      UpdatedAt: "Ngày cập nhật",
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
      DeleteSuccess: "Xóa phòng máy {0} thành công",

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
      ComputerAddTitle: "Thêm máy tính",
      ComputerInfoTitle: "Thông tin máy",
      Row: "Vị trí hàng",
      Col: "Vị trí cột",
      OS: "Hệ điều hành",
      CPU: "CPU",
      RAM: "RAM",
      HardDriver: "Ổ cứng",
      Condition: "Tình trạng máy",
      DeleteSuccess: "Xóa máy {0} ở phòng máy {1} thành công",
      State: "Trạng thái",
      StateTime: "Cập nhật trạng thái lần cuối",
      IsDowloaded: "Đã tải",
      IsInstalled: "Đã cài đặt",
      ListSoftware: "Danh sách phần mềm",
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
         ConflicRowColComputer: "Vị trí đã dược sử dụng",
         InValidRowColComputer: "Vị trí không hợp lệ"

      }
   },

   Software: {
      Name: "Tên phần mềm",
      Process: "Tên tiến trình",
      InstallationFileFolder: "Đường dẫn chứa file cài đặt",
      SoftwareFolder: "Thư mục chứa phần mềm",
      IsUpdate: "Cập nhật",
      IsInstall: "Cài đặt",
      CreatedAt: "Ngày tạo",
      UpdatedAt: "Ngày cập nhật",
      FileList: "Danh sách file cài",
      DeleteSuccess: "Xóa phần mềm {0} thành công",


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
   },
   MonitorSession: {
      ComputerRoomName: "Tên phòng máy",
      MonitorTypeLabel: "Loại giám sát",
      State: "Trạng thái",
      StartDate: "Thời gian bắt đầu",
      EndDate: "Thời gian kết thúc",
      Domain: "Tên miền cho phép",
      OwnerIdText: "Người sở hữu",
      Date: "Ngày",
      Session: "Phiên giám sát",
      SessionTime: "Giờ giám sát",
      QuickAddMonitorSessionTitle: "Thêm nhanh phiên giám sát",
      AddDomain: "Thêm tên miền",
      DetailInfo: "Thông tin chi tiết",
      ListComputer: "Danh sách máy",
      HistoryAccess: "Lịch sử truy cập",
      FileProof: "Danh sách minh chứng vi phạm",
      Validate: {
         ConflicMonitorSessionTime: "Phiên giám sát đã bị trùng thời gian"
      },
      MonitorType: {
         Practive: "Thực hành",
         Exam: "Kiểm tra"
      },
      StateType: {
         Running: "Đang hoạt động",
         YetStarted: "Chưa bắt đầu",
         Finished: "Đã kết thúc",
      }
   },
   ConfigOption: {
      OptionName: "Key cấu hình",
      OptionValue: "Giá trị",
      IsSystem: "Hệ thống",
      IsAgent: "Agent",
      Desc: "Mô tả",
      CreatedAt: "Ngày tạo",
      UpdatedAt: "Ngày cập nhật",
      AddTitle: "Thêm thiết lập",
      EditTtile: "Sửa thiết lập",
      Validate: {
         ConflicOptionName: "Tên thiết lập đã tồn tại",
         CantEditOptionNameSystem: "Không thể sửa tên của thiết lập hệ thống"
      }
   },
   Agent: {
      Version: "Phiên bản",
      IsUpdate: "Cập nhật",
      FileName: "Tên file",
      Size: "Dung lượng",
      AddAgentTitle: "Thêm agent",
      EditAgentTitle: "Sửa agent",
   },
   Student: {
      StudentCode: "Mã số sinh viên",
      StudentName: "Tên sinh viên",
      CreatedAt: "Ngày tạo",
      UpdatedAt: "Ngày cập nhật",
      AddTitle: "Thêm sinh viên",
      EditTitle: 'Sửa sinh viên',
      Validate: {
         ConflicStudentCode: "Mã số sinh viên bị trùng"
      }
   },
   FileProof: {
      StudentCode: "MSSV",
      StudentName: "Tên sinh viên",
      ComputerName: "Tên máy",
      CreatedAt: "Ngày tạo",
   },
   Sidebar: {
      Dashboard: "Tổng quan",
      ComputerRoomManager: "Quản lý phòng máy",
      ComputerManager: "Quản lý máy tính",
      UserManager: "Quản lý người dùng",
      SoftwareManager: "Quản lý phần mềm",
      FileManager: "Quản lý file cài",
      MonitorSessionManager: "Quản lý phiên giám sát",
      ConfigOptionManager: "Quản lý cấu hình",
      AgentManager: "Quản lý agent",
      StudentManager: "Quản lý sinh viên"
   },
   ChartComputerOnline: {
      Label: "Thống kế số lượng máy tính đang hoạt động",
      Labels: {
         ComputerOnline: "Số lượng máy online",
         ComputerTotal: "Tổng số máy"
      }
   },
   ChartComputerCondition: {
      Label: "Thống kế tình trạng máy tính",
      Labels: {
         Perfect: "Tốt",
         Hardware: "Lỗi phần cứng",
         Software: "Lỗi phần mềm",
         Network: "Lỗi mạng",
         OS: "Lỗi hệ điều hành",
         Total: "Tổng số máy"
      }
   }


}