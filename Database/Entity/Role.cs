namespace BaseAuth.Database.Entity;

public class Role
{
    public int Id { get; set; }
    public string Uuid { get; set; }
    public string Name { get; set; }
    
    public virtual ICollection<Account>? Accounts { get; set; }
}