using BaseAuth.Database;
using BaseAuth.Database.Entity;
using Microsoft.AspNetCore.Mvc;

namespace BaseAuth.Controllers;

[ApiController]
[Route("[controller]")]
public class RoleController(AppDbContext appDbContext) : ControllerBase
{
    [HttpGet(Name = "GetRoles")]
    public IEnumerable<string> Get()
    {
        return appDbContext.Roles.Select(r => r.Name).ToArray();
    }
    
    [HttpPost(Name = "CreateRole")]
    public async Task<IActionResult> Create([FromBody] string roleName)
    {
        var role = new Role()
        {
            Uuid = Guid.NewGuid().ToString(),
            Name = roleName
        };
        await appDbContext.Roles.AddAsync(role);
        await appDbContext.SaveChangesAsync();
        return Ok("Role created");
    }
}