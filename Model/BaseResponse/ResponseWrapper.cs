using BaseAuth.Application;

namespace BaseAuth.Model.BaseResponse;

public class ResponseWrapper
{
    public object? Data { get; set; }
    public AppError Error { get; set; } = new(ErrorCode.Success);
}