using System.ComponentModel;
using Microsoft.OpenApi.Extensions;

namespace BaseAuth.AppError;

public class AppError
{
    public int Code { get; set; }
    public string Message { get; set; }

    public AppError()
    {
        Code = ErrorCode.Success.GetHashCode();
        Message = ErrorCode.Success.GetAttributeOfType<DescriptionAttribute>().Description;
    }
    
    public AppError(ErrorCode error)
    {
        Code = error.GetHashCode();
        Message = error.GetAttributeOfType<DescriptionAttribute>().Description;
    }
}