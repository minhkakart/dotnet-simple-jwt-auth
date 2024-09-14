namespace BaseAuth.Database.Entity;

public class Account
{
    public int Id { get; set; }
    public string Uuid { get; set; }
    public string UserUuid { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    
    public virtual User User { get; set; }
    public virtual ICollection<Role> Roles { get; set; }
}