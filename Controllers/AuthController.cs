using BaseAuth.AppError;
using BaseAuth.Database;
using BaseAuth.Manager;
using BaseAuth.Middleware;
using BaseAuth.Model.Request;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BaseAuth.Controllers;

[ApiController]
[Route("api/[controller]")]
[ResponseWrapped]
public class AuthController(AppDbContext appDbContext) : ControllerBase
{
    [HttpPost("login", Name = "Login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        if (string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password))
        {
            throw new AppException(ErrorCode.MissingUsernameOrPassword);
        }

        // Check user's account
        var account = await appDbContext.Accounts.Include(acc => acc.Roles)
            .FirstOrDefaultAsync(acc => acc.Username == request.Username);

        if (account == null || !BCrypt.Net.BCrypt.Verify(request.Password, account.Password))
        {
            throw new AppException(ErrorCode.UsernameOrPasswordIncorrect);
        }

        var accountRoles = account.Roles.Select(r => r.Name).ToList();
        List<KeyValuePair<string, object>> claims = [];
        claims.AddRange([
            new KeyValuePair<string, object>("acc_uuid", account.Uuid),
            new KeyValuePair<string, object>("roles", accountRoles)
        ]);
        var token = TokenManager.GenerateToken(claims);

        return Ok(token);
    }

    [HttpPost("logout", Name = "Logout")]
    [Authorised]
    public IActionResult Logout()
    {
        var token = HttpContext.Request.Headers.Authorization.ToString().Substring("Bearer ".Length).Trim();

        TokenManager.RevokeToken(token);

        return Ok();
    }
}