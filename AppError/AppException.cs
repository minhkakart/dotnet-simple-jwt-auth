namespace BaseAuth.AppError;

public class AppException (ErrorCode errorCode) : Exception
{
    public ErrorCode ErrorCode { get; } = errorCode;
}