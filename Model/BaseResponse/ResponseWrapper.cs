using BaseAuth.AppError;

namespace BaseAuth.Model.BaseResponse;

public class ResponseWrapper
{
    public object? Data { get; set; }
    public AppError.AppError Error { get; set; } = new(ErrorCode.Success);
}