using BaseAuth.Database;
using BaseAuth.Manager;
using BaseAuth.Model.Request;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BaseAuth.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(AppDbContext appDbContext) : ControllerBase
{
    [HttpPost("login", Name = "Login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        // Validate request
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password))
        {
            return BadRequest("Username and password are required");
        }

        // Check user's account
        var account = await appDbContext.Accounts.Include(acc => acc.Roles)
            .FirstOrDefaultAsync(acc => acc.Username == request.Username);

        if (account != null && BCrypt.Net.BCrypt.Verify(request.Password, account.Password))
        {
            var accountRoles = account.Roles.Select(r => r.Name).ToList();
            var claims = new List<KeyValuePair<string, object>>();
            claims.Add(new KeyValuePair<string, object>("acc_uuid", account.Uuid));
            claims.Add(new KeyValuePair<string, object>("roles", accountRoles));
            var token = TokenManager.GenerateToken(claims);
            
            return Ok(token);
        }

        return Unauthorized("Invalid username or password");
    }
}