using System.ComponentModel;

namespace BaseAuth.Application;

public enum ErrorCode
{
    [Description("Lỗi hệ thống!")]
    Unknown = -1,
    [Description("Lỗi khác.")]
    Other,
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
    AdminAccountAlreadyExists,
    [Description("Refresh token không hợp lệ.")]
    InvalidRefreshToken
}