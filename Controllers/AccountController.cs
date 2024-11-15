using BaseAuth.AppError;
using BaseAuth.Database;
using BaseAuth.Middleware;
using BaseAuth.Service;
using Microsoft.AspNetCore.Mvc;

namespace BaseAuth.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountController(AppDbContext appDbContext, IAccountService accountService) : AppController
{
    [HttpGet(Name = "GetAccounts")]
    [Authorised(["Admin"])]
    public IEnumerable<string> Get()
    {
        Console.WriteLine("GetAccounts");
        return appDbContext.Accounts.Select(a => a.Username).ToArray();
    }

    [HttpGet("initialize", Name = "InitializeAdminAccount")]
    public IActionResult InitializeAdminAccount()
    {
        var result = accountService.InitializeAdminAccount();
        if (result != 1)
        {
            throw new AppException(ErrorCode.AdminAccountAlreadyExists);
        }

        return Ok("Admin account created");
    }
}