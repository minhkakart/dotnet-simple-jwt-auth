﻿using BaseAuth.AppError;
using BaseAuth.Model.BaseResponse;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BaseAuth.Middleware;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public class ResponseWrappedAttribute : ActionFilterAttribute
{
    public ResponseWrappedAttribute()
    {
        Order = 100;
    }

    public override void OnActionExecuted(ActionExecutedContext context)
    {
        context.ExceptionHandled = true;
        context.Result = OnException(context.Exception, (context.Result as ObjectResult)?.Value);
    }
    
    public static ObjectResult OnException(Exception? ex, object? value)
    {
        if (ex != null)
        {
            return new ObjectResult(new ResponseWrapper
            {
                Data = value,
                Error = new AppError.AppError((ex as AppException)?.ErrorCode ?? ErrorCode.Unknown)
            });
        }
        
        return new ObjectResult(new ResponseWrapper
        {
            Data = ex is AppException ? ex.StackTrace : null,
            Error = new AppError.AppError()
        });
    }
}