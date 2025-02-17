using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BaseAuth.Database.Entity;

public class User
{
    [Key]
    [Column("id")]
    public int Id { get; set; }
    [Column("uuid")]
    public string Uuid { get; set; } = Guid.NewGuid().ToString();
    [Column("first_name")]
    public string FirstName { get; set; }
    [Column("last_name")]
    public string LastName { get; set; }
    
    public Account Account { get; set; }
}