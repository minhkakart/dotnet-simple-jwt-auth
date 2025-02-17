using BaseAuth.Application;
using BaseAuth.Database;
using BaseAuth.Database.Entity;
using BaseAuth.Extension;
using BaseAuth.Model.Dto;
using BaseAuth.Model.Request;

namespace BaseAuth.Service;

[ScopedService]
public interface IUserService
{
    List<UserInfo> GetAllUsers();
    UserInfo GetUserByUuid(string uuid);
    UserInfo CreateUser(UserRequest userInfo);
}

[ScopedService]
public class UserService(AppDbContext appDbContext) : IUserService
{
    public List<UserInfo> GetAllUsers()
    {
        var users = appDbContext.Users.ToList();
        return users.Select(user => new UserInfo
        {
            Uuid = user.Uuid,
            FirstName = user.FirstName,
            LastName = user.LastName
        }).ToList();
    }

    public UserInfo GetUserByUuid(string uuid)
    {
        var user = appDbContext.Users.FirstOrDefault(user => user.Uuid == uuid);
        if (user == null)
        {
            throw new AppException(ErrorCode.UserNotFound);
        }
        return new UserInfo
        {
            Uuid = user.Uuid,
            FirstName = user.FirstName,
            LastName = user.LastName
        };
    }

    public UserInfo CreateUser(UserRequest userInfo)
    {
        if (string.IsNullOrWhiteSpace(userInfo.FirstName) || string.IsNullOrWhiteSpace(userInfo.LastName))
        {
            throw new AppException(ErrorCode.MissingParameter);
        }
        
        var user = new User()
        {
            Uuid = Guid.NewGuid().ToString(),
            FirstName = userInfo.FirstName,
            LastName = userInfo.LastName
        };
        appDbContext.Users.Add(user);
        appDbContext.SaveChanges();
        return new UserInfo
        {
            Uuid = user.Uuid,
            FirstName = user.FirstName,
            LastName = user.LastName
        };
    }
}