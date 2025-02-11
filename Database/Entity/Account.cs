using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BaseAuth.Database.Entity;

public class Account
{
    [Key]
    [Column("id")]
    public int Id { get; set; }
    [Column("uuid")]
    public string Uuid { get; set; } = Guid.NewGuid().ToString();
    [Column("user_uuid")]
    public string UserUuid { get; set; }
    [Column("username")]
    public string Username { get; set; }
    [Column("password")]
    public string Password { get; set; }
    
    public User User { get; set; }
    public ICollection<Role> Roles { get; set; }
}