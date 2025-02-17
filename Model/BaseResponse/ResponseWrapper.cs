using BaseAuth.Application;

namespace BaseAuth.Model.BaseResponse;

public class ResponseWrapper<T>
{
    public T Data { get; set; }
    public AppError Error { get; set; } = new(ErrorCode.Success);
}