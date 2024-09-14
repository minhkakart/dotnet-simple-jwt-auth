using BaseAuth.Database;
using BaseAuth.Database.Entity;
using Microsoft.EntityFrameworkCore;

namespace BaseAuth.Service;

public interface IAccountService
{
    public int CreateAccount(string username, string password);
    public int InitializeAdminAccount();
}

public class AccountService(AppDbContext appDbContext) : IAccountService
{
    public int CreateAccount(string username, string password)
    {
        throw new NotImplementedException();
    }

    public int InitializeAdminAccount()
    {
        var adminRole = appDbContext.Roles.Include(role => role.Accounts).FirstOrDefault(role => role.Name == "Admin");
        
        if (adminRole.Accounts == null || adminRole.Accounts.Count == 0)
        {
            User user = new()
            {
                Uuid = Guid.NewGuid().ToString(),
                FirstName = "Admin",
                LastName = "Admin",
            };
            appDbContext.Users.Add(user);
            appDbContext.SaveChanges();
            Account adminAccount = new()
            {
                Uuid = Guid.NewGuid().ToString(),
                UserUuid = user.Uuid,
                Username = "admin",
                Password = BCrypt.Net.BCrypt.HashPassword("admin"),
                Roles = [adminRole]
            };
            appDbContext.Accounts.Add(adminAccount);
            appDbContext.SaveChanges();
            return 1;
        }

        return 0;
    }
}