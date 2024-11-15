using System.ComponentModel;

namespace BaseAuth.AppError;

public enum ErrorCode
{
    [Description("Lỗi không xác định.")]
    Unknown = -1,
    [Description("Thành công.")]
    Success,
    [Description("Yêu cầu sai!")]
    BadRequest,
    [Description("Xác thực thất bại.")]
    UnAuthorized,
    [Description("Thiếu quyền try cập.")]
    MissingRoles,
    [Description("Thiếu thông tin tài khoản hoặc mật khẩu.")]
    MissingUsernameOrPassword,
    [Description("Tài khoản hoặc mật khẩu không chính xác.")]
    UsernameOrPasswordIncorrect,
    [Description("Không có quyền truy cập.")]
    Forbidden,
    [Description("Không tìm thấy người dùng.")]
    UserNotFound,
    [Description("Thiếu tham số.")]
    MissingParameter,
    [Description("Tài khoản admin đã tồn tại.")]
    AdminAccountAlreadyExists
}