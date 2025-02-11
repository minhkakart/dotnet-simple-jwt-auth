namespace BaseAuth.Application;

public class AppException (ErrorCode errorCode) : Exception
{
    public ErrorCode ErrorCode { get; } = errorCode;
}