namespace BaseAuth.Database.Entity;

public class User
{
    public int Id { get; set; }
    public string Uuid { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    
    public virtual Account Account { get; set; }
}