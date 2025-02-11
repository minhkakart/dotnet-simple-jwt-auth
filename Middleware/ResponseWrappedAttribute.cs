using BaseAuth.Application;
using BaseAuth.Model.BaseResponse;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BaseAuth.Middleware;

[AttributeUsage(AttributeTargets.Class)]
public class ResponseWrappedAttribute : ActionFilterAttribute
{
    public ResponseWrappedAttribute()
    {
        Order = 100;
    }

    public override void OnActionExecuted(ActionExecutedContext context)
    {
        context.Result = OnException(context.Exception, (context.Result as ObjectResult)?.Value);
        context.ExceptionHandled = true;
    }
    
    public static ObjectResult OnException(Exception? ex, object? value)
    {
        if (ex == null)
        {
            return new ObjectResult(new ResponseWrapper
            {
                Data = value,
                Error = new AppError()
            });
        }
        
        return new ObjectResult(new ResponseWrapper
        {
            Data = ex is AppException ? null : ex.StackTrace,
            Error = new AppError((ex as AppException)?.ErrorCode ?? ErrorCode.Unknown)
        });
    }
}